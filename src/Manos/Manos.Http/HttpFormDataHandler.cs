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

using Manos.Server;
using Manos.Collections;

namespace Manos.Http {

	public class HttpFormDataHandler : IHttpBodyHandler {

		private enum State {
			InKey,
			InValue,
		}

		private State state;
		private StringBuilder key_buffer = new StringBuilder ();
		private StringBuilder value_buffer = new StringBuilder ();
		
		public void HandleData (HttpTransaction transaction, ByteBuffer data, int pos, int len)
		{
			string str_data = transaction.Request.ContentEncoding.GetString (data.Bytes, pos, len);

			str_data = HttpUtility.HtmlDecode (str_data);

			pos = 0;
			len = str_data.Length;

			while (pos < len) {
				char c = str_data [pos++];

				if (c == '&') {
					if (state == State.InKey)
						throw new InvalidOperationException ("& symbol can not be used in key data.");
					FinishPair (transaction);
					state = State.InKey;
					continue;
				}

				if (c == '=') {
					if (state == State.InValue)
						throw new InvalidOperationException ("= symbol can not be used in value data.");
					state = State.InValue;
					continue;
				}

				switch (state) {
				case State.InKey:
					key_buffer.Append (c);
					break;
				case State.InValue:
					value_buffer.Append (c);
					break;
				}
			}
		}

		public void Finish (HttpTransaction transaction)
		{
			if (state == State.InKey)
				throw new HttpException ("Malformed POST data, key found without value.");

			FinishPair (transaction);
		}

		private void FinishPair (HttpTransaction transaction)
		{
			if (key_buffer.Length == 0 || value_buffer.Length == 0)
				throw new HttpException ("zero length www-form data.");

			Encoding e =  transaction.Request.ContentEncoding;
			transaction.Request.PostData.Set (HttpUtility.UrlDecode (key_buffer.ToString (), e),
					HttpUtility.UrlDecode (value_buffer.ToString (), e));

			key_buffer.Clear ();
			value_buffer.Clear ();
		}
	}
}

