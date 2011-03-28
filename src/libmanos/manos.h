
#ifndef MANOS_H
#define MANOS_H

#include <stdint.h>
		
#include "ev.h"
#include "eio.h"


typedef struct {
	int offset;
	int length;
	char* bytes;	
} bytebuffer_t;

typedef struct {
	struct ev_loop *loop;
	struct ev_idle eio_idle_watcher;
} manos_data_t;

typedef struct {
	int32_t fd;
	int32_t port;
	int32_t is_ipv4;
	union {
		uint32_t ipv4addr;
		uint8_t  address_bytes[16];
	};
} manos_socket_info_t;


manos_data_t *manos_init (struct ev_loop *loop);
void manos_shutdown (manos_data_t *data);


int manos_socket_connect (char *host, int port, int *err);
int manos_socket_listen (char *host, int port, int backlog, int *err);
int manos_dgram_socket_listen (char *host, int port, int *err);
int manos_socket_accept (int fd, manos_socket_info_t *info, int *err);
int manos_socket_accept_many (int fd, manos_socket_info_t *infos, int len, int *err);
int manos_socket_receive (int fd, char* data, int len, int *err);
int manos_socket_receive_from (int fd, char* buffer, int len, int flags, manos_socket_info_t *info, int *err );
int manos_socket_send (int fd, bytebuffer_t *buffers, int len, int *err);
int manos_socket_close (int fd, int *err);

#endif

