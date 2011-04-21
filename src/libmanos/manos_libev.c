
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <fcntl.h>
#include <errno.h>
#include <string.h>

#include "manos.h"



typedef struct {
	ev_io watcher;
	manos_watcher_cb *callback;
	const void *data;
} manos_io_watcher_t;

static void
manos_io_watcher_cb (struct ev_loop *loop, ev_io *w, int revents)
{
	manos_io_watcher_t *watcher = (manos_io_watcher_t*) w;
	watcher->callback (watcher->data, revents);
}

void*
manos_io_watcher_create (int fd, int events, manos_watcher_cb callback, const void *data)
{
	manos_io_watcher_t *watcher;

	watcher = malloc (sizeof (*watcher));
	memset (watcher, 0, sizeof (*watcher));

	ev_io_init (&watcher->watcher, manos_io_watcher_cb, fd, events);

	watcher->callback = callback;
	watcher->data = data;

	return watcher;
}

void
manos_io_watcher_destroy (void *watcher)
{
	free (watcher);
}



typedef struct {
	ev_async watcher;
	manos_watcher_cb *callback;
	const void *data;
} manos_async_watcher_t;

static void
manos_async_watcher_cb (struct ev_loop *loop, ev_io *w, int revents)
{
	manos_async_watcher_t *watcher = (manos_async_watcher_t*) w;
	watcher->callback (watcher->data, revents);
}

void*
manos_async_watcher_create (manos_watcher_cb callback, const void *data)
{
	manos_async_watcher_t *watcher;

	watcher = malloc (sizeof (*watcher));
	memset (watcher, 0, sizeof (*watcher));

	ev_async_init (&watcher->watcher, manos_async_watcher_cb);

	watcher->callback = callback;
	watcher->data = data;

	return watcher;
}

void
manos_async_watcher_destroy (void *watcher)
{
	free (watcher);
}




typedef struct {
	ev_check watcher;
	manos_watcher_cb *callback;
	const void *data;
} manos_check_watcher_t;

static void
manos_check_watcher_cb (struct ev_loop *loop, ev_io *w, int revents)
{
	manos_check_watcher_t *watcher = (manos_check_watcher_t*) w;
	watcher->callback (watcher->data, revents);
}

void*
manos_check_watcher_create (manos_watcher_cb callback, const void *data)
{
	manos_check_watcher_t *watcher;

	watcher = malloc (sizeof (*watcher));
	memset (watcher, 0, sizeof (*watcher));

	ev_check_init (&watcher->watcher, manos_check_watcher_cb);

	watcher->callback = callback;
	watcher->data = data;

	return watcher;
}

void
manos_check_watcher_destroy (void *watcher)
{
	free (watcher);
}



typedef struct {
	ev_idle watcher;
	manos_watcher_cb *callback;
	const void *data;
} manos_idle_watcher_t;

static void
manos_idle_watcher_cb (struct ev_loop *loop, ev_io *w, int revents)
{
	manos_idle_watcher_t *watcher = (manos_idle_watcher_t*) w;
	watcher->callback (watcher->data, revents);
}

void*
manos_idle_watcher_create (manos_watcher_cb callback, const void *data)
{
	manos_idle_watcher_t *watcher;

	watcher = malloc (sizeof (*watcher));
	memset (watcher, 0, sizeof (*watcher));

	ev_idle_init (&watcher->watcher, manos_idle_watcher_cb);

	watcher->callback = callback;
	watcher->data = data;

	return watcher;
}

void
manos_idle_watcher_destroy (void *watcher)
{
	free (watcher);
}




typedef struct {
	ev_prepare watcher;
	manos_watcher_cb *callback;
	const void *data;
} manos_prepare_watcher_t;

static void
manos_prepare_watcher_cb (struct ev_loop *loop, ev_io *w, int revents)
{
	manos_prepare_watcher_t *watcher = (manos_prepare_watcher_t*) w;
	watcher->callback (watcher->data, revents);
}

void*
manos_prepare_watcher_create (manos_watcher_cb callback, const void *data)
{
	manos_prepare_watcher_t *watcher;

	watcher = malloc (sizeof (*watcher));
	memset (watcher, 0, sizeof (*watcher));

	ev_prepare_init (&watcher->watcher, manos_prepare_watcher_cb);

	watcher->callback = callback;
	watcher->data = data;

	return watcher;
}

void
manos_prepare_watcher_destroy (void *watcher)
{
	free (watcher);
}




typedef struct {
	ev_timer watcher;
	manos_watcher_cb *callback;
	const void *data;
} manos_timer_watcher_t;

static void
manos_timer_watcher_cb (struct ev_loop *loop, ev_io *w, int revents)
{
	manos_timer_watcher_t *watcher = (manos_timer_watcher_t*) w;
	watcher->callback (watcher->data, revents);
}

void*
manos_timer_watcher_create (ev_tstamp after, ev_tstamp repeat, manos_watcher_cb callback, const void *data)
{
	manos_timer_watcher_t *watcher;

	watcher = malloc (sizeof (*watcher));
	memset (watcher, 0, sizeof (*watcher));

	ev_timer_init (&watcher->watcher, manos_timer_watcher_cb, after, repeat);

	watcher->callback = callback;
	watcher->data = data;

	return watcher;
}

void
manos_timer_watcher_set (void *watcher, ev_tstamp after, ev_tstamp repeat)
{
	manos_timer_watcher_t timer = watcher;

	ev_timer_set (timer->watcher, after, repeat);
}

void
manos_timer_watcher_destroy (void *watcher)
{
	free (watcher);
}


