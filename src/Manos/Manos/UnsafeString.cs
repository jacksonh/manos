

using System;
using System.Text;

namespace Manos
{
	public class UnsafeString
	{
		private string unsafe_value;
		private string safe_value;
		private bool has_unsafe_data;
		
		public UnsafeString (string str)
		{
			this.unsafe_value = str;
		}
		
		public string UnsafeValue {
			get { return unsafe_value; }	
		}
		
		public string SafeValue {
			get {
				if (safe_value == null)
					safe_value = Escape (unsafe_value, out has_unsafe_data);
				return safe_value;
			}
		}
		
		public bool HasUnsafefData {
			get {
				if (safe_value == null)
					safe_value = Escape (unsafe_value, out has_unsafe_data);
				return has_unsafe_data;
			}
		}
		
		public override string ToString ()
		{
			return SafeValue;
		}
		
		public static implicit operator string (UnsafeString input)
		{
			if (input == null)
				return null;
			return input.ToString ();	
		}
		
		/*
		public static explicit operator string (UnsafeString input)
		{
			return input.ToString ();	
		}
		*/
		
		public static string Escape (string input)
		{
			bool dummy;
			
			return Escape (input, out dummy);
		}
		
		public static string Escape (string input, out bool has_unsafe_data)
		{
			StringBuilder builder = new StringBuilder ();

			has_unsafe_data = false;
			
			for (int i = 0; i < input.Length; i++) {
				char c = input [i];
				
				switch (c) {
				case '&':
					builder.Append ("&amp;");
					has_unsafe_data  = true;
					break;
				case '<':
					builder.Append ("&lt;");
					has_unsafe_data = true;
					break;
				case '>':
					builder.Append ("&gt;");
					has_unsafe_data = true;
					break;
				case '"':
					builder.Append ("&quot;");
					has_unsafe_data = true;
					break;
				case '\'':
					builder.Append ("&#39;");
					has_unsafe_data = true;
					break;
				default:
					builder.Append (c);
					break;
				}
			}
			
			if (has_unsafe_data)
				return builder.ToString ();
			return input;	
		}
	}
}

