

using System;


namespace Mango.Server {

	public static class UriParser {

		public static bool TryParse (string uri, out string scheme, out string path, out string query)
		{
			int end;

			if (String.IsNullOrEmpty (uri)) {
				scheme = null;
				path = null;
				query = null;
				return false;
			}

			
			return TryParseScheme (uri, out scheme, out end) &&
			       TryParsePath (uri, end, out path, out end) &&
			       TryParseQuery (uri, end, out query);
		}

		public static bool TryParseScheme (string uri, out string scheme, out int end)
		{
			end = uri.IndexOf ("://");
			if (end == -1) {
				scheme = "http";
				end = 0;
			} else {
				scheme = uri.Substring (0, end);
				end += 3; // move past the ://
			}

			return true;
		}

		public static bool TryParsePath (string uri, int start, out string path, out int end)
		{
			if (start >= uri.Length) {
				path = null;
				end = -1;
				return false;
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

