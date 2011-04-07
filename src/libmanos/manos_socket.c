
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

static int
create_any_socket (const char *host, int port, int type,
		int (socket_alive_cb)(int fd, struct sockaddr *addr, int addrlen))
{
	struct sockaddr_in in;
	struct sockaddr_in6 in6;
	int fd, ipv4;
	
	memset (&in, 0, sizeof (in));
	memset (&in6, 0, sizeof (in6));
	
	in.sin_port = in6.sin6_port = htons (port);
	in.sin_family = AF_INET;
	in6.sin6_family = AF_INET6;

	if (inet_pton (AF_INET, host, &(in.sin_addr)) > 0) {
		ipv4 = 1;
		fd = socket (PF_INET, type, 0);
	} else if (inet_pton(AF_INET6, host, &(in6.sin6_addr)) > 0) {
		ipv4 = 0;
		fd = socket (PF_INET6, type, 0);
	} else {
		return -1;
	}

	if (!setup_socket (fd)) {
		close (fd);
		return -1;
	}

	if (socket_alive_cb) {
		int r = socket_alive_cb (fd,
				ipv4 ? (struct sockaddr*) &in : (struct sockaddr*) &in6,
				ipv4 ? sizeof (in) : sizeof (in6));
		if (r != 0) {
			close (fd);
			return -1;
		}
	}

	return fd;
}

static int
connect_async (int fd, struct sockaddr *addr, int addrlen)
{
	int r = connect (fd, addr, addrlen);
	
	if (r < 0 && errno != EINPROGRESS) {
		return -1;
	}
	return 0;
}

int
manos_socket_connect (const char *host, int port, int *err)
{
	int fd = create_any_socket (host, port, SOCK_STREAM, &connect_async);

	if (fd < 0) {
		*err = errno;
		return -1;
	}

	return fd;
}	

static int
bind_async (int fd, struct sockaddr *addr, int addrlen)
{
	return bind (fd, addr, addrlen);
}

int
manos_dgram_socket_listen (const char *host, int port, int *err)
{
	int fd;

	fd = create_any_socket (host, port, SOCK_DGRAM, &bind_async);

	if (fd < 0) {
		*err = errno;
		return -1;
	}

	return fd;
}

int
manos_socket_listen (const char *host, int port, int backlog, int *err)
{
	int fd, r;

	fd = create_any_socket (host, port, SOCK_STREAM, &bind_async);

	if (fd < 0) {
		*err = errno;
		return -1;
	}

	r = listen (fd, backlog);
	if (r < 0) {
		*err = errno;
		return -1;
	}

	return fd;
}

static void
parse_sockaddr (struct sockaddr_storage *addr, manos_socket_info_t *info)
{
	struct sockaddr_in *in4;
	struct sockaddr_in6 *in6;

	switch (addr->ss_family) {
	case AF_INET:
		in4 = (struct sockaddr_in *) addr;
		info->port = ntohs (in4->sin_port);
		info->address.ipv4addr = in4->sin_addr.s_addr;
		info->is_ipv4 = 1;
		break;
	case AF_INET6:
		in6 = (struct sockaddr_in6 *) addr;
		info->port = ntohs (in6->sin6_port);
		memcpy (info->address.address_bytes, in6->sin6_addr.s6_addr, 16);
		info->is_ipv4 = 0;
		break;
	}
}

int
manos_socket_accept (int fd, manos_socket_info_t *info, int *err)
{
	struct sockaddr_storage addr;
	socklen_t len = sizeof (struct sockaddr_storage);
	int res;

	res = accept (fd, (struct sockaddr *) &addr, &len);
	if (res < 0) {
		if (errno == EAGAIN || errno == ECONNABORTED) {
			*err = 0;
			return -1;
		}
		*err = errno;
		return -1;
	}

	if (!setup_socket (res)) {
		*err = errno;
		close (res);
		return -1;
	}

	memset (info, 0, sizeof (*info));
	info->fd = res;

	parse_sockaddr (&addr, info);
	
	return res;
}

int
manos_socket_accept_many (int fd, manos_socket_info_t *infos, int len, int *err)
{
	int i = 0;
	
	for (i = 0; i < len; i++) {
		int a = manos_socket_accept (fd, &(infos [i]), err);

		if (a < 0 && *err == 0)
			return i; // Just a wouldblock error
		if (a < 0)
			return -1; // err will be set by manos_socket_accept
	}

	return i;
}

int
manos_socket_receive_from (int fd, char* buffer, int len, int flags, manos_socket_info_t *info, int *err)
{
    ssize_t rc;
       
    struct sockaddr_storage addr;
    socklen_t addrlen = sizeof (struct sockaddr_storage);

    rc = recvfrom( fd, buffer, len, flags, (struct sockaddr*)&addr, &addrlen );
    if (rc < 0 ) {
        if (errno == EAGAIN || errno == EINTR) {
        *err = 0;
        return -1;
        }
        *err = errno;
        return -1;
    }

	if (info) {
		parse_sockaddr (&addr, info);
	}

    return rc;
}


int
manos_socket_receive (int fd, char* buffer, int len, int *err)
{
	return manos_socket_receive_from (fd, buffer, len, 0, NULL, err);
}


int
manos_socket_send (int fd, const char *buffer, int offset, int len, int* err)
{
	ssize_t rc;

	rc = send (fd, buffer + offset, len, 0);

	if (rc < 0 && (errno == EAGAIN || errno == EINTR)) {
		*err = 0;
	} else {
		*err = errno;
	}
	return rc;
}


int
manos_socket_close (int fd, int *err)
{
	int rc = close (fd);
	if (rc < 0)
		*err = errno;
	return rc;
}

