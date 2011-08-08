Run your Manos app with SPDY
==================================

*Note: SPDY support in Manos is an experimental feature*


SSL:

1. Start Manos:

	manos -s -N 8080 -c certificate.crt -k keyfile.key

2. Start Chrome:

	chrome --use-spdy=ssl

3. Navigate to your app (https://localhost:8080)

No SSL:

1. Start Manos:

	manos -s -n 8080

2. Start Chrome:

	chrome --use-spdy=no-ssl

3. Navigate to your app (http://localhost:8080)

To view SPDY details, head to
[chrome://net-internals/#events&q=type:SPDY_SESSION%20is:active](chrome://net-internals/#events&q=type:SPDY_SESSION%20is:active).
