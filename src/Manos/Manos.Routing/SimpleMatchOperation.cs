//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Manos.Collections;

namespace Manos.Routing
{
	public class SimpleMatchOperation : IMatchOperation
	{
		private string pattern;
		private Group [] groups;
		
		private class Group {
			public int Start;
			public int End;
			public string Name;
			
			public Group (int start, int end, string name)
			{
				Start = start;
				End = end;
				Name = name;
			}
		}
		
		public SimpleMatchOperation (string pattern)
		{
			Pattern = pattern;
		}
	
		public string Pattern {
			get { return pattern; }
			set {
				if (value == null)
					throw new ArgumentNullException ("pattern");
				pattern = value;
				
				UpdateGroups ();
			}
		}
		
		public bool IsMatch (string input, int start, out DataDictionary data, out int end)
		{
			end = start;
			data = null;

			if (groups == null) {
				return false;
			}
			
			int input_pos = start;
			int pattern_pos = 0;
			
			string data_str;
			foreach (Group g in groups) {
				// scan until start
				int g_start = start + g.Start;

				if (g_start > pattern.Length)
					return false;

				int len = g_start - pattern_pos;
				for (int i = 0; i < len; i++) {
					if (input [input_pos] != pattern [pattern_pos])
						return false;
					
					input_pos++;
					pattern_pos++;
					
					if (input_pos > input.Length - 1)
						return false;
				}

				if (g.End == pattern.Length - 1) {
					// slurp until end
					data_str = input.Substring (input_pos);
					if (data == null)
						data = new DataDictionary ();
					data.Set (g.Name, data_str);

					end = input.Length;
					return true;
				}
				
				int input_start = input_pos;
				char end_marker = pattern [g.End + 1];
				while (input [input_pos] != end_marker) {
					input_pos++;
					if (input_pos >= input.Length)
						return false;
				}
				
				data_str = input.Substring (input_start, input_pos - input_start);
				if (data == null)
					data = new DataDictionary ();
				data.Set (g.Name, data_str);

				input_pos++;
				pattern_pos = g.End + 2;
			}

			while (pattern_pos < pattern.Length) {
				if (pattern [pattern_pos] != input [input_pos]) {
					return false;
				}
				pattern_pos++;
				input_pos++;
				
				if (input_pos > input.Length) {
					return false;
				}
			}

			end = input_pos;
			return true;
		}
		
		private void UpdateGroups ()
		{
			List<Group> groups = new List<Group> ();
			
			int start = pattern.IndexOf ('{');
			while (start >= 0) {
				if (start < pattern.Length - 1 && pattern [start + 1] == '{') {
					start += 2;
					continue;
				}
				
				bool not_closed = false;
				int end = pattern.IndexOf ('}', start + 1);
				while (end < pattern.Length - 1 && pattern [end + 1] == '}') {
					end = pattern.IndexOf ('}', start + 1);
					if (end < 0)
						not_closed = true;
				}
				
				if (not_closed) {
					Console.Error.WriteLine ("Unclosed matching group in '{0}'. All matches will be disabled.", pattern);
					groups = null;
					return;
				}
					
				Group group = new Group (start, end, pattern.Substring (start + 1, end - start - 1));
				groups.Add (group);
				start = pattern.IndexOf ('{', end);
			}
			
			this.groups = groups.ToArray ();
		}
	}
}

