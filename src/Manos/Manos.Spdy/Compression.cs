using System;
using Ionic.Zlib;
using System.Text;

namespace Manos.Spdy
{
	public abstract class ZlibContext
	{
		internal static string dict = "optionsgetheadpostputdeletetraceacceptaccept-charsetaccept-encodingaccept-" + 
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
								".1statusversionurl" + char.MinValue;
		internal ZlibCodec stream;
		internal int bytes;

		public ZlibContext ()
		{
			stream = new ZlibCodec ();
		}
	}

	public class InflatingZlibContext : ZlibContext
	{
		public InflatingZlibContext () : base()
		{
			this.stream.InitializeInflate ();
		}

		public byte [] Inflate (byte [] input, int offset, int length)
		{
			byte[] output = new byte[40000];
			stream.InputBuffer = input;
			stream.NextIn = offset;
			stream.AvailableBytesIn = length;
			stream.OutputBuffer = output;
			stream.NextOut = 0;
			stream.AvailableBytesOut = output.Length;
			int error = stream.Inflate (FlushType.Sync);
			if (error == ZlibConstants.Z_NEED_DICT) {
				stream.SetDictionary (Encoding.UTF8.GetBytes (dict));
				stream.Inflate (FlushType.Sync);
			}
			int ret = (int) stream.TotalBytesOut - bytes;
			bytes = (int) stream.TotalBytesOut;
			Array.Resize (ref output, ret);
			return output;
		}

	}

	public class DeflatingZlibContext : ZlibContext
	{
		public DeflatingZlibContext () : base()
		{
			this.stream.InitializeDeflate ();
		}

		public byte [] Deflate (byte [] input, int offset, int length)
		{
			byte[] output = new byte[40000];
			stream.InputBuffer = input;
			stream.NextIn = offset;
			stream.AvailableBytesIn = length;
			stream.OutputBuffer = output;
			stream.NextOut = 0;
			stream.AvailableBytesOut = output.Length;
			int error = stream.Deflate (FlushType.Sync);
			if (error == ZlibConstants.Z_NEED_DICT) {
				stream.SetDictionary (Encoding.UTF8.GetBytes (dict));
				stream.Deflate (FlushType.Sync);
			}
			int ret = (int) stream.TotalBytesOut - bytes;
			bytes = (int) stream.TotalBytesOut;
			Array.Resize (ref output, ret);
			return output;
		}
	}
}

