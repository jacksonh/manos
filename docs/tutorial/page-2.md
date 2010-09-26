Building the Shorty application
===============================


Setting up the Routes
---------------------

For our application we are going to want four main routes:

* A homepage where people can submit their links
* A route that handles the submission
* A page that gives users their new link and gives them statistics
* The redirector

So here are what our routes should look like:

    [Route ("/", "/Home", "/Index")]
    public void Index (IManosContext ctx)
    {
    }

    [Post ("/submit-link")]
    public void SubmitLink (IManosContext ctx, string link)
    {
    }

    [Route ("/r/{id}~")]
    public void LinkInfo (IManosContext ctx, string id)
    {
    }

    [Route ("/r/{id}")]
    public void Redirector (IManosContext ctx, Shorty shorty, string id)
    {
    }

Note that we used a couple different ways of setting up routes here. Our Index method doesn't
need any parameters, so it simply implements the ManosAction delegate. It also takes advantage
of Route's params constructor to route a few different strings to itself.

The second method SubmitLink will be called when we submit our form.  We don't really want people
accidently going to thise "page", so it will only accept POST requests. Note that we are recieving
the link as a parameter.  The link will be set in form data.

The last two methods use the simple routing syntax to map pieces of the request url to parameters.

In the final method you'll notice we also recieved an instance of our Shorty app. This is the top
level object that is being hosted by Manos. You can optionally recieve this instance as the
second parameter of your target methods. 


Writing some HTML
-----------------

The first thing we need to do is create our home page. Since Manos's template engine is disabled
in this release, we need to write the HTML ourself (or we could use another template engine).

    [Route ("/", "/Home", "/Index")]
    public void Index (IManosContext ctx)
    {
        ctx.Response.WriteLine (@"<html>
                                   <head><title>Welcome to Shorty</title></head>
                                   <body>
                                    <form method='POST' action='submit-link'>
                                     <input type='text' name='link'>
                                     <input type='submit'>
                                    </form>
                                   </body>
                                  </html>");
    }

It's not beautiful, but that will at least let us get some links into our application.


Storing our links
-----------------

For now, lets just assume our app will never crash and these links can live in memory
forever. That will let us use Manos's object cache for storing our links.

We'll also need to create a simple LinkData class to store our links in the cache, this
can either be a nested class or you can create a LinkData.cs file and stick it in there.

    public class LinkData {

        public string Link;
        public int Clicks;

        public LinkData (string link)
        {
            Link = link;
        }
    }

Finally, we need to create a hashing function for generating unique ids based on our URLs.

    private static string GenerateHash (string str, int length)
    {
        byte [] data = Encoding.Default.GetBytes (str);

        SHA1 sha = new SHA1CryptoServiceProvider (); 
        data = sha.ComputeHash (data);

        string base64 = Convert.ToBase64String (data);

        int i = 0;
        StringBuilder result = new StringBuilder ();
        while (result.Length < length) {
            if (Char.IsLetterOrDigit (base64 [i]))
                result.Append (base64 [i]);
            ++i;
            if (i >= base64.Length)
                return null;
        }
        return result.ToString ();
    }


Now that we have that stuff out of the way, all we need to do is stick our id and LinkData in the
cache and then redirect the user to their LinkInfo page.

    [Post ("/submit-link")]
    public void SubmitLink (Shorty app, IManosContext ctx, string link)
    {
        string id = GenerateHash (link, 5);

        Cache [id] = new LinkData (link);
        ctx.Response.Redirect ("/r/" + id + "~");
    }


Displaying the Link Data
------------------------

Our LinkInfo method is pretty straight forward.  It looks up the suppiled id and displays its
corresponding data.  If no data is found, the user is given a 404 error.

    [Route ("/r/{id}~")]
    public void LinkInfo (Shorty app, IManosContext ctx, string id)
    {
        LinkData info = Cache [id] as LinkData;

        if (info == null) {
	    ctx.Response.StatusCode = 404;
	    return;
	}

	ctx.Response.WriteLine (@"<html>
                                   <head><title>Welcome to Shorty</title></head>
                                   <body>
                                    <a href='{0}'>{0}</a> was clicked {1} times.
                                   </body>
                                  </html>", info.Link, info.Clicks);
    }


Handling the Redirection
------------------------

The only complicated thing in our redirection method is the way that we increment the
Clicks field. Manos runs HTTP transactions in parallel, so there is a chance
that another user is redirecting at the exact same time as us. To make sure our
Clicks field is incremented properly, we can use the
System.Threading.Interlocked.Increment method.

    [Route ("/r/{id}")]
    public void Redirector (Shorty app, IManosContext ctx, string id)
    {
        LinkData info = Cache [id] as LinkData;

        if (info == null) {
            ctx.Response.StatusCode = 404;
            return;
        }

        //
        // Because multiple http transactions could be occuring at the
        // same time, we need to make sure this shared data is incremented
        // properly
        //
        Interlocked.Increment (ref info.Clicks);

        ctx.Response.Redirect (info.Link);
    }

Adding a Redirection Cookie
---------------------------

If we are the creator of a redirection link, there is a good chance we don't
want to be redirected when we click the link, we're more interested in how
many peope have clicked our link. To facilitate this can allow the creator
to set a cookie when they create a new redirection link. Then, in the Redirector
method we can check for the cookie and display the LinkInfo page if it is set.

First off, lets add the option for setting the cookie.  To do this lets just
add a checkbox to the Index page.

    <input type='checkbox' name='show_info' /> Show me the info page, instead of redirecting.<br />

and we'll update our SubmitLink signature to accept the show_info as a
boolean parameter.  Manos's type converters will turn the 'on'/'off' values
that the browser submits into true/false booleans for us.

    public void SubmitLink (IManosContext ctx, Shorty app, string link, bool show_info)

and finally, we'll set the cookie if show_info is true.

    if (show_info)
        ctx.Response.SetCookie ("show_info", "true");

Now that the cookie is set on the browser, we just need to check it and call
LinkInfo at the top of our Redirector method.

    if (ctx.Request.Cookies.Get ("show_info") != null) {
        LinkInfo (ctx, app, id);
        return;
    }


That's it!
----------
Manos still has a long ways to go, but hopefully this tutorial shows off some
of its potential. Future versions of Manos will be shaped by your comments and
suggestions, so please don't be afraid to offer advice.

The complete source code for this tutorial is available in the
examples/Shorty directory.


