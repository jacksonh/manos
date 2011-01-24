


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


#define PARSE_ADDR(host, port, addr, addrlen) 	{					\
		static struct sockaddr_in in;						\
		static struct sockaddr_in6 in6;						\
	        memset (&in, 0, sizeof (in));						\
		memset (&in6, 0, sizeof (in6));						\
	        in.sin_port = in6.sin6_port = htons (port); 				\
		in.sin_family = AF_INET;  		  				\
		in6.sin6_family = AF_INET6;             				\
		int is_ipv4 = 1; 		           				\
		if (inet_pton (AF_INET, host, &(in.sin_addr)) <= 0) { 			\
			is_ipv4 = 0; 							\
			if (inet_pton(AF_INET6, host, &(in6.sin6_addr)) <= 0) { 	\
				return -1; 						\
			} 								\
		} 									\
		addr = is_ipv4 ? (struct sockaddr*)&in : (struct sockaddr*)&in6; 	\
		addrlen = is_ipv4 ? sizeof in : sizeof in6;   				\
	}


/*
typedef struct {
	struct ev_loop *loop;
	struct ev_idle eio_poll;
	struct ev_async eio_want_poll_watcher;
	struct ev_async eio_done_poll_watcher;
} manos_data_t;
*/

manos_data_t *
manos_init (struct ev_loop *loop)
{
	manos_data_t *res = malloc (sizeof (manos_data_t));

	memset (res, 0, sizeof (manos_data_t));

	
}

void
manos_shutdown (manos_data_t *data)
{

}



int
setup_socket (int fd)
{
	int flags = 1;
	setsockopt (fd, SOL_SOCKET, SO_REUSEADDR, (void *)&flags, sizeof (flags));

	return (fcntl (fd, F_SETFL, O_NONBLOCK) != -1);
}

int create_socket (int *err)
{
	int fd = socket (PF_INET, SOCK_STREAM, 0);
	if (fd < 0) {
		*err = errno;
		return -1;
	}

	if (!setup_socket (fd)) {
		*err = errno;
		close (fd);
		return -1;
	}

	return fd;
}

int
manos_socket_connect (char *host, int port, int *err)
{
	struct sockaddr* addr;
	ssize_t addrlen;
	int fd;

	
	fd = create_socket (err);
	if (fd < 0)
		return -1;

	PARSE_ADDR (host, port, addr, addrlen);

	int r = connect (fd, addr, addrlen);
	if (r < 0 && errno != EINPROGRESS) {
		*err = errno;
		return -1;
	}

	return fd;
}	

int
manos_socket_listen (char *host, int port, int backlog, int *err)
{
	struct sockaddr* addr;
	ssize_t addrlen;
	int fd, r;

	
	fd = create_socket (err);
	if (fd < 0)
		return -1;

	PARSE_ADDR (host, port, addr, addrlen);

	r = bind (fd, addr, addrlen);
	if (r < 0) {
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

int
manos_socket_accept (int fd, int *err)
{
	struct sockaddr_storage dummy;
	socklen_t len = sizeof (struct sockaddr_storage);
	int res;

	res = accept (fd, (struct sockaddr *) &dummy, &len);
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

	return res;
}


int
manos_socket_accept_many (int fd, int *fds, int len, int *err)
{
	int i = 0;
	
	for (i = 0; i < len; i++) {
		int a = manos_socket_accept (fd, err);
		if (a < 0 && *err == 0)
			return i; // Just a wouldblock error
		if (a < 0)
			return -1; // err will be set by manos_socket_accept
		*(fds + i) = a;
	}

	return i;
}


int
manos_socket_receive (int fd, char* buffer, int len, int *err)
{
	ssize_t rc;
	struct iovec iov [1];
	struct msghdr msg;

	memset (&msg, 0, sizeof (msg));
	memset (iov, 0, sizeof (iov));

	iov [0].iov_base = buffer;
	iov [0].iov_len = len;
	msg.msg_iov = iov;
	msg.msg_iovlen = 1;

	rc = recvmsg (fd, &msg, 0);
	if (rc < -1) {
		if (errno == EAGAIN || errno == EINTR) {
			*err = 0;
			return -1;
		}
		*err = errno;
		return -1;
	}

	return rc;
}


int
manos_socket_send (int fd, bytebuffer_t* buffers, int len, int* err)
{
	int i;
	ssize_t rc;
	struct msghdr msg;

	memset (&msg, 0, sizeof (msg));

	msg.msg_iovlen = len;
	msg.msg_iov = malloc (sizeof (struct iovec) * len);
	memset (msg.msg_iov, 0, sizeof (msg.msg_iov));

	for (i = 0; i < len; i++) {
		msg.msg_iov [i].iov_base = buffers [i].bytes + buffers [i].offset;
		msg.msg_iov [i].iov_len  = buffers [i].length;
	}

	rc = sendmsg (fd, &msg, 0);
	if ((int) rc < 0) {
		if (errno == EAGAIN || errno == EINTR) {
			*err = 0;
			return -1;
		}
		*err = errno;
		return -1;
	}

	return rc;
}


int
manos_socket_send_file (int socket_fd, int file_fd, off_t offset, int length, int *err)
{
	int res;
	int len = length;
	
#ifdef __linux__
	res = sendfile (socket_fd, file_fd, 0, length);
#elif defined(DARWIN)
	res = sendfile (file_fd, socket_fd, offset, &len, NULL, 0);
#endif	

	if (res != 0) {
		if (errno == EAGAIN || errno == EINTR) {
			*err = 0;
			return len;
		}
		*err = errno;
		return -1;
	}

	return len;
}


int
manos_socket_close (int fd, int *err)
{
	int rc = close (fd);
	if (rc < 0)
		*err = errno;
	return rc;
}

