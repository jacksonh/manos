#include "config.h"

#ifdef false && HAVE_LIBGNUTLS


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

#include "manos.h"


gnutls_priority_t	priority_cache;
gnutls_dh_params_t	dh_params;


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
	gnutls_dh_params_deinit (dh_params);
	gnutls_global_deinit ();
}

int
manos_tls_regenerate_dhparams (int bits)
{
	gnutls_dh_params_t params, oldparams;
	int err;

	err = gnutls_dh_params_init (&params);
	if (err != 0) {
		return err;
	}

	err = gnutls_dh_params_generate2 (params, bits);
	if (err != 0) {
		gnutls_dh_params_deinit (params);
		return err;
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
		return ENOMEM;
	}

	memset (socket, 0, sizeof (*socket));

	err = gnutls_certificate_allocate_credentials (&socket->credentials);
	if (err != 0) {
		return err;
	}

	err = gnutls_certificate_set_x509_key_file (socket->credentials, cert, key, GNUTLS_X509_FMT_PEM);
	if (err != 0) {
		return err;
	}

	gnutls_certificate_set_params_function(socket->credentials, get_dh_params);

	*tls = socket;
	return 0;
}

int
manos_tls_listen (manos_tls_socket_t tls, const char *host, int port, int backlog, int *reserr)
{
	int err;

	*reserr = 0;
	tls->socket = manos_socket_listen (host, port, backlog, &err);
	if (tls->socket == -1) {
		*reserr = err;
		return -1;
	}

	return tls->socket;
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
			return EAGAIN;
		} else {
			return inner_err;
		}
	}

	client_socket = malloc (sizeof (*client_socket));
	if (client_socket == NULL) {
		close (info->fd);
		return ENOMEM;
	}

	memset (client_socket, 0, sizeof (*client_socket));

	client_socket->socket = info->fd;

	err = gnutls_init (&(client_socket->tls_session), GNUTLS_SERVER);
	if (err != 0) {
		close (info->fd);
		if (err != GNUTLS_E_MEMORY_ERROR) {
			gnutls_deinit (client_socket->tls_session);
		} else {
			err = ENOMEM;
		}
		free (client_socket);
		return err;
	}

	gnutls_priority_set (client_socket->tls_session, priority_cache);
	gnutls_credentials_set (client_socket->tls_session, GNUTLS_CRD_CERTIFICATE, server->credentials);
	gnutls_transport_set_ptr (client_socket->tls_session, (gnutls_transport_ptr_t) info->fd);

	*client = client_socket;
	do_handshake(client_socket);

	return 0;
}

static int
tls_errno_or_again (int tlserror)
{
	if (tlserror == GNUTLS_E_AGAIN || tlserror == GNUTLS_E_INTERRUPTED) {
		return 0;
	} else {
		return tlserror;
	}
}

int
manos_tls_receive (manos_tls_socket_t tls, char *data, int len, int *reserr)
{
	int recvd, err;

	*reserr = 0;

	err = do_handshake (tls);
	if (err != 0) {
		*reserr = tls_errno_or_again (err);
		return -1;
	}

	recvd = gnutls_record_recv (tls->tls_session, data, len);
	if (recvd < 0) {
		*reserr = tls_errno_or_again (recvd);
		return -1;
	}

	return recvd;
}

int
manos_tls_send (manos_tls_socket_t tls, const char *data, int offset, int len, int *reserr)
{
	int sent, err;

	*reserr = 0;

	err = do_handshake (tls);
	if (err != 0) {
		*reserr = tls_errno_or_again (err);
		return -1;
	}

	sent = gnutls_record_send (tls->tls_session, data + offset, len);
	if (sent < 0) {
		*reserr = tls_errno_or_again (sent);
		return -1;
	}

	return sent;
}

int
manos_tls_redo_handshake (manos_tls_socket_t tls)
{
	int err;

	err = gnutls_rehandshake (tls->tls_session);
	if (err != 0) {
		return err;
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
	if (tls->credentials) {
		gnutls_certificate_free_credentials (tls->credentials);
	}
	gnutls_deinit (tls->tls_session);
	free (tls);

	return 0;
}


#endif
