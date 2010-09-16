Getting Started with Manos
==========================

This tutorial will get you writing your first Manos apps in no time.

We assume you already have Manos installed on your machine.  If you don't check out
the Installation Guide. To ensure you have Manos running try typing the manos -help
command in a terminal.

    manos -help


Create your first Manos Project
-------------------------------

To create your first Manos application all you need to do is run the following command:

`manos -init <YourApplicationName>`

Make sure you don't have any spaces in your application name.

This will create a folder named <YourApplicationName> and a few files and subfolders. Assuming
your application was named HelloWorld, you'll now have a structure something like this:


    HelloWorld/
    HelloWorld/Content/css/handheld.css
    HelloWorld/Content/css/style.css
    HelloWorld/Content/js/plugins.js
    HelloWorld/Content/js/dd_belatedpng.js
    HelloWorld/Content/js/jquery-1.4.2.min.js
    HelloWorld/Content/js/profiling
    HelloWorld/Content/js/profiling/charts.swf
    HelloWorld/Content/js/profiling/config.js
    HelloWorld/Content/js/profiling/yahoo-profiling.css
    HelloWorld/Content/js/profiling/yahoo-profiling.min.js
    HelloWorld/Content/js/script.js
    HelloWorld/Content/js/modernizr-1.5.min.js
    HelloWorld/Deployment/Deploy.cs
    HelloWorld/HelloWorldApp.cs
    HelloWorld/lib/Manos.dll
    HelloWorld/lib/nunit-framework.dll
    HelloWorld/StaticContentModule.cs
    HelloWorld/Templates/
    HelloWorld/Tests/
    HelloWorld/Tests/HelloWorldAppTest.cs


We'll worry more about these files later in the tutorial, but for now here's a
quick overview of what we've got:

 *    **Content/**: Your static content like images, js and css goes here.  The supplied files are based on the HTML5 Boilerplate project.
 *    **Templates/**: This directory will contain all your templates for defining what your pages look like.
 *    **HelloWorldApp.cs**: This is the main entry point for your app.  Its the first thing that will be loaded by Manos.
 *    **Deployment/Deploy.cs**: This file will be used to control how your application is deployed from your desktop to a server.
 *    **StaticContentModule.cs**: The static content module is in charge of handling static content, things like css files and images.
If you are hosting your static content on a separate server you can remove this module.


Exploring HelloWorldApp.cs
--------------------------

The first file we are going to deal with is HelloWorldApp.cs.  This file is the main entry point into our application and is where we
handle our top level routing. Routes can be added to our app in the constructor or by using method attributes.  If you look at HelloWorldApp.cs
you'll see there has already been a route to the StaticContentModule added for you:

    Route ("Content/", new StaticContentModule ());

The Route method will route all HTTP calls that match the supplied pattern, in this case "Content/" to the supplied module or action. Here we
have routed every url request that starts with "Content/" to the StaticContentModule.

If we just want to handle certain types of HTTP method requests we can use the corresponding Manos routing method. So if we just want to route
HTTP GET requests to the StaticContentModule, we could change the above code to look like this:

    Get ("Content/", new StaticContentModule ());

We'll talk more about routes later.


Hello, Manos World
------------------

Routes don't necessarily have to go to modules, we can also route them to actions.  An action is any function that accepts a single IManosContext
parameter.  So lets create our first action, add this line right above the static content route.

    Get ("/", ctx => ctx.Response.Write ("Hello, Manos World!"));

What we've done here is created a route using the "/" path. Routes can be created with simple paths like this or we could create more complex
routes with regular expressions. Something like Get ("/d.d", ...) would route "/dad" and "/dfd".

Instead of creating an action method to handle the request we've just used a simple lambda expression that takes the IManosContext and sends
"Hello, Manos World!" as the response.


Building and Running Manos Apps
-------------------------------

To test out your application first you'll need to build it. You can do this using manos:

    manos -build

This will compile our application and any templates or modules that we happen to have in your application directory.  In this case all we've got is
the StaticContentModule and we haven't created any templates yet.  Once this is complete we'll have a HelloWorldApp.dll in our directory.

To run this app all we'll use the manos again:

    manos -server

Once the application is running you can check it out by navigating your browser to http://localhost:8080/


Moving On
---------

In part two of this tutorial we'll add a template to our application.



