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
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Manos.Server;
using Manos.Collections;

namespace Manos.Http {

	public class HttpMultiPartFormDataHandler : IHttpBodyHandler {

		private enum State {

			InHeaderKey,
			InHeaderValue,
			PostHeader1,
			PostHeader2,

			InFormData,
			InFileData,

			InBoundary,
			PostBoundary1,
			PostBoundaryComplete,

			Finished
		}

		private int index;
		private State state;
		private State previous_state;
		private int previous_pos;
		private bool not_boundary;

		private string boundary;
		private Encoding encoding;
		private IUploadedFileCreator file_creator;
		
		private List<byte> header_key = new List<byte> ();
		private List<byte> header_value = new List<byte> ();
		private List<byte> boundary_buffer = new List<byte> ();

		private string current_name;
		private string current_filename;
		private string content_type;

		private int upload_count;
		private UploadedFile uploaded_file;
		private List<byte> form_data = new List<byte> ();

		public HttpMultiPartFormDataHandler (string boundary, Encoding encoding, IUploadedFileCreator file_creator)
		{
			this.boundary = "--" + boundary;
			this.encoding = encoding;
			this.file_creator = file_creator;

			state = State.InBoundary;
		}
		
		public void HandleData (HttpTransaction transaction, ByteBuffer data, int pos, int len)
		{
			// string str_data = encoding.GetString (data.Bytes, pos, len);
			byte [] str_data = data.Bytes;

			pos = -1;
			len = str_data.Length;

			Console.WriteLine ("STR DATA for state '{1}' {0}", data.Length, state);

			while (pos < len - 1 && state != State.Finished) {

				byte c = str_data [++pos];

				switch (state) {
				case State.InBoundary:
					if (index == boundary.Length - 1) {

						boundary_buffer.Clear ();

						// Flush any data
						FinishFormData (transaction);
						FinishFileData (transaction);

						state = State.PostBoundary1;
						index = 0;
						break;
					}

					boundary_buffer.Add (c);

					if (c != boundary [index]) {
						// Copy the boundary buffer to the beginning and restart parsing there
						MemoryStream stream = new MemoryStream ();
						stream.Write (boundary_buffer.ToArray (), 0, boundary_buffer.Count);
						Console.WriteLine ("boundary buffer:  ***\n{0}\n****", encoding.GetString (stream.ToArray ()));
						stream.Write (str_data, pos + 1, str_data.Length - pos - 1);
						Console.WriteLine ("remaining: ===\n{0}\n====", encoding.GetString (str_data, pos + 1, str_data.Length - pos - 1));
						str_data = stream.ToArray ();

						Console.WriteLine ("DATA:'{0}'", encoding.GetString (str_data));
							
						/*
						byte [] new_data = new byte [boundary_buffer.Count + str_data.Length - pos - 1];
						Array.Copy (boundary_buffer.ToArray (), new_data, boundary_buffer.Count);
						Array.Copy (str_data, pos, new_data, boundary_buffer.Count, str_data.Length - pos);
						str_data = new_data;
						*/

						pos = -1;
						len = str_data.Length;

						not_boundary = true;
						boundary_buffer.Clear ();
						state = previous_state;
						index = 0;

						// continue instead of break so not_boundary is not reset
						continue;
					}

					++index;
					break;

				case State.PostBoundary1:
					if (c == '-') {
						state = State.PostBoundaryComplete;
						break;
					}

					if (c == '\r')
						break;

					if (c == '\n') {
						state = State.InHeaderKey;
						break;
					}

					throw new Exception (String.Format ("Invalid post boundary char '{0}'", c));

				case State.PostBoundaryComplete:
					if (c != '-')
						throw new Exception (String.Format ("Invalid char '{0}' in boundary complete.", c));

					state = State.Finished;
					break;

				case State.InHeaderKey:
					if (c == '\n') {
						state = current_filename == null ? State.InFormData : State.InFileData;
						break;
					}

					if (c == ':') {
						state = State.InHeaderValue;
						break;
					}

					header_key.Add (c);
					break;

				case State.InHeaderValue:
					if (c == '\r') {
						state = State.PostHeader1;
						break;
					}

					header_value.Add (c);
					break;

				case State.PostHeader1:
					if (c != '\n')
						throw new Exception (String.Format ("Invalid char '{0}' in post header 1.", c));
					state = State.PostHeader2;
					break;

				case State.PostHeader2:
					HandleHeader (transaction);
					header_key.Clear ();
					header_value.Clear ();
					state = State.InHeaderKey;
					break;


				case State.InFormData:
					if (CheckStartingBoundary (str_data, pos))
						break;

					form_data.Add (c);
					break;

				case State.InFileData:
					if (CheckStartingBoundary (str_data, pos))
						break;;

					uploaded_file.Contents.WriteByte (c);
					break;
				default:
					throw new Exception (String.Format ("Unhandled state: {0}", state));
				}

				not_boundary = false;
			}
		}

		private bool CheckStartingBoundary (byte [] str_data, int pos)
		{
			if (not_boundary)
				return false;
			if (pos >= str_data.Length)
				return false;
			bool res = str_data [pos] == boundary [0];
			if (res) {
				boundary_buffer.Clear ();
				boundary_buffer.Add (str_data [pos]);

				index = 1;
				previous_state = state;
				state = State.InBoundary;
				previous_pos = pos;
			}

			return res;
		}

		private void HandleHeader (HttpTransaction transaction)
		{
			string key = encoding.GetString (header_key.ToArray ());
			string value = encoding.GetString (header_value.ToArray ());

			if (key == "Content-Disposition")
				ParseContentDisposition (value);
			else if (key == "Content-Type")
				ParseContentType (value);

		}

		public void Finish (HttpTransaction transaction)
		{
			FinishFormData (transaction);
			FinishFileData (transaction);
		}

		private void FinishFormData (HttpTransaction transaction)
		{
			if (form_data.Count == 0)
				return;

			transaction.Request.PostData.Set (current_name, HttpUtility.UrlDecode (form_data.ToString (), encoding));
			form_data.Clear ();
		}

		private void FinishFileData (HttpTransaction transaction)
		{
			if (uploaded_file == null)
				return;

			// Chop off the \r\n that gets appended before the boundary marker
			uploaded_file.Contents.SetLength (uploaded_file.Contents.Position - 2);

			uploaded_file.Finish ();
			transaction.Request.Files.Add (current_filename, uploaded_file);
			uploaded_file = null;
		}

		public void ParseContentDisposition (string str)
		{
			current_name = GetContentDispositionAttribute (str, "name");
			current_filename = GetContentDispositionAttributeWithEncoding (str, "filename");

			if (!String.IsNullOrEmpty (current_filename)) 
				uploaded_file = file_creator.Create (current_filename);
		}

		public void ParseContentType (string str)
		{
			content_type = str.Substring ("Content-Type:".Length).Trim ();
		}

		public static string GetContentDispositionAttribute (string l, string name)
		{
			int idx = l.IndexOf (name + "=\"", StringComparison.InvariantCulture);
			if (idx < 0)
				return null;
			int begin = idx + name.Length + "=\"".Length;
			int end = l.IndexOf ('"', begin);
			if (end < 0)
				return null;
			if (begin == end)
				return String.Empty;
			return l.Substring (begin, end - begin);
		}

		public string GetContentDispositionAttributeWithEncoding (string l, string name)
		{
			int idx = l.IndexOf (name + "=\"", StringComparison.InvariantCulture);
			if (idx < 0)
				return null;
			int begin = idx + name.Length + "=\"".Length;
			int end = l.IndexOf ('"', begin);
			if (end < 0)
				return null;
			if (begin == end)
				return String.Empty;

			string temp = l.Substring (begin, end - begin);
			byte [] source = new byte [temp.Length];
			for (int i = temp.Length - 1; i >= 0; i--)
				source [i] = (byte) temp [i];

			return encoding.GetString (source);
		}
	}
}


