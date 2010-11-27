Getting Started with Manos
==========================

This is page three of the Getting Started with Manos tutorial.  It assumes you already have
Manos installed and have created the Hello World App from page one and the templates on page two.


Testing Manos Applications
--------------------------

Manos was designed in a way to make testing every aspect of an application dead simple. We can
easily test our routing tables, application logic and templates using nunit mocks and Manos's
interfaced based infrastructure.

Before we get started testing our application though, we're going to need to restructure our
routing code though.


Using Attributes for Routing
----------------------------

Using the Get method in a our application constructor is an easy way to hook up to an HTTP method
but it makes it difficult to test our code. To make our code easily testable we will want to
create a separate method for our action:

    public static void Hello (IManosContext ctx)
    {
        ctx.Render ("index.html", new {
            Name = "Manos"
        });
    }

Manos will automatically route your Hello method to the "Hello" URI, so if you rebuild your
application and navigate too http://localhost:8080/Hello we'll see our Hello World page again.

If we want to map our action to a different URL, we can use the HTTP Attributes to map any (or all)
HTTP methods to our method:

    [Get ("/")]
    public static void Hello (IManosContext ctx)
    {
        ctx.Render ("index.html", new {
            Name = "Manos"
        });
    }


Testing Actions with ManosTestContext
-------------------------------------

Manos includes a number of built in nunit mocking objects that we can use to build tests.  Here
is a simple test to check the output of our Hello Action.

    [Test]
    public void Hello_Invoked_PropertySet ()
    {
        var ctx = new ManosTestContext ();

        HelloWorldApp.Hello (ctx);

        Assert.Assert (ctx.PropertySet ("Name"), "is prop set");
        Assert.AreEqual ("Manos", ctx.PropertyValue ("Name"));
    }

    [Test]
    public void Hello_Invoked_HelloHtmlRendered ()
    {
        var ctx = new ManosTestContext ();

        HelloWorldApp.Hello (ctx);

        Assert.Assert (ctx.RenderedTemplate, "hello.html");
    }

Testing Routes with ManosTestRoute
----------------------------------

To ensure your routes are setup correctly Manos offers some convenience functions for testing routing
outcomes.


    [Test]
    public void Hello_PutHello_CallsHello ()
    {
        Assert.AreEqual (ManosTestRoute.Put ("Hello"), HelloWorldApp.Hello);
    }


Testing Templates with ManosTestContext
----------------------------------------

We can also write tests for our template output using ManosTestContext.

    [Test]
    public void Hello_RenderIndexTemplateWithName_ExactMatchOutput ()
    {
        var ctx = new ManosTestContext ();

        ctx.Render ("index.html", new {
            Name = "Manos"
        });

        Assert.AreEqual (ctx.OutputString, @"<html>
      <head>
        <title>Hello World!</title>
      </head>
      <body>
         Hello World!
      </body>
    </html>");

    }

To simplify testing output strings we can also use the HtmlCompare utility:

    [Test]
    public void Hello_RenderIndexTemplateWithName_HtmlMatchOutput ()
    {
        var ctx = new ManosTestContext ();

        ctx.Render ("index.html", new {
            Name = "Manos"
        });

        Assert.Assert (Utilities.HtmlCompare (ctx.OutputString, @"
           <html>
             <head><title>Hello World!</title></head>
             <body>Hello World!</body>
           </html>");
    }

This will compare HTML output ignoring white space between tags.

---

Continue the tutorial in [part four](./4).