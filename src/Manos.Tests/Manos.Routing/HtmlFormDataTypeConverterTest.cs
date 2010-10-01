using System;
using NUnit.Framework;

using Manos.ShouldExt;


namespace Manos.Routing.Tests
{
	[TestFixture()]
	public class HtmlFormDataTypeConverterTest
	{
		[Test]
		public void Ctor_NullDestType_Throws ()
		{
			Should.Throw<ArgumentNullException> (() => new HtmlFormDataTypeConverter (null));
		}
		
		[Test]
		public void Ctor_ValidDestType_SetsDestTypeProp ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (string));
			
			var dest = converter.DestinationType;
			Assert.AreEqual (typeof (string), dest);
		}
		
		[Test]
		public void CanConvertFrom_NullContextAndSource_ReturnsFalse ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var can = converter.CanConvertFrom (null);
			Assert.IsFalse (can);
		}
		
		[Test]
		public void CanConvertFrom_StringType_ReturnsTrue ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var can = converter.CanConvertFrom (typeof (string));
			Assert.IsTrue (can);
		}
		
		[Test]
		public void CanConvertFrom_NonStringType_ReturnsFalse ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var can = converter.CanConvertFrom (typeof (int));
			Assert.IsFalse (can);
		}
		
		[Test]
		public void ConvertFrom_on_StringToBool_ReturnsTrue ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom ("on");
			Assert.IsTrue (value);
		}
		
		[Test]
		public void ConvertFrom_off_StringToBool_ReturnsFalse ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom ("off");
			Assert.IsFalse (value);
		}
		
		[Test]
		public void ConvertFrom_ON_StringToBool_ReturnsTrue ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom ("on");
			Assert.IsTrue (value);
		}
		
		[Test]
		public void ConvertFrom_OFF_StringToBool_ReturnsFalse ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom ("off");
			Assert.IsFalse (value);
		}
		
		[Test]
		public void ConvertFrom_On_StringToBool_ReturnsTrue ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom ("on");
			Assert.IsTrue (value);
		}
		
		[Test]
		public void ConvertFrom_OfF_StringToBool_ReturnsFalse ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom ("off");
			Assert.IsFalse (value);
		}
		
		[Test]
		public void ConvertFrom_NullToBool_ReturnsFalse ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = (bool) converter.ConvertFrom (null);
			Assert.IsFalse (value);
		}
		
		[Test]
		public void ConvertFrom_UnsupportedStringToBool_ReturnsNull ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (bool));
			
			var value = converter.ConvertFrom ("woah");
			Assert.IsNull (value);
		}
		
		[Test]
		public void ConvertFrom_StringToInt_ReturnsNull ()
		{
			var converter = new HtmlFormDataTypeConverter (typeof (int));
			
			var value = converter.ConvertFrom ("42");
			Assert.IsNull (value);
		}
	}
}

