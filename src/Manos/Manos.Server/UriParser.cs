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

using System.Text;

namespace Manos.Server {

	public static class UriParser {

		public static bool TryParse (string uri, out string scheme, out string host, out string path, out string query)
		{
			int end;

			uri = HttpUtility.UrlDecode (uri, Encoding.Default);
			
			if (String.IsNullOrEmpty (uri)) {
				scheme = null;
				path = null;
				query = null;
				host = null;
				return false;
			}

			path = null;
			query = null;
			host = null;

			return TryParseScheme (uri, out scheme, out end) &&
			       TryParseHost (uri, end, out host, out end) &&
			       TryParsePath (uri, end, out path, out end) &&
			       TryParseQuery (uri, end, out query);
		}

		public static bool TryParseScheme (string uri, out string scheme, out int end)
		{
			end = uri.IndexOf ("://", StringComparison.InvariantCulture);
			if (end == -1) {
				scheme = "http";
				end = 0;
			} else {
				scheme = uri.Substring (0, end);
				end += 3; // move past the ://
			}

			return true;
		}

		public static bool TryParseHost (string uri, int start, out string host, out int end)
		{
			if (start >= uri.Length) {
				host = null;
				end = -1;
				return false;
			}

			end = uri.IndexOf ('/', start);
			if (end == -1) {
				host = uri.Substring (start);
				end = uri.Length;
			} else if (end <= start) {
				host = null;
				end = start;
			} else {
				host = uri.Substring (start, end - start);
			}

			return true;
		}
		
		public static bool TryParsePath (string uri, int start, out string path, out int end)
		{
			if (start >= uri.Length) {
				path = "/";
				end = start;
				return true;
			}

			end = uri.IndexOf ('?', start);
			if (end == -1) {
				path = uri.Substring (start, uri.Length - start);
				end = uri.Length;
			} else {
				path = uri.Substring (start, end - start);
				++end; // advance past the ?
			}
			
			return true;
		}

		public static bool TryParseQuery (string uri, int start, out string query)
		{
			if (start >= uri.Length) {
				// Perfectly legal to not have a query
				query = String.Empty;
				return true;
			}

			int end = uri.IndexOf ('#', start);
			if (end == -1)
				end = uri.Length;

			query = uri.Substring (start, end - start);
			return true;
		}
	}
}

