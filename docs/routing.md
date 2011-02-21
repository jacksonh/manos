Routing in Manos
================

Manos exposes a number of different methods of mapping URLs to methods and anonymous methods.

Explicitly calling the Routing methods
--------------------------------------

The ManosModule type has a number of methods for adding routing handlers. These methods are usually used to route to another ManosModule
but can also be used to map to a ManosAction.

The routing method names correspond to the HTTP methods that they add a route handler for:

    Get
    Put
    Post
    Head
    Delete
    Trace
    Options
    Route

The final method (Route) will route all HTTP methods to the supplied action.

There are a number of different overloads available for each method:

    Route (string pattern, ManosModule module)
    Route (string pattern, ManosAction action)
    Route (ManosAction action, params string [] patterns)
    Route (ManosModule module, params string [] patterns)

The last two overloads let you easily map multiple routes to a single ManosAction or ManosModule:

    Route (IndexHandler, "/", "Index", "Home");



Implicit method Routing
-----------------------

Any method in a ManosModule with the correct signature will be mapped to a URL based on its name. There are two types of signatures that will be implicitly mapped:

A simple ManosAction signature:

    public void MyMethod (IManosContext ctx)
    {
    }

These are methods that match the ManosAction delegate. They will be added to the current ManosModule's RouteTable as "/" + <Method Name>. In the above example
the string "/MyMethod" would be added to the RouteTable.

If you would like to receive an instance of your top level application or your request data as method parameters you can also use ParameterizedActions. These actions take
the form:

    public void MyMethod (MyManosApp app, IManosContext ctx, string myparam1, int myparam2, bool myparam3)
    {
    }

A ParameterizedAction must be a public method that returns void. The type of the first parameter must be a subclass of ManosApp,
it should either be a generic ManosApp object or your applications top level ManosApp type. The second parameter must be an IManosContext. After
the first two parameters, the rest of the parameters can be of any type as long as they are not passed by ref (ie not a ref or out param). The
parameters will be converted from the strings passed to Manos from the browser in url and form data. The method may be an instance method or a static method.

Default values can also be used for Actions to make parameters optional.  Standard C# default param syntax is used:

    public void MyMethod (IManosContext ctx, string user_name = "ANONYMOUS")
    {
    }

Default values will be used if the user_name is not found in the supplied data.

If you have a method that matches this signature and don't want it to be added to the routing table simply add an IgnoreAttribute

    [Ignore]
    public void MyMethod (MyManosApp app, IManosContext ctx)
    {
    }


Arrays and Dictionaries in parameters
--------------------------------------

Manos supports creating arrays or dictionaries of parameters.  For example the following html form data would be mapped to an array param:

    &lt;input type="text" name="foobar[]"&gt;
    &lt;input type="text" name="foobar[]"&gt;
    &lt;input type="text" name="foobar[]"&gt;

with those three elements in a form you can now have a method signature like this:

    public void MyMethod (IManosContext ctx, string [] foobar)
    {
    }

The element values of the array follow the exact same rules as normal parameter values.  So in the case of a string [] array type, the elements are escaped UnsafeStrings.

Dictionaries can also be created by using a dictionary syntax:

    &lt;input type="text" name="foobar[blah]"&gt;
    &lt;input type="text" name="foobar[blarg]"&gt;
    &lt;input type="text" name="foobar[blaz]"&gt;

the elements would map to a Dictionary<string,...> in a method like this:

    public void MyMethod (IManosContext ctx, Dictionary<string,string> foobar)
    { 
        Console.WriteLine (foobar ["blah"]);
    }

Dictionaries can be created from any type that implements IDictionary however the keys will always be strings and if you create a generic type that implements IDictionary the second generic parameter will be used to map the value.  So in the standard Dictionary<TKey,TValue> TKey must be a string and TValue will be mapped using the same conversion methods as any other parameter.


Explicitly routing methods using attributes
-------------------------------------------

Manos includes attributes that allow you to map routes to methods or ManosModule properties. The routing attributes correspond to the HTTP methods and also
include a RouteAttribute similar to the Route method. To use these attributes simply apply them to a method that matches either the ManosAction signature
or the ParameterizedAction signature:

    [Get ("/Index", "/Home")]
    public static void Home (IManosContext ctx)
    {
    }

    [Post ("/FormPosted")]
    public static void Main (MyManosApp app, IManosContext ctx, string foo)
    {
    }

    [Route ("/Admin")]
    public AdminModule Admin {
        get {
            if (admin == null)
                admin = new AdminModule ();
            return admin;
        }
    }

Properties that map ManosModules must not return null.

If necessary you can add more than one routing attribute to a method:

    [Get ("/Foo")]
    [Post ("/FooBar")]
    public static void Foo (IManosContext ctx)
    {
    }


More on ParameterizedActions
----------------------------

ParameterizedActions will match data from the request to the names of the parameters. If no data is found the routing will fail and return an HTTP 402 status code. Therefor, parameters should only be used for mandatory data. Optional parameters should be retrieved from the Request object.

Data is looked up in the following order:

1. ***UrlData dictionary***: This is a dictionary of key/values containing data passed in the query string of the url and from the named regular expressions in the route.
2. ***FormData dictionary***: This is a dictionary of key/values containing data passed from the browser as form data.


Regular Expressions as Routes
-----------------------------

Manos routes can also be matched to regular expressions. In this case your regex variables will be added to your request data and made available as parameters to your method.

    [Get ("^articles/(?<slug>.*?)/$")]
    public static void Articles (ManosApp app, IManosContext ctx, string slug)
    {
    }
 

Simple patterns are Routes
--------------------------

Manos routes also support a simple string matching system. Strings are searched for {variable} patterns to match routes. These variables will be stored in the Request
data as strings, but the parameter type conversion can convert these strings for you:

    [Get ("/articles/{slug}/")]
    public static void Articles (ManosApp app, IManosContext ctx, string slug)
    {
    }



