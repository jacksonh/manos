Building the Shorty application
===============================

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
    public void SubmitLink (Shorty app, IManosContext ctx, string link)
    {
    }

    [Route ("/r/{id}~")]
    public void LinkInfo (Shorty app, IManosContext ctx, string old_url, string new_url)
    {
    }

    [Route ("/r/{id}")]
    public void Redirector (Shorty app, IManosContext ctx, string id)
    {
    }

