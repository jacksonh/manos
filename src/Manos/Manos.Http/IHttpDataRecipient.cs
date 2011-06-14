using System;
using System.Text;
using Manos.Collections;
using System.Collections.Generic;

namespace Manos.Http
{
	public interface IHttpDataRecipient
	{
		Encoding ContentEncoding { get; set; }
		DataDictionary PostData { get; set; }
		Dictionary<string,UploadedFile> Files { get; }
		string PostBody { get; set; }
	}
}

