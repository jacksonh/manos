using System;
using System.Collections.Generic;
using Manos.Collections;

namespace Manos.IO.Libev
{
	class StreamCopySequencer : IEnumerable<ByteBuffer>
	{
		Stream source, target;
		ByteBuffer currentBuffer;
		bool active;

		public StreamCopySequencer (Stream source, Stream target)
		{
			this.source = source;
			this.target = target;
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

