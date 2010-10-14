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

