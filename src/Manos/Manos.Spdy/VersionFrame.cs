using System;

namespace Manos.Spdy
{
	public class VersionFrame : ControlFrame
	{
		public int [] SupportedVersions { get; set; }

		public VersionFrame ()
		{
			this.Type = ControlFrameType.VERSION;
		}

		public VersionFrame (byte [] data,int offset,int length)
		{
			this.Type = ControlFrameType.VERSION;
			base.Parse (data, offset, length);
			int versionscount = Util.BuildInt (data, offset + 8, 4);
			int index = 12;
			this.SupportedVersions = new int[versionscount];
			for (int i = 0; i < versionscount; i++) {
				SupportedVersions [i] = Util.BuildInt (data, index, 2);
				index += 2;
			}
		}

		public new byte [] Serialize ()
		{
			this.Length = 4 + this.SupportedVersions.Length * 2;
			var headers = base.Serialize ();
			Array.Resize (ref headers, this.Length + 8);
			Util.IntToBytes (this.SupportedVersions.Length, ref headers, 8, 4);
			for (int i = 0; i < this.SupportedVersions.Length; i++) {
				Util.IntToBytes (this.SupportedVersions [i], ref headers, 12 + i * 2, 2);
			}
			Console.WriteLine (BitConverter.ToString (headers));
			return headers;
		}
	}
}

