Manos Pipes
===========

Pipes give your application an easy way to "do something" for every request/response transaction that goes into an application. Its a set of hooks into the Manos transaction processing that allows you to reshape requests and response before/after their actions are invoked.

A good example of a ManosPipe object would be authentication.  The authentication pipe looks at every request and determines whether the requestor has permission for the target they are attempting to invoke. If the pipe determines the user does not have permission it can redirect the response to a login page.

Pipe can also effect the response post view rendering. An example of a pipe module that does this may be an analytics pipe that would add a google analytics snippet to the bottom of every html page.

Your two mains concerns with ManosPipes are registration and the available API hooks.


Registration
------------

Registration is as simple as creating an instance of the ManosPipe object and calling ManosApp::RegisterPipe ()

    public MyApp ()
    {
        RegisterPipe (new MySpecialPipe ());
    }

C#'s property initializer syntax can be very useful for registering ManosPipes:

    public MyApp ()
    {
        RegisterPipe (new MySpecialPipe () {
				  LevelOfSpecialness = 10,
			   });
    }


The order that you register your pipes is very important.  Pipes are executed in a first registered, first executed order during the request portion of execution and a last registered first executed order during the response portion of execution.  Since pipes can effect the request/response data, in some cases it may be important to ensure the correct order of execution.

There are three main ways to register pipes in Manos.


    RegisterPipe (IManosPipe mw);
    RegisterPipe (string name, IManosPipe mw);

This first method is rather obvious. It simply adds the supplied pipe to the end of applicationss pipe list.

    RegisterPipeBefore (IManosPipe mw);
    RegisterPipeBefore (string name, IManosPipe mw);
    RegisterPipeAfter (IManosPipe mw);
    RegisterPipeAfter (string name, IManosPipe mw);

This method is a little more complicated. It inserts the supplied pipe into the apps pipe list at 'end of list' +/- 1. Typically this would only be called from another pipe object that wants another piece of pipe to execute before/after it has finished execution. Simply calling RegisterPipe from the constructor may not register the pipe at the correct spot, because its unknown whether or not the containing pipe has been added to the list yet. The code creating the pipe instance could look like this:

    IManosPipe foo = new PipeThatHasADep (this);
    RegisterPipe (foo);

or could look like this:

    RegisterPipe (new PipeThatHasADep (this));

If you think about whats going on there, in the first case any this.RegisterPipe calls made from PipeThatHasADep's constructor would be added before PipeThatHasADep and in the second case would be registered after PipeThatHasADep. The RegisterPipeBefore/After methods allow you to explicitly control this behavior.
    


    ReplacePipe (string name, IManosPipe mw);

This method takes advantage of the fact that all pipes can be registered with a unique name. If you want to replace a pipe that has been registered by Manos or by another pipe, you can call this method. For example ReplacePipe ("manos-auth", new MyAwesomeAuthSystem ()) allows you to replace Manos' built in auth system with a custom implementation.


Naming in Pipe
--------------

Optionally pipes can be registered with a unique name.  This name must be globally unique to the application or an exception will be thrown on registration. Its advisable that if you are writing pipes that register their own dependant pipes you register that pipe with a name, so users can easily replace that dependency.


Developing your own Pipes
--------------------------

To create your own pipes simply inherit from ManosPipe or implement the IManosPipe interface.  Typically inheriting from ManosPipe allows for cleaner code because you do not have to implement a method for each API hook.

Once you have inherited from ManosMiddlware you can override any or all of the following methods:

    ProcessRequest (IManosContext)

This method is called before anything at all has been done with the request. No routing has been done and no target has been invoked.

    PreProcessAction (IManosContext, IManosTarget)

This is called after the target has been looked up through routing. This method will not be called if no route is found.

    PostProcessAction (IManosContext)

This method is called directly after the ManosAction has ben invoked. At this point there should be a valid response object available and its likely that there is a buffer of rendered text that you could manipulate.

    ProcessError (IManosContext)

This method is invoked any time an error occurs. Typically a pipe would log the error and redirect the user to an error page.

