#ifndef MANOS_SOCKET_H
#define MANOS_SOCKET_H

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




#endif

