#ifndef MANOS_TLS_H
#define MANOS_TLS_H


#include <gnutls/gnutls.h>


#include "manos_socket.h"

typedef struct manos_tls_socket *manos_tls_socket_t;


int manos_tls_global_init (const char *priorities);
void manos_tls_global_end ();


int manos_tls_regenerate_dhparams (int bits);

int manos_tls_init (manos_tls_socket_t *tls, const char *cert, const char *key);

int manos_tls_listen (manos_tls_socket_t tls, const char *host, int port, int backlog);

int manos_tls_accept (manos_tls_socket_t tls, manos_tls_socket_t *client, manos_socket_info_t *info);

int manos_tls_receive (manos_tls_socket_t tls, char *data, int len);

int manos_tls_redo_handshake (manos_tls_socket_t tls);

int manos_tls_send (manos_tls_socket_t tls, const char *data, int len);

int manos_tls_close (manos_tls_socket_t tls);







#endif

