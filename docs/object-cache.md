Caching Objects in Manos
========================

Every Manos application can cache objects on the server side using the object cache.  The object cache is a key/value store that runs on the server side.

The default object cache is an in process, if you would like to cache objects across multiple servers or would like a more robust cache you can use the memcached backend. See below for more information.


The Cache interface
-------------------

There is a single Cache object per a Manos application. The Cache lives in the AppHost object, but can be accessed from any ManosModule or from the ManosApp:

    public MyManosApp ()
    {
        Cache ["Foo"] = new Bar ();
    }

this is equivalent to:

    public MyManosApp ()
    {
        AppHost.Cache ["Foo"] = new Bar ();
    }


The following methods are available on the IManosCache interface:


    object Get (string key);

Gets a single object from the cache using a key name, returns null if no object with that key name has been registered.


    void Set (string key, object obj);

Set a single object in the cache using a key name.  If an object with the same key already exists, that object will be
replaced in the cache with the new object. If no object with that key exists, the new object will be added to the cache.


    void Set (string key, object value, TimeSpan expires);

Set a single object in the cache using a key name and expiration time. Follows the same rules as Set (key, value) with regards
to replacing/adding objects.  The object will exist in the cache for the duration of the TimeSpan specified and will be
removed from the cache once that time expires.


    void Remove (string key);

Remove the object from the cache with the specified key. If no object exists for that key, nothing will happen.


Changing the Cache backend
--------------------------

TODO: This relies on the Environment interface that I still haven't worked out.

