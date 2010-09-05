using System;


namespace Manos.Caching
{
	public interface IManosCache
	{
		object Get (string key);
		
		void Set (string key, object obj);
		void Set (string key, object value, TimeSpan expires);
		
		void Remove (string key);
		
		object this [string key] {
			get;
			set;
		}
		
		void Clear ();
	}
}

