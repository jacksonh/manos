
Middleware in Manos
===================

Middleware gives your application an easy way to "do something" for every request/response transaction that goes into an application. Its a set of hooks into the Manos transaction processing that allows you to reshape requests and response before/after their actions are invoked.

A good example of a middleware object would be authentication.  The authentication middleware looks at every request and determines whether the requestor has permission for the target they are attempting to invoke. If the middleware determines the user does not have permission it can redirect the response to a login page.

Middleware can also effect the response post view rendering. An example of a middleware module that does this may be an analytics middlware that would add a google analytics snippet to the bottom of every html page.

Your two mains concerns with middleware are registration and the available API hooks.


Registration
------------

Registration is as simple as creating an instance of the middleware object and calling MangoApp::RegisterMiddleware ()

    public MyApp ()
    {
        RegisterMiddleware (new MySpecialMiddleware ());
    }

C#'s property initializer syntax can be very useful for registering middleware:

    public MyApp ()
    {
        RegisterMiddleware (new MySpecialMiddleware () {
				  LevelOfSpecialness = 10,
			   });
    }


The order that you register your middleware is very important.  Middleware is executed in a first registered, first executed order during the request portion of execution and a last registered first executed order during the response portion of execution.  Since middleware can effect the request/response data, in some cases it may be important to ensure the correct order of execution.

There are three main ways to register middleware in Manos.


    RegisterMiddleware (IManosMiddleware mw);
    RegisterMiddleware (string name, IManosMiddleware mw);

This first method is rather obvious. It simply inserts the supplied middleware into the apps middleware list at the end of the list.

    RegisterMiddlewareBefore (IManosMiddleware mw);
    RegisterMiddlewareBefore (string name, IManosMiddleware mw);
    RegisterMiddlewareAfter (IManosMiddleware mw);
    RegisterMiddlewareAfter (string name, IManosMiddleware mw);

This method is a little more complicated. It inserts the supplied middleware into the apps middleware list at 'end of list' +/- 1. Typically this would only be called from another middleware object that wants another piece of middleware to execute before/after it has finished execution. Simply calling RegisterMiddleware from the constructor may not register the middleware at the correct spot, because its unknown whether or not the containing middleware has been added to the list yet. The code creating the middleware instance could look like this:

    IManosMiddleware foo = new MiddlewareThatHasADep (this);
    RegisterMiddleware (foo);

or could look like this:

    RegisterMiddleware (new MiddlewareThatHasADep (this));

If you think about whats going on there, in the first case any this.RegisterMiddleware calls made from MiddlewareThatHasADep's constructor would be added before MiddlewareThatHasADep and in the second case would be registered after MiddlewareThatHasADep. The RegisterMiddlewareBefore/After methods allow you to explicitly control this behavior.
    


    ReplaceMiddleware (string name, IManosMiddleware mw);

This method takes advantage of the fact that all middleware can be registered with a unique name. If you want to replace some middleware that has been registered by Manos or by another piece of middleware you can call this method. For example ReplaceMiddleware ("manos-auth", new MyAwesomeAuthSystem ()) allows you to replace Manos' built in auth system with custom middleware.


Naming in Middleware
--------------------

Optionally middleware can be registered with a unique name.  This name must be globally unique to the application or an exception will be thrown on registration. Its advisable that if you are writing middleware that registers its own dependant middleware you register that middleware with a name, so users can easily replace that dependency.


Developing your own Middleware
------------------------------

To create your own middleware simply inherit from ManosMiddleware or implement the IManosMiddleware interface.  Typically inheriting from ManosMiddleware allows for cleaner code because you do not have to implement a method for each API hook.

Once you have inherited from ManosMiddlware you can override any or all of the following methods:

    ProcessRequest (IManosContext)

This method is called before anything at all has been done with the request. No routing has been done and no target has been invoked.

    PreProcessAction (IManosContext, IManosTarget)

This is called after the target has been looked up through routing. This method will not be called if no route is found.

    PostProcessAction (IManosContext)

This method is called directly after the ManosAction has ben invoked. At this point there should be a valid response object available and its likely that there is a buffer of rendered text that you could manipulate.

    ProcessError (IManosContext)

This method is invoked any time an error occurs. Typically middleware would log the error and redirect the user to an error page.

