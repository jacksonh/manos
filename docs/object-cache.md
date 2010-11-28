Caching Objects in Manos
========================

Every Manos application can cache objects on the server side using the object cache.  The object cache is a key/value store that runs on the server side. Depending on which backend is in use you can either be sharing your objects with a single Manos server or a cluster of Manos servers.

The default object cache is an in process. A redis-cache is in-development.


The Cache interface
-------------------

There is a single Cache object per a Manos application. The Cache lives in the AppHost object, but can be accessed from any ManosModule or from the ManosApp:

    public MyManosApp ()
    {
        Cache.Set ("Foo", new Bar ());
    }

The following methods are available on the IManosCache interface:


    object Get (string key, CacheItemCallback callback);

Gets a single object from the cache using a key name and invokes your callback.  The callback will receive a null item value if no object with that key name has been registered.

    void Set (string key, object obj);
    void Set (string key, object obj, CacheItemCallback callback);

Set a single object in the cache using a key name.  If an object with the same key already exists, that object will be
replaced in the cache with the new object. If no object with that key exists, the new object will be added to the cache.
In the second method, the callback will be invoked once the set operation has completed.

    void Set (string key, object value, TimeSpan expires);
    void Set (string key, object value, TimeSpan expires, CacheItemCallback callback);

Set a single object in the cache using a key name and expiration time. Follows the same rules as Set (key, value) with regards to replacing/adding objects.  The object will exist in the cache for the duration of the TimeSpan specified and will be removed from the cache once that time expires. In the second method, the callback will be invoked once the set operation has completed.


    void Remove (string key);
    void Remove (string key, CacheItemCallback callback);

Remove the object from the cache with the specified key. If no object exists for that key, nothing will happen. In the second method the callback will be invoked when the remove operation completes. The name of the item will be passed to the callback along with the removed item.


Changing the Cache backend
--------------------------

TODO: This relies on the Environment interface that I still haven't worked out.
TODO: A redis backend is in development.

