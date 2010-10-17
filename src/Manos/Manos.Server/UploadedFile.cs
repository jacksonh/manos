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

//

using System;
using System.IO;

namespace Manos.Server {

	  public abstract class UploadedFile {

	  	 public UploadedFile (string name)
		 {
			Name = name;
		 }

	  	 public string Name {
		 	get;
			private set;
		 }

		 public string ContentType {
		 	get;
			set;
		 }

		 public abstract long Length {
		 	get;
		 }

		 public abstract Stream Contents {
		 	get;
		 }

		 // TEMP: Eventually this should be streamed to the file
		 public abstract void SetData (byte [] data, int index, int count);

		 // Probably best to not use this method as it creates a (potentially) huge buffer
		 public abstract byte [] GetData ();
	  }

	  public class InMemoryUploadedFile : UploadedFile {

	  	 private byte [] data;

	  	 public InMemoryUploadedFile (string name) : base (name)
		 {
		 }
		 
		 public override long Length {
		 	get {
			    if (data == null)
			       return 0;
			    return data.Length;
			}
		 }

		 public override Stream Contents {
		 	get {
			    return new MemoryStream (data);
			}
		 }

		 public override void SetData (byte [] src, int index, int count)
		 {
			data = new byte [count];

			Array.Copy (src, index, data, 0, count);
		 }

		 public override byte [] GetData ()
		 {
			return data;
		 }
	  }

	  public class TempFileUploadedFile : UploadedFile {

	  	 public TempFileUploadedFile (string name, string temp_file) : base (name)
		 {
			TempFile = temp_file;
		 }
	  	 
		 public string TempFile {
		 	get;
			private set;
		 }

		 public override long Length {
		 	get {
			    FileInfo f = new FileInfo (TempFile);
			    return f.Length;
			}
		 }

		 public override Stream Contents {
		 	get {
			    return File.OpenRead (TempFile);
			}
		 }

		 public override void SetData (byte [] data, int index, int count)
		 {
			using (var stream = File.OpenWrite (TempFile)) {
			      stream.Write (data, index, count);
			}
		 }

		 public override byte [] GetData ()
		 {
			return File.ReadAllBytes (TempFile);
		 }
	  }
}

