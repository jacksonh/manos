


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



/*
 * I want these to be part of the manos_data_t but then how do i get them in
 * to eio_want_poll/eio_done_poll ?
 */
struct ev_async eio_want_poll_watcher;
struct ev_async eio_done_poll_watcher;
struct ev_idle eio_idle_watcher;


static void
eio_on_idle (EV_P_ ev_idle *watcher, int revents)
{
	if (eio_poll () != -1)
		ev_idle_stop (EV_A_ watcher);
}

static void
eio_on_want_poll (EV_P_ ev_async *watcher, int revents)
{
	manos_data_t *data = (manos_data_t *) watcher->data;
	if (eio_poll () == -1)
		ev_idle_start (EV_A_ &eio_idle_watcher);
}

static void
eio_on_done_poll (EV_P_ ev_async *watcher, int revents)
{
	manos_data_t *data = (manos_data_t *) watcher->data;
	if (eio_poll () != -1)
		ev_idle_stop (EV_A_ &eio_idle_watcher);
}

static void
eio_want_poll ()
{
	ev_async_send (EV_DEFAULT_UC_ &eio_want_poll_watcher);
}

static void
eio_done_poll ()		
{
	ev_async_send (EV_DEFAULT_UC_ &eio_done_poll_watcher);
}


manos_data_t *
manos_init (struct ev_loop *loop)
{
	manos_data_t *data = malloc (sizeof (manos_data_t));

	memset (data, 0, sizeof (manos_data_t));

	data->loop = loop;

	ev_idle_init (&eio_idle_watcher, eio_on_idle);
	eio_idle_watcher.data = data;
	
	ev_async_init (&eio_want_poll_watcher, eio_on_want_poll);
	ev_async_start (EV_DEFAULT_UC_ &eio_want_poll_watcher);
	eio_want_poll_watcher.data = data;
	

	ev_async_init (&eio_done_poll_watcher, eio_on_done_poll);
        ev_async_start (EV_DEFAULT_UC_ &eio_done_poll_watcher);
	eio_done_poll_watcher.data = data;
	
	eio_init (eio_want_poll, eio_done_poll);
}

void
manos_shutdown (manos_data_t *data)
{
	ev_async_stop (data->loop, &eio_want_poll_watcher);
	ev_async_stop (data->loop, &eio_done_poll_watcher);
	ev_idle_stop (data->loop, &eio_idle_watcher);

	free (data);
}



int
setup_socket (int fd)
{
	int flags = 1;
	setsockopt (fd, SOL_SOCKET, SO_REUSEADDR, (void *)&flags, sizeof (flags));

	return (fcntl (fd, F_SETFL, O_NONBLOCK) != -1);
}

static
int create_any_socket (char *host, int port, int type,
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

int create_dgram_socket (int ipv4, int *err)
{
	int fd = socket (ipv4 ? PF_INET : PF_INET6, SOCK_DGRAM, 0);
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
manos_socket_connect (char *host, int port, int *err)
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
manos_dgram_socket_listen (char *host, int port, int *err)
{
	int fd, r;

	fd = create_any_socket (host, port, SOCK_DGRAM, &bind_async);

	if (fd < 0) {
		*err = errno;
		return -1;
	}

	return fd;
}

int
manos_socket_listen (char *host, int port, int backlog, int *err)
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

	struct sockaddr_in *in4;
	struct sockaddr_in6 *in6;

	switch (addr.ss_family) {
	case AF_INET:
		in4 = (struct sockaddr_in *) &addr;
		info->port = ntohs (in4->sin_port);
		info->ipv4addr = in4->sin_addr.s_addr;
		info->is_ipv4 = 1;
		break;
	case AF_INET6:
		in6 = (struct sockaddr_in6 *) &addr;
		info->port = ntohs (in6->sin6_port);
		memcpy (info->address_bytes, in6->sin6_addr.s6_addr, 16);
		info->is_ipv4 = 0;
		break;
	}
	
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
manos_socket_receive_from (int fd, char* buffer, int len, int flags, manos_socket_info_t *info, int *err )
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

    struct sockaddr_in *in4;
    struct sockaddr_in6 *in6;

    switch (addr.ss_family) {
    case AF_INET:
        in4 = (struct sockaddr_in *) &addr;
        info->port = ntohs (in4->sin_port);
        info->ipv4addr = in4->sin_addr.s_addr;
		info->is_ipv4 = 1;
    break;
    case AF_INET6:
        in6 = (struct sockaddr_in6 *) &addr;
        info->port = ntohs (in6->sin6_port);
		memcpy (info->address_bytes, in6->sin6_addr.s6_addr, 16);
		info->is_ipv4 = 0;
    break;
    }

    return rc;
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
	if (rc == -1) {
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

	free (msg.msg_iov);

	return rc;
}



typedef struct {
	length_cb cb;
	void *gchandle;
} callback_data_t;



static void
free_callback_data (callback_data_t *data)
{
	free (data);
}

int
manos_socket_close (int fd, int *err)
{
	int rc = close (fd);
	if (rc < 0)
		*err = errno;
	return rc;
}


static int
file_get_length_stat_cb (eio_req *req)
{
	struct stat *buf = EIO_STAT_BUF (req);
	callback_data_t *data = (callback_data_t *) req->data;

	data->cb (data->gchandle, buf->st_size, 0);

	free_callback_data (data);
	return 0;
}


int
manos_file_get_length (char *path, length_cb cb, void *gchandle)
{
	callback_data_t *data = malloc (sizeof (callback_data_t));

	memset (data, 0, sizeof (callback_data_t));

	data->cb = cb;
	data->gchandle = gchandle;

	eio_stat (path, 0, file_get_length_stat_cb, data);

	return 0;
}
