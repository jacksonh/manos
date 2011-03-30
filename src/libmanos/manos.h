
#ifndef MANOS_H
#define MANOS_H

#include <stdint.h>
#include <gnutls/gnutls.h>
		
#include "ev.h"
#include "eio.h"



void manos_init (struct ev_loop *loop);
void manos_shutdown ();


typedef struct {
	int offset;
	int length;
	char* bytes;	
} bytebuffer_t;

typedef struct {
	int32_t fd;
	int32_t port;
	int32_t is_ipv4;
	union {
		uint32_t ipv4addr;
		uint8_t  address_bytes[16];
	} address;
} manos_socket_info_t;



int manos_socket_connect (const char *host, int port, int *err);

int manos_socket_listen (const char *host, int port, int backlog, int *err);

int manos_dgram_socket_listen (const char *host, int port, int *err);

int manos_socket_accept (int fd, manos_socket_info_t *info, int *err);
int manos_socket_accept_many (int fd, manos_socket_info_t *infos, int len, int *err);

int manos_socket_receive (int fd, char* data, int len, int *err);
int manos_socket_receive_from (int fd, char* buffer, int len, int flags, manos_socket_info_t *info, int *err );

int manos_socket_send (int fd, bytebuffer_t *buffers, int len, int *err);

int manos_socket_close (int fd, int *err);



struct manos_tls_socket {
	gnutls_certificate_credentials_t		credentials;
	gnutls_session_t						tls_session;
	int										handshake_done;
	int										socket;
};

typedef struct manos_tls_socket *manos_tls_socket_t;


int manos_tls_global_init (const char *priorities);
void manos_tls_global_end ();


int manos_tls_regenerate_dhparams (int bits);

int manos_tls_init (manos_tls_socket_t *tls, const char *cert, const char *key);

int manos_tls_listen (manos_tls_socket_t tls, const char *host, int port, int backlog, int *err);

int manos_tls_accept (manos_tls_socket_t tls, manos_tls_socket_t *client, manos_socket_info_t *info);

int manos_tls_receive (manos_tls_socket_t tls, char *data, int len, int *err);

int manos_tls_redo_handshake (manos_tls_socket_t tls);

int manos_tls_send (manos_tls_socket_t tls, const bytebuffer_t *buffers, int len, int *err);

int manos_tls_close (manos_tls_socket_t tls);




#endif

