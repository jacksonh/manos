using System;
using System.Collections.Generic;

namespace Manos.IO
{
	class StreamCopySequencer : IEnumerable<ByteBuffer>
	{
		IByteStream source, target;
		ByteBuffer currentBuffer;
		bool active, ownsSource;

		public StreamCopySequencer (IByteStream source, IByteStream target, bool ownsSource)
		{
			this.source = source;
			this.target = target;
			this.ownsSource = ownsSource;
		}

		IEnumerable<ByteBuffer> CopySequencer ()
		{
			active = true;
			var reader = source.Read (OnSourceData, OnSourceError, OnSourceClose);
			target.PauseWriting ();
			yield return new ByteBuffer(new byte[0], 0, 0);
			while (active) {
				var buffer = currentBuffer;
				target.PauseWriting ();
				source.ResumeReading ();
				yield return buffer;
			}
			reader.Dispose ();
			if (ownsSource) {
				source.Close ();
			}
			source = null;
			target = null;
			currentBuffer = null;
		}

		void OnSourceData (ByteBuffer buffer)
		{
			currentBuffer = buffer;
			source.PauseReading ();
			target.ResumeWriting ();
		}

		void OnSourceClose ()
		{
			active = false;
			target.ResumeWriting ();
		}

		void OnSourceError (Exception error)
		{
			active = false;
			target.ResumeWriting ();
		}

		public IEnumerator<ByteBuffer> GetEnumerator ()
		{
			return CopySequencer ().GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}

