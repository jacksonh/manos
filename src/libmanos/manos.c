
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

#include <gnutls/gnutls.h>

#ifdef HAVE_SYS_SENDFILE_H
#include <sys/sendfile.h>
#endif


#include "manos.h"



typedef struct {
	struct ev_loop *loop;
	struct ev_idle eio_idle_watcher;
} manos_data_t;

manos_data_t manos_data;



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




void
manos_init (struct ev_loop *loop)
{
	memset (&manos_data, 0, sizeof (manos_data_t));

	manos_data.loop = loop;

	ev_idle_init (&eio_idle_watcher, eio_on_idle);
	eio_idle_watcher.data = &manos_data;
	
	ev_async_init (&eio_want_poll_watcher, eio_on_want_poll);
	ev_async_start (EV_DEFAULT_UC_ &eio_want_poll_watcher);
	eio_want_poll_watcher.data = &manos_data;
	

	ev_async_init (&eio_done_poll_watcher, eio_on_done_poll);
        ev_async_start (EV_DEFAULT_UC_ &eio_done_poll_watcher);
	eio_done_poll_watcher.data = &manos_data;
	
	eio_init (eio_want_poll, eio_done_poll);
}

void
manos_shutdown ()
{
	ev_async_stop (manos_data.loop, &eio_want_poll_watcher);
	ev_async_stop (manos_data.loop, &eio_done_poll_watcher);
	ev_idle_stop (manos_data.loop, &eio_idle_watcher);
}


