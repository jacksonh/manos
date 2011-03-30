
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


#include "manos_tls.h"


gnutls_priority_t	priority_cache;
gnutls_dh_params_t	dh_params;

struct manos_tls_socket {
	gnutls_certificate_credentials_t		credentials;
	gnutls_session_t						tls_session;
	int										handshake_done;
	int										socket;
};


static int
get_dh_params (gnutls_session_t session, gnutls_params_type_t type, gnutls_params_st *res)
{
	if (type != GNUTLS_PARAMS_DH) {
		return -1;
	}

	res->type = type;
	res->deinit = 0;
	res->params.dh = dh_params;

	return 0;
}


int
manos_tls_global_init (const char *priorities)
{
	int err;

	priority_cache = NULL;
	dh_params = NULL;
   
	err = gnutls_global_init ();
	if (err != 0) {
		return -1;
	}

	err = gnutls_priority_init (&priority_cache, priorities, NULL);
	if (err != 0) {
		return -2;
	}

	return 0;
}

void
manos_tls_global_end ()
{
	gnutls_priority_deinit (priority_cache);
	gnutls_global_deinit ();
}

int
manos_tls_regenerate_dhparams (int bits)
{
	gnutls_dh_params_t params, oldparams;
	int err;

	err = gnutls_dh_params_init (&params);
	if (err != 0) {
		return -1;
	}

	err = gnutls_dh_params_generate2 (params, bits);
	if (err != 0) {
		gnutls_dh_params_deinit (params);
		return -2;
	}

	oldparams = dh_params;
	dh_params = params;

	if (oldparams != NULL) {
		gnutls_dh_params_deinit (oldparams);
	}

	return 0;
}

int
manos_tls_init (manos_tls_socket_t *tls, const char *cert, const char *key)
{
	manos_tls_socket_t socket;
	int err;

	socket = malloc (sizeof (*socket));
	if (socket == NULL) {
		return -1;
	}

	memset (socket, 0, sizeof (*socket));

	err = gnutls_certificate_allocate_credentials (&socket->credentials);
	if (err != 0) {
		return -2;
	}

	err = gnutls_certificate_set_x509_key_file (socket->credentials, cert, key, GNUTLS_X509_FMT_PEM);
	if (err != 0) {
		return -3;
	}

	gnutls_certificate_set_params_function(socket->credentials, get_dh_params);

	*tls = socket;
	return 0;
}

int
manos_tls_listen (manos_tls_socket_t tls, const char *host, int port, int backlog)
{
	int err;

	tls->socket = manos_socket_listen (host, port, backlog, &err);
	if (tls->socket == -1) {
		return err;
	}

	return 0;
}

static int
do_handshake (manos_tls_socket_t tls)
{
	int err;

	if (tls->handshake_done) {
		return 0;
	}

	err = gnutls_handshake (tls->tls_session);
	if (err != 0) {
		return err;
	} else {
		tls->handshake_done = 1;
		return 0;
	}
}

int
manos_tls_accept (manos_tls_socket_t server, manos_tls_socket_t *client, manos_socket_info_t *info)
{
	int err, inner_err;
	manos_tls_socket_t client_socket;
	
	err = manos_socket_accept (server->socket, info, &inner_err);
	if (err < 0) {
		if (inner_err == 0) {
			return -1;
		} else {
			return inner_err;
		}
	}

	client_socket = malloc (sizeof (*client_socket));
	if (client == NULL) {
		close (info->fd);
		return -1;
	}

	memset (client_socket, 0, sizeof (*client_socket));

	client_socket->socket = info->fd;
	client_socket->credentials = server->credentials;

	err = gnutls_init (&(client_socket->tls_session), GNUTLS_SERVER);
	if (err != 0) {
		close (info->fd);
		if (err != GNUTLS_E_MEMORY_ERROR) {
			gnutls_deinit (client_socket->tls_session);
		}
		free (client_socket);
		return -2;
	}

	gnutls_priority_set (client_socket->tls_session, priority_cache);
	gnutls_credentials_set (client_socket->tls_session, GNUTLS_CRD_CERTIFICATE, server->credentials);
	gnutls_transport_set_ptr (client_socket->tls_session, (gnutls_transport_ptr_t) info->fd);

	*client = client_socket;
	do_handshake(client_socket);

	return 0;
}

static int
tls_errno_or_default (int tlserror, int def, int again)
{
	if (tlserror == GNUTLS_E_AGAIN || tlserror == GNUTLS_E_INTERRUPTED) {
		return again;
	} else {
		return def;
	}
}

int
manos_tls_receive (manos_tls_socket_t tls, char *data, int len)
{
	int recvd, err;

	err = do_handshake (tls);
	if (err != 0) {
		return tls_errno_or_default (err, -2, -1);
	}

	recvd = gnutls_record_recv (tls->tls_session, data, len);
	if (recvd < 0) {
		return tls_errno_or_default (recvd, -3, -1);
	}

	return recvd;
}

int
manos_tls_send (manos_tls_socket_t tls, const char *data, int len)
{
	int sent, err;

	err = do_handshake (tls);
	if (err != 0) {
		return tls_errno_or_default (err, -2, -1);
	}

	sent = gnutls_record_send (tls->tls_session, data, len);
	if (sent < 0) {
		return tls_errno_or_default (sent, -3, -1);
	}

	return sent;
}

int
manos_tls_redo_handshake (manos_tls_socket_t tls)
{
	int err;

	err = gnutls_rehandshake (tls->tls_session);
	if (err != 0) {
		return -1;
	}

	tls->handshake_done = 0;
	return 0;
}

int
manos_tls_close (manos_tls_socket_t tls)
{
	int err;

	err = gnutls_bye (tls->tls_session, GNUTLS_SHUT_RDWR);
	close (tls->socket);
	free (tls);

	return 0;
}

