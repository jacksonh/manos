Getting Started with Manos
==========================

This tutorial will get you writing your first Manos apps in no time.

We assume you already have Manos installed on your machine.  If you don't check out
the Installation Guide. To ensure you have Manos running try typing the manos -help
command in a terminal.

    manos -help

For this tutorial we are going to create a simple URL shortening service. It will allow
users to submit links for shortening and will also keep track of how many times a shortened
link has been clicked.


Create a Manos Project
-------------------------------

To create a Manos application all you need to do is run the following command:

    manos -init <YourApplicationName>

Make sure you don't have any spaces in your application name. An app name should just be letters,
digits and underscores. 

We'll call our app Shorty;

    manos -init Shorty

This will create a folder named Shorty and a few files and subfolders. Assuming
your application was named HelloWorld, you'll now have a structure something like this:


    Shorty/
    Shorty/Content/css/handheld.css
    Shorty/Content/css/style.css
    Shorty/Content/js/plugins.js
    Shorty/Content/js/dd_belatedpng.js
    Shorty/Content/js/jquery-1.4.2.min.js
    Shorty/Content/js/profiling
    Shorty/Content/js/profiling/charts.swf
    Shorty/Content/js/profiling/config.js
    Shorty/Content/js/profiling/yahoo-profiling.css
    Shorty/Content/js/profiling/yahoo-profiling.min.js
    Shorty/Content/js/script.js
    Shorty/Content/js/modernizr-1.5.min.js
    Shorty/Shorty.cs
    Shorty/StaticContentModule.cs


We'll worry more about these files later in the tutorial, but for now here's a
quick overview of what we've got:

 *    **Content/**: Static content like images, js and css goes here.  The supplied files are based on the HTML5 Boilerplate project.
 *    **Shorty.cs**: This is the main entry point for our app.  Its the first thing that will be loaded by Manos.
 *    **StaticContentModule.cs**: The static content module is in charge of handling static content, things like css files and images.
If you are hosting your static content on a separate server you can remove this module.


Exploring Shorty.cs
--------------------------

The first file we are going to deal with is Shorty.cs.  This file is the main entry point into our application and is where we
handle our top level routing. Routes can be added to our app in the constructor or by using method attributes.  If you look at Shorty.cs
you'll see there has already been a route to the StaticContentModule added for you:

    Route ("/Content/", new StaticContentModule ());

The Route method will route all HTTP calls that match the supplied pattern, in this case "/Content/" to the supplied module or action. Here we
have routed every url request that starts with "/Content/" to the StaticContentModule.

If we just want to handle certain types of HTTP method requests we can use the corresponding Manos routing method. So if we just want to route
HTTP GET requests to the StaticContentModule, we could change the above code to look like this:

    Get ("/Content/", new StaticContentModule ());

We'll talk more about routes later.


Hello, Manos World
------------------

Routes don't necessarily have to go to modules, we can also route them to actions.  An action is any function that accepts a single IManosContext
parameter.  So lets create our first action, add this line right above the static content route.

    Get ("/", ctx => ctx.Response.Write ("Hello, Manos World!"));

What we've done here is created a route using the "/" path.

Instead of creating an action method to handle the request we've just used a simple lambda expression that takes the IManosContext and sends
"Hello, Manos World!" as the response.

Routes can be created with simple paths like this or we could create more complex
routes with regular expressions. Something like Get ("/d.d", ...) would route "/dad" and "/dfd". There is also support for simple pattern matching
on routes, so things like Get ("/Article/{name}", ...).

Building and Running Manos Apps
-------------------------------

To test out your application first you'll need to build it. You can build our app using the gmcs compiler.

    gmcs -target:library -pkg:manos -out:Shorty.dll Shorty.cs StaticContentModule.cs

Note that we are using the gmcs -pkg option. This will add a reference for Manos.dll.

To run this app all we'll use the manos -server command. This command loads the local compiled manos app
and sets up hosting for it.

    manos -server

Once the application is running you can check it out by navigating your browser to http://localhost:8080/


Moving On
---------

In part two of this tutorial we'll start building our Shorty application.



