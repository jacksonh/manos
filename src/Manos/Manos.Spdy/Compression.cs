using System;
using Ionic.Zlib;
using System.Text;

namespace Manos.Spdy
{
	public static class Compression
	{
		private static string dict = "optionsgetheadpostputdeletetraceacceptaccept-charsetaccept-encodingaccept-" +
							 "languageauthorizationexpectfromhostif-modified-sinceif-matchif-none-matchi" +
						  	 "f-rangeif-unmodifiedsincemax-forwardsproxy-authorizationrangerefererteuser" +
							 "-agent10010120020120220320420520630030130230330430530630740040140240340440" +
							 "5406407408409410411412413414415416417500501502503504505accept-rangesageeta" +
							 "glocationproxy-authenticatepublicretry-afterservervarywarningwww-authentic" +
							 "ateallowcontent-basecontent-encodingcache-controlconnectiondatetrailertran" +
							 "sfer-encodingupgradeviawarningcontent-languagecontent-lengthcontent-locati" +
							 "oncontent-md5content-rangecontent-typeetagexpireslast-modifiedset-cookieMo" +
							 "ndayTuesdayWednesdayThursdayFridaySaturdaySundayJanFebMarAprMayJunJulAugSe" +
							 "pOctNovDecchunkedtext/htmlimage/pngimage/jpgimage/gifapplication/xmlapplic" +
							 "ation/xhtmltext/plainpublicmax-agecharset=iso-8859-1utf-8gzipdeflateHTTP/1" +
							 ".1statusversionurl"; // + char.MinValue;
		public static int Inflate(byte[] input, int offset, int length, out byte[] output)
		{
			int error;
			output = new byte[40000];
			ZlibCodec stream = new ZlibCodec();
			stream.InputBuffer = input;
			stream.NextIn = offset;
			stream.AvailableBytesIn = length;
			error = stream.InitializeInflate();
			CheckForError(stream, error, "Init");
			stream.OutputBuffer = output;
			stream.NextOut = 0;
			stream.AvailableBytesOut = output.Length;
			error = stream.Inflate(FlushType.Sync);
			if (error == ZlibConstants.Z_NEED_DICT) {
				error = stream.SetDictionary(Encoding.ASCII.GetBytes(dict));
			}
			error = stream.Inflate(FlushType.Sync);
			CheckForError(stream, error, "inflate");
			error = stream.EndInflate();
			CheckForError(stream, error, "End");
			return (int)stream.TotalBytesOut;
		}
		public static int Deflate(byte[] input, int offset, int length, out byte[] output)
		{
			int error;
			output = new byte[40000];
			ZlibCodec stream = new ZlibCodec();
			stream.InputBuffer = input;
			stream.NextIn = offset;
			stream.AvailableBytesIn = length;
			error = stream.InitializeDeflate();
			CheckForError(stream, error, "Init");
			stream.OutputBuffer = output;
			stream.NextOut = 0;
			stream.AvailableBytesOut = output.Length;
			error = stream.Deflate(FlushType.Sync);
			if (error == ZlibConstants.Z_NEED_DICT) {
				error = stream.SetDictionary(Encoding.ASCII.GetBytes(dict));
			}
			error = stream.Deflate(FlushType.Sync);
			CheckForError(stream, error, "inflate");
			error = stream.EndDeflate();
			CheckForError(stream, error, "End");
			return (int)stream.TotalBytesOut;
		}
		internal static void CheckForError(ZlibCodec z, int err, System.String msg)
    	{
        	if (err != ZlibConstants.Z_OK)
        	{
            	//if (z.Message != null)
            	    //System.Console.Out.Write(z.Message + " ");
            	//System.Console.Out.WriteLine(msg + " error: " + err);

            	//System.Environment.Exit(1);
        	}
    	}
	}
}

