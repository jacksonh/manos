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


using Manos.Collections;

namespace Manos.Server {

	public delegate void IMFDReadCallback (byte [] data, int offset, int len);

	public interface IMFDStream {

		void ReadUntil (string delimiter, IMFDReadCallback callback);
		void ReadBytes (int count, IMFDReadCallback callback);

		void BeginFile (string name);
		UploadedFile EndFile ();
	}

	public class InMemoryMFDStream : IMFDStream {

		private int pos;
		private byte [] buffer;
		private UploadedFile current_file;

		public InMemoryMFDStream (byte [] buffer)
		{
			this.buffer = buffer;
		}

		public void ReadUntil (string delimiter, IMFDReadCallback callback)
		{
			byte [] read_delimiter = Encoding.ASCII.GetBytes (delimiter);
			
			int count = ByteUtils.FindDelimiter (read_delimiter, buffer, pos, buffer.Length);
			int start = pos;

			pos = count;

			callback (buffer, start, count - start);
		}

		public void ReadBytes (int count, IMFDReadCallback callback)
		{
			int start = pos;
			pos += count;

			callback (buffer, start, count);
		}

		public void BeginFile (string name)
		{
			current_file = new InMemoryUploadedFile (name);
		}

		public UploadedFile EndFile ()
		{
			return current_file;
		}
	}

	public class TempFileMFDStream : IMFDStream {

		private IOStream iostream;
		private UploadedFile current_file;

		public TempFileMFDStream (IOStream iostream)
		{
			this.iostream = iostream;
		}

		public void ReadUntil (string delimiter, IMFDReadCallback callback)
		{
			iostream.ReadUntil (delimiter, (s, data, offset, count) => {
				callback (data, offset, count);
			});
		}

		public void ReadBytes (int count, IMFDReadCallback callback)
		{
			iostream.ReadBytes (count, (s, data, offset, _count) => {
				callback (data, offset, _count);
			});
		}

		public void BeginFile (string name)
		{
			string temp = Path.GetTempFileName ();
			current_file = new TempFileUploadedFile (name, temp);
		}

		public UploadedFile EndFile ()
		{
			var file = current_file;
			current_file = null;

			return file;
		}
	}

	public class MultipartFormDataParser {

		private IHttpRequest request;
		private string boundary;
		private IMFDStream stream;
		private Action complete;

		private DataDictionary form_data;

		private Encoding encoding;

		//
		// State machine data
		//
		private string name;
		private string filename;
		private string content_type;

		public MultipartFormDataParser (IHttpRequest request, string boundary, IMFDStream stream, Action complete)
		{
			this.request = request;
			this.boundary = boundary;
			this.stream = stream;
			this.complete = complete;

			encoding = request.ContentEncoding;
			form_data = new DataDictionary ();
		}

		public void ParseParts ()
		{
			ClearState ();

			stream.ReadUntil (boundary + "\r\n", OnPart);
		}

		public void ClearState ()
		{
			name = null;
			filename = null;
			content_type = null;
		}

		public void OnPart (byte [] data, int offset, int count)
		{
			stream.ReadUntil ("\r\n", OnHeader);
		}

		public void OnHeader (byte [] data, int offset, int count)
		{
			string str = encoding.GetString (data, offset, count);

			if (str == "\r\n") {
				stream.ReadUntil (boundary, OnBody);
				return;
			}

			if (str.StartsWith ("Content-Disposition:", StringComparison.InvariantCultureIgnoreCase)) {
				ParseContentDisposition (str);
			} else if (str.StartsWith ("Content-Type:", StringComparison.InvariantCultureIgnoreCase)) {
				ParseContentType (str);
			}

			stream.ReadUntil ("\r\n", OnHeader);
		}

		public void ParseContentDisposition (string str)
		{
			name = GetContentDispositionAttribute (str, "name");
			filename = GetContentDispositionAttributeWithEncoding (str, "filename");

			if (filename != null)
				stream.BeginFile (filename);
		}

		public void ParseContentType (string str)
		{
			content_type = str.Substring ("Content-Type:".Length).Trim ();
		}

		public void OnBody (byte [] data, int offset, int count)
		{
			byte [] trailer = encoding.GetBytes ("\r\n" + boundary + "--");
			int data_len = count - trailer.Length;

			if (filename == null) {
				// We are finishing form data
				string str = encoding.GetString (data, offset, data_len);
				if (name != null && str != null)
					form_data.Set (name, new UnsafeString (str));
			} else {
				UploadedFile uploaded = stream.EndFile ();
			   
//				uploaded.SetData (data, offset, data_len);
				request.Files.Add (filename, uploaded);
			}

			stream.ReadBytes (2, OnPostBody);
		}

		public void OnPostBody (byte [] data, int offset, int len)
		{
			string str = encoding.GetString (data, offset, len);

			if (str == "--") {
				EndParsing ();
				return;
			}
						   
			stream.ReadUntil (boundary, OnPart);
		}

		public void EndParsing ()
		{
			request.SetWwwFormData (form_data);
			complete ();
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

