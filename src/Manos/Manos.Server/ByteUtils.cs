

using System;

namespace Manos.Server {

	  public static class ByteUtils {

	  	 public static int FindDelimiter (byte [] delimiter, byte [] data, int start)
		 {
			start = Array.IndexOf (data, delimiter [0], start);

			while (start >= 0) {
				bool match = true;
				for (int i = 1; i < delimiter.Length; i++) {
					if (data [start + i] == delimiter [i])
						continue;
					match = false;
					break;
				}
				if (match)
					return start + delimiter.Length;
				start = Array.IndexOf (data, delimiter [0], start + 1);
			}

			return -1;
		 }
	  }
}

