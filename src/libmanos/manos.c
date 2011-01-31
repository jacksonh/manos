


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

	free (msg.msg_iov);

	return rc;
}



enum {
	NO_FLAGS,
	SEND_LENGTH = 0x2
};


typedef struct {
	int fd;
	int socket;
	int flags;
	size_t length;
	size_t offset;
	length_cb cb;
	void *gchandle;
	ev_io *watcher;
} callback_data_t;

static int sendfile_complete_cb (eio_req *req);



static void
free_callback_data (callback_data_t *data)
{
	if (data->watcher)
		free (data->watcher);
	free (data);
}

static void
sendfile_on_ready (EV_P_ ev_io *watcher, int revents)
{
	callback_data_t *data = (callback_data_t *) watcher->data;

	eio_sendfile (data->socket, data->fd, data->offset, data->length - data->offset, 0, sendfile_complete_cb, data);

	ev_io_stop (EV_DEFAULT_UC_ watcher);
}

void
queue_sendfile (callback_data_t *data)
{
	if (!data->watcher) {
		data->watcher = malloc (sizeof (ev_io));
		memset (data->watcher, 0, sizeof (ev_io));

		ev_io_init (data->watcher, sendfile_on_ready, data->socket, EV_WRITE);
		data->watcher->data = data;
	}

	ev_io_start (EV_DEFAULT_UC_ data->watcher);
}


static int
sendfile_close_cb (eio_req *req)
{
	callback_data_t *data = (callback_data_t *) req->data;

	data->cb (data->gchandle, data->length, 0);

	free_callback_data (data);
	return 0;
}

static int
sendfile_complete_cb (eio_req *req)
{
	callback_data_t *data = (callback_data_t *) req->data;
	ssize_t length = req->result;

	if (length == -1) {
		if (req->errorno == EAGAIN ||  req->errorno == EINTR) {
			queue_sendfile (data);
			return 0;
		}

		eio_close (data->fd, 0, sendfile_close_cb, data);
		return 0;
	}

	if (length + data->offset < data->length) {
		data->offset += length;
		eio_sendfile (data->socket, data->fd, data->offset, data->length - data->offset, 0, sendfile_complete_cb, data);
		return 0;
	}

	eio_close (data->fd, 0, sendfile_close_cb, data);
	return 0;
}

static int
sendfile_stat_cb (eio_req *req)
{
	struct stat *buf = EIO_STAT_BUF (req);
	callback_data_t *data = (callback_data_t *) req->data;
	static char buffer [24];
	int buffer_len;

	data->length = buf->st_size;

	buffer_len = snprintf (buffer, 10, "%x\r\n", data->length);

	/* send the chunk length */
	if (send (data->socket, buffer, buffer_len, 0) != buffer_len) {
		data->cb (data->gchandle, -1, errno);
		return 0;
	}

	eio_sendfile (data->socket, data->fd, 0, data->length, 0, sendfile_complete_cb, data);

	return 0;
}

static int
sendfile_open_cb (eio_req *req)
{
	callback_data_t *data = (callback_data_t *) req->data;
	data->fd = EIO_RESULT (req);

	if (data->flags & SEND_LENGTH) 
		eio_fstat (data->fd, 0, sendfile_stat_cb, data);
	else
		eio_sendfile (data->socket, data->fd, 0, data->length, 0, sendfile_complete_cb, data);

	return 0;
}


static int
sendfile_internal (int socket, char *name, size_t length, int flags, length_cb cb, void *gchandle)
{
	callback_data_t *data = malloc (sizeof (callback_data_t));

	memset (data, 0, sizeof (callback_data_t));

	data->socket = socket;
	data->flags = flags;
	data->length = length;
	data->cb = cb;
	data->gchandle = gchandle;

	eio_open (name, O_RDONLY, 0777, 0, sendfile_open_cb, data);

	return 0;
}

int
manos_socket_send_file (int socket, char *name, int chunked, size_t length, length_cb cb, void *gchandle)
{
	return sendfile_internal (socket, name, length, chunked ? SEND_LENGTH : NO_FLAGS, cb, gchandle);
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
