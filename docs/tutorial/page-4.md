
Manos Futures
=============

Everything in the tutorial so far is implemented in Manos Rev 1.  Future revisions
will include a number of extra features and the tutorial will be expanded as features
are implemented.

Here are some of the things that are in the works:


Revision Two:
------------

### Windows Support
In Rev 1 of Manos, Windows is not supported as it doesn't support epoll. Rev 2 will include
a version of the IO layer that uses poll instead of epoll.

### Logging
An interface needs to be made available for logging.  Hopefully I can use one of the many
existing logging libraries.


### Timeouts
Applications need the ability to schedule events to occur at fixed intervals, something like:

    MyApp ()
    {
        AddTimeout (new TimeSpan (...), ctx => Cache.Clear ());
    }

### Exception/Error Handling Mechanism
An easy system for raising/handling errors needs to be implemented...


Revision Three:
--------------

### DataBase integration
This is the most glaring missing piece of Manos right now. I'm putting it off until some of
the lower level components are more complete, better tested and more thought out.

Likely there will be a tight integration with MongoDB. Users can easily use another DB system
if they want, but I'd like to have a dead simple, easy to use default system that requires
zero configuration.

### Memcached
Integration with memcached.


Revision Four:
-------------

### Simplified Deployment
manos-tool will be able to create standalone applications with a bundled web server.  These
applications can be run as a daemon on Linux or as a service on Windows.

### Cache-Control
A method of specifying cache options on actions, must likely attribute based.

### Validation
Attribute style validation, possibly using Fluent (http://fluentvalidation.codeplex.com/)


Revision Five:
-------------

### Math in the template engine
Support for performing math operations will be added to the template engine, so you can do things like

    {% if count > 10 * 2 %}
      .....
    {% endif %}

