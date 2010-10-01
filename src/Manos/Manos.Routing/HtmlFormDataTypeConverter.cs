using System;
using System.ComponentModel;
using System.Globalization;


namespace Manos.Routing
{
	public class HtmlFormDataTypeConverter : TypeConverter
	{
		public HtmlFormDataTypeConverter (Type dest_type)
		{
			if (dest_type == null)
				throw new ArgumentNullException ("dest_type");
			
			DestinationType = dest_type;
		}
		
		public Type DestinationType {
			get;
			private set;
		}
		
		public override bool CanConvertFrom (ITypeDescriptorContext context, Type source_type)	
		{
			return source_type == typeof (string);
		}
		
		public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (DestinationType == typeof (bool)) {
				
				if (value == null)
					return false;
				
				string str = (string) value;

				if (String.Compare ("on", str, StringComparison.InvariantCultureIgnoreCase) == 0)
					return true;
				
				if (String.Compare ("off", str, StringComparison.InvariantCultureIgnoreCase) == 0)
					return false;
			}
			
			// Technically I am supposed to throw NotSupportedException here, but lets
			// at least pretend to be effecient.
			return null;
		}
	}
}

