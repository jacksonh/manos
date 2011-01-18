

typedef struct {
	int offset;
	int length;
	char* bytes;	
} bytebuffer_t;


int manos_socket_connect (char *host, int port, int *err);
int manos_socket_listen (char *host, int port, int backlog, int *err);
int manos_socket_accept (int fd, int *err);
int manos_socket_accept_many (int fd, int *fds, int len, int *err);
int manos_socket_receive (int fd, char* data, int len, int *err);
int manos_socket_send (int fd, bytebuffer_t *buffers, int len, int *err);
int manos_socket_send_file (int fd, int file_fd, int offset, int *err);
int manos_socket_close (int fd, int *err);

/*


int manos_socket_send (int fd, char** data, char** offsets);
int manos_socket_recieve (int fd, char** data, int len);
*/
