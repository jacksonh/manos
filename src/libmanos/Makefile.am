AUTOMAKE_OPTIONS = foreign no-dependencies


include_HEADERS = manos.h

pkglib_LTLIBRARIES = libmanos.la

libmanos_la_SOURCES = manos.c manos_socket.c manos_tls.c manos_libev.c


libmanos_la_LIBADD = ../libev/libev.la ../libeio/libeio.la

libmanos_la_LDFLAGS = -no-undefined $(GNUTLS_LIBS)
