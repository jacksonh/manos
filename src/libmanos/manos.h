
#ifndef MANOS_H
#define MANOS_H

#include <stdint.h>
		
#include "ev.h"
#include "eio.h"

#include "manos_socket.h"
#include "manos_tls.h"


void manos_init (struct ev_loop *loop);
void manos_shutdown ();


#endif

