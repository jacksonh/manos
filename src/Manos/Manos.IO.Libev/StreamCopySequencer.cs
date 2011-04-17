using System;
using System.Collections.Generic;
using Manos.Collections;

namespace Manos.IO.Libev
{
	class StreamCopySequencer : IEnumerable<ByteBuffer>
	{
		Stream source, target;
		ByteBuffer currentBuffer;
		bool active, ownsSource;

		public StreamCopySequencer (Stream source, Stream target, bool ownsSource)
		{
			this.source = source;
			this.target = target;
			this.ownsSource = ownsSource;
		}

		IEnumerable<ByteBuffer> CopySequencer ()
		{
			active = true;
			source.Read (OnSourceData, OnSourceError, OnSourceClose);
			yield return new ByteBuffer(new byte[0], 0, 0);
			while (active) {
				var buffer = currentBuffer;
				target.PauseWriting ();
				source.ResumeReading ();
				yield return buffer;
			}
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
			target.ResumeWriting ();
			source.PauseReading ();
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

