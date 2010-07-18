Getting Started with Mango
==========================

This tutorial will get you writing your first Mango apps in no time.

We assume you already have Mango installed on your machine.  If you don't check out
the Installation Guide. To ensure you have Mango running try typing the command
mango-tool in a terminal.


Create your first Mango Project
-------------------------------

To create your first Mango application all you need to do is run the following command:

`mango-tool -init <YourApplicationName>`

Make sure you don't have any spaces in your application name.

This will create a folder named <YourApplicationName> and a few files and subfolders. Assumming
your application was named HelloWorld, you'll now have a structure something like this:


    HelloWorld/
    HelloWorld/Templates/
    HelloWorld/HelloWorldApp.cs
    HelloWorld/Deployment/
    HelloWorld/Deployment/Deploy.cs
    HelloWorld/StaticContentModule.cs

We'll worry more about these files later in the tutorial, but for now here's a
qiuck overview of what we've got:


 *    **Templates/**: This directory will contain all your templates for defining what your pages look like.
 *    **HelloWorldApp.cs**: This is the main entry point for your app.  Its the first thing that will be loaded by Mango.
 *    **Deployment/Deploy.cs**: This file will be used to control how your application is deployed from your desktop to a server.
 *    **StaticContentModule.cs**: The static content module is in charge of handling static content, things like css files and images.
If you are hosting your static content on a separate server you can remove this module.


Exploring HelloWorldApp.cs
--------------------------

The first file we are going to deal with is HelloWorldApp.cs.  This file is the main entry point into our application and is where we
handle our top level routing. Routes can be added to our app in the constructor or by using method attributes.  If you look at HelloWorldApp.cs
you'll see there has already been a route to the StaticContentModule added for you:

    Route ("Content/").Add (new StaticContentModule ());

The Route method will route all HTTP calls that match the supplied pattern, in this case "Content/" to the supplied module or action. Here we
have routed every url request that starts with "Content/" to the StaticContentModule.

If we just want to handle certain types of HTTP method requests we can use the corresponding Mango routing method. So if we just want to route
HTTP GET requests to the StaticContentModule, we could change the above code to look like this:

    Get ("Content/").Add (new StaticContentModule ());

We'll talk more about routes later.


Hello, Mango World
------------------

Routes don't necasarily have to go to modules, we can also route them to actions.  An action is any function that accepts a single IMangoContext
parameter.  So lets create our first action, add this line right above the static content route.

    Get ("$").Add (ctx => ctx.Response.Write ("Hello, Mango World!"));

What we've done here is created a route using the "$" regular expression, the "$" regex is used to denote the end of a line, so what we are doing here
is mapping to a blank URL, basically the home page of your site.

Instead of creating an action method to handle the request we've just used a simple lambda expression that takes the IMangoContext and sends
"Hello, Mango World!" as the response.


Building and Running Mango Apps
-------------------------------

To test out your application first you'll need to build it. You can do this using mango-tool:

    mango-tool -build

This will compile our application and any templates or modules that we happen to have in your application directory.  In this case all we've got is
the StaticContentModule and we haven't created any templates yet.  Once this is complete we'll have a HelloWorldApp.dll in our directory.

To run this app all we'll use the mango-tool again:

    mango-tool -run

Once the application is running you can check it out by navigating your browser to http://localhost:8080/


Moving On
---------

In part two of this tutorial we'll discuss routing a little more in-depth.







