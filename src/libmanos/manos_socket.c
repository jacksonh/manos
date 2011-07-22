
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <fcntl.h>
#include <errno.h>
#include <string.h>

#include <sys/ioctl.h>
#include <sys/socket.h>
#include <sys/un.h>
#include <sys/types.h>

#include <arpa/inet.h>

#include <netdb.h>
#include <netinet/in.h>
#include <netinet/tcp.h>

#ifdef HAVE_SYS_SENDFILE_H
#include <sys/sendfile.h>
#endif


#include "manos.h"



static int
setup_socket (int fd)
{
	int flags = 1;
	setsockopt (fd, SOL_SOCKET, SO_REUSEADDR, (void *)&flags, sizeof (flags));

	return (fcntl (fd, F_SETFL, O_NONBLOCK) != -1);
}

static void
parse_sockaddr (struct sockaddr_storage *addr, manos_ip_endpoint_t *ep)
{
	struct sockaddr_in *in4;
	struct sockaddr_in6 *in6;

	switch (addr->ss_family) {
		case AF_INET:
			in4 = (struct sockaddr_in*) addr;
			ep->port = ntohs (in4->sin_port);
			memcpy (ep->address_bytes, &in4->sin_addr.s_addr, 16);
			ep->is_ipv4 = 1;
			break;

		case AF_INET6:
			in6 = (struct sockaddr_in6*) addr;
			ep->port = ntohs (in6->sin6_port);
			memcpy (ep->address_bytes, in6->sin6_addr.s6_addr, 16);
			ep->is_ipv4 = 0;
			break;
	}
}

static socklen_t
parse_endpoint (manos_ip_endpoint_t *ep, struct sockaddr_storage *addr)
{
	struct sockaddr_in *in4;
	struct sockaddr_in6 *in6;

	if (ep->is_ipv4) {
		in4 = (struct sockaddr_in*) addr;
		in4->sin_family = AF_INET;
		in4->sin_port = htons (ep->port);
		memcpy (&in4->sin_addr.s_addr, ep->address_bytes, 4);
		return sizeof (*in4);
	} else {
		in6 = (struct sockaddr_in6*) addr;
		in6->sin6_family = AF_INET6;
		in6->sin6_port = htons (ep->port);
		memcpy (in6->sin6_addr.s6_addr, ep->address_bytes, 16);
		return sizeof (*in6);
	}
}




int
manos_socket_localname_ip (int fd, manos_ip_endpoint_t *ep, int *err)
{
	struct sockaddr_storage addr;
	socklen_t len;
	int result;

	len = sizeof (addr);

	result = getsockname (fd, (struct sockaddr*) &addr, &len);

	parse_sockaddr (&addr, ep);

	*err = errno;
	return result;
}

int
manos_socket_peername_ip (int fd, manos_ip_endpoint_t *ep, int *err)
{
	struct sockaddr_storage addr;
	socklen_t len;
	int result;

	len = sizeof (addr);

	result = getpeername (fd, (struct sockaddr*) &addr, &len);

	parse_sockaddr (&addr, ep);

	*err = errno;
	return result;
}

int
manos_socket_create (int addressFamily, int protocolFamily, int *err)
{
	static int domains[] = { AF_INET, AF_INET6 };
	static int types[] = { SOCK_STREAM, SOCK_DGRAM };
	static int protocols[] = { IPPROTO_TCP, IPPROTO_UDP };

	int result = socket (domains[addressFamily], types[protocolFamily], protocols[protocolFamily]);

	if (result > 0 && setup_socket (result) < 0) {
		*err = errno;
		close (result);
		return -1;
	}

	*err = errno;
	return result;
}

int
manos_socket_bind_ip (int fd, manos_ip_endpoint_t *ep, int *err)
{
	struct sockaddr_storage addr;
	socklen_t len;
	int result;

	len = parse_endpoint (ep, &addr);

	result = bind (fd, (struct sockaddr*) &addr, len);

	*err = errno;
	return result;
}

int
manos_socket_connect_ip (int fd, manos_ip_endpoint_t *ep, int *err)
{
	struct sockaddr_storage addr;
	socklen_t len;
	int result;

	len = parse_endpoint (ep, &addr);

	result = connect (fd, (struct sockaddr*) &addr, len);
	
	if (result < 0 && errno == EINPROGRESS) {
		*err = 0;
		return 0;
	} else {
		*err = errno;
		return result;
	}
}

int
manos_socket_listen (int fd, int backlog, int *err)
{
	int result;

	result = listen (fd, backlog);
	*err = errno;
	return result;
}

int
manos_socket_accept (int fd, manos_ip_endpoint_t *remote, int *err)
{
	struct sockaddr_storage addr;
	socklen_t len = sizeof (struct sockaddr_storage);
	int result;

	result = accept (fd, (struct sockaddr*) &addr, &len);
	if (result < 0) {
		if (errno == EAGAIN || errno == ECONNABORTED) {
			*err = 0;
			return -1;
		}
		*err = errno;
		return -1;
	}

	if (!setup_socket (result)) {
		*err = errno;
		close (result);
		return -1;
	}

	parse_sockaddr (&addr, remote);
	
	return result;
}

int
manos_socket_send (int fd, const char *buffer, int offset, int len, int* err)
{
	ssize_t rc;

	rc = send (fd, buffer + offset, len, 0);

	if (rc < 0 && (errno == EAGAIN)) {
		*err = 0;
	} else {
		*err = errno;
	}
	return rc;
}

int
manos_socket_receive (int fd, char* buffer, int len, int *err)
{
	int result;

	result = recv (fd, buffer, len, 0);

	if (result < 0 && (errno == EAGAIN)) {
		*err = 0;
	} else {
		*err = errno;
	}
	return result;
}

int
manos_socket_sendto_ip (int fd, const char *buffer, int offset, int len, manos_ip_endpoint_t *to, int *err)
{
	int result;
	struct sockaddr_storage target;
	socklen_t slen;

	slen = parse_endpoint (to, &target);

	result = sendto (fd, buffer + offset, len, 0, (struct sockaddr*) &target, slen);

	if (result < 0 && (errno == EAGAIN)) {
		*err = 0;
	} else {
		*err = errno;
	}
	return result;
}

int
manos_socket_receivefrom_ip (int fd, char* data, int len, manos_ip_endpoint_t *from, int *err)
{
	int result;
	struct sockaddr_storage source;
	socklen_t slen = sizeof (source);

	result = recvfrom (fd, data, len, 0, (struct sockaddr*) &source, &slen);

	if (result < 0 && (errno == EAGAIN)) {
		*err = 0;
		return -1;
	} else {
		*err = errno;
		if (from) {
			parse_sockaddr (&source, from);
		}
		return result;
	}
}

int
manos_socket_close (int fd, int *err)
{
	int result = close (fd);
	*err = errno;
	return result;
}





