using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using NUnit.Framework;
using Manos.Collections;

namespace Manos.Server.Tests
{
	[TestFixture()]
	public class RequestDataDictionaryTest
	{
		[Test()]
		public void Count_NoItems_ReturnsZero ()
		{
			var rdd = new RequestDataDictionary ();
			
			Assert.AreEqual (0, rdd.Count);
		}
		
		[Test]
		public void Count_SingleItem_ReturnsOne ()
		{
			var rdd = new RequestDataDictionary ();
			
			rdd.Add ("foobar", "baz");
			
			Assert.AreEqual (1, rdd.Count);
		}
		
		[Test]
		public void Count_SingleItemInCollection_ReturnsOne ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			
			Assert.AreEqual (1, rdd.Count);
		}
		
		[Test]
		public void Count_ItemInCollectionAndItemAdded_ReturnsTwo ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			rdd.Add ("foobar", "baz");
			
			Assert.AreEqual (2, rdd.Count);
		}
		
		[Test]
		public void Clear_SingleItemAdded_SetsCountZero ()
		{
			var rdd = new RequestDataDictionary ();

			rdd.Add ("foobar", "baz");
			rdd.Clear ();
			
			Assert.AreEqual (0, rdd.Count);
		}
		
		[Test]
		public void Clear_ItemInCollection_SetsCountZero ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			
			rdd.Clear ();
			Assert.AreEqual (0, rdd.Count);
		}
		
		[Test]
		public void Clear_ItemInCollectionAndItemAdded_SetsCountZero ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			rdd.Add ("foobar", "baz");
			
			rdd.Clear ();
			
			Assert.AreEqual (0, rdd.Count);
		}
		
		[Test]
		public void Clear_SingleItemAdded_RemovesItem ()
		{
			var rdd = new RequestDataDictionary ();

			rdd.Add ("foobar", "baz");
			rdd.Clear ();
			
			string dummy;
			bool has_item = rdd.ContainsKey ("foobar");
			
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void Clear_ItemInCollection_RemovesItem ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			
			rdd.Clear ();
			
			bool has_item = rdd.ContainsKey ("foobar");
			
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void Clear_ItemInCollectionAndItemAdded_RemovesItem ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			rdd.Add ("foobar", "baz");
			
			rdd.Clear ();
			
			bool has_item = rdd.ContainsKey ("foobar");
			
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void ContainsKey_NoItems_ReturnsFalse ()
		{
			var rdd = new RequestDataDictionary ();

			bool has_item = rdd.ContainsKey ("foobar");
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void ContainsKey_ItemAdded_ReturnsTrue ()
		{
			var rdd = new RequestDataDictionary ();
			
			rdd.Add ("foobar", "baz");
			
			bool has_item = rdd.ContainsKey ("foobar");
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void ContainsKey_ItemInCollection_ReturnsTrue ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.ContainsKey ("foobar");
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void Contains_NoItemsAdded_ReturnsFalse ()
		{
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string, string> ("foobar", "baz");
			
			bool has_item = rdd.Contains (pair);
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void Contains_ItemAddedKeyMatchesValueDoesnt_ReturnsFalse ()
		{
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string, string> ("foobar", "baz");
			
			rdd.Add ("foobar", "nope");
			
			bool has_item = rdd.Contains (pair);
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void Contains_ItemAddedKeyDoesNotMatchValueDoes_ReturnsFalse ()
		{
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string, string> ("foobar", "baz");
			
			rdd.Add ("nope", "baz");
			
			bool has_item = rdd.Contains (pair);
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void Contains_ItemAddedKeyAndValueMatch_ReturnsTrue ()
		{
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string, string> ("foobar", "baz");
			
			rdd.Add ("foobar", "baz");
			
			bool has_item = rdd.Contains (pair);
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void Contains_ItemInCollectionKeyAndValuesMatch_ReturnsTrue ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string,string> ("foobar", "baz");
			
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.Contains (pair);
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void Contains_MultipleItemsInCollectionFirstKeyAndValuesMatch_ReturnsTrue ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string,string> ("foobar", "baz");
			
			nvc.Add ("foobar", "baz");
			nvc.Add ("foobar", "blah");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.Contains (pair);
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void Contains_MultipleItemsInCollectionLastKeyAndValuesMatch_ReturnsTrue ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string,string> ("foobar", "baz");
			
			nvc.Add ("foobar", "blah");
			nvc.Add ("foobar", "baz");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.Contains (pair);
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void Contains_MultipleItemsInCollectionMiddleKeyAndValuesMatch_ReturnsTrue ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string,string> ("foobar", "baz");
			
			nvc.Add ("foobar", "blah");
			nvc.Add ("foobar", "baz");
			nvc.Add ("foobar", "burrah");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.Contains (pair);
			Assert.IsTrue (has_item);
		}
		
		[Test]
		public void Contains_MultipleItemsWithPartialStartMatchesInCollection_ReturnsFalse ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string,string> ("foobar", "baz");
			
			nvc.Add ("foobar", "baza");
			nvc.Add ("foobar", "baza");
			nvc.Add ("foobar", "baza");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.Contains (pair);
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void Contains_MultipleItemsWithPartialEndMatchesInCollection_ReturnsFalse ()
		{
			var nvc = new DataDictionary ();
			var rdd = new RequestDataDictionary ();
			var pair = new KeyValuePair<string,string> ("foobar", "baz");
			
			nvc.Add ("foobar", "abaz");
			nvc.Add ("foobar", "abaz");
			nvc.Add ("foobar", "abaz");
			rdd.AddCollection (nvc);
			
			bool has_item = rdd.Contains (pair);
			Assert.IsFalse (has_item);
		}
		
		[Test]
		public void TryGetValue_NoValuesAdded_ReturnsFalse ()
		{
			var rdd = new RequestDataDictionary ();
			
			string dummy;
			bool has_value = rdd.TryGetValue ("foobar", out dummy);
			
			Assert.IsFalse (has_value);
		}
		
		[Test]
		public void TryGetValue_NoValuesAdded_SetsValueNull ()
		{
			var rdd = new RequestDataDictionary ();
			
			string the_data;
			rdd.TryGetValue ("foobar", out the_data);
			
			Assert.IsNull (the_data);
		}
		
		[Test]
		public void TryGetValue_NonMatchingValueAdded_ReturnsFalse ()
		{
			var rdd = new RequestDataDictionary ();
			
			rdd.Add ("blizbar", "baz");
			
			string dummy;
			bool has_value = rdd.TryGetValue ("foobar", out dummy);
			
			Assert.IsFalse (has_value);
		}
		
		[Test]
		public void TryGetValue_SingleAddedValueMatches_ReturnsTrue ()
		{
			var rdd = new RequestDataDictionary ();
			
			rdd.Add ("foobar", "baz");
			
			string dummy;
			bool has_value = rdd.TryGetValue ("foobar", out dummy);
			
			Assert.IsTrue (has_value);
		}
		
		[Test]
		public void TryGetValue_SingleAddedValueMatches_SetsData ()
		{
			var rdd = new RequestDataDictionary ();
			
			rdd.Add ("foobar", "baz");
			
			string the_data;
			rdd.TryGetValue ("foobar", out the_data);
			
			Assert.AreEqual ("baz", the_data);
		}
	}
}

