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


namespace Manos
{
	/// <summary>
	/// Provides a mechanism for things to happen periodically within a ManosApp. 
	/// Timeouts are gauranteed to happen at some moment on or after the TimeSpan specified has ellapsed.
	/// 
	/// Timeouts will never run before the specified TimeSpan has ellapsed.
	/// 
	/// Use the method <see cref="Manos.IO.IOLoop"/> "AddTimeout" method to register each Timeout.
	/// </summary>
	public class Timeout
	{
		internal TimeSpan begin;
		internal TimeSpan span;
		internal IRepeatBehavior repeat;
		internal object data;
		internal TimeoutCallback callback;

		public Timeout (TimeSpan span, IRepeatBehavior repeat, object data, TimeoutCallback callback) : this (TimeSpan.Zero, span, repeat,data, callback)
		{
		}

		public Timeout (TimeSpan begin, TimeSpan span, IRepeatBehavior repeat, object data, TimeoutCallback callback)
		{
			this.begin = begin;
			this.span = span;
			this.repeat = repeat;
			this.data = data;
			this.callback = callback;
		}
		
		/// <summary>
		/// Causes the action specified in the constructor to be executed. Infrastructure.
		/// </summary>
		/// <param name="app">
		/// A <see cref="ManosApp"/>
		/// </param>
		public void Run (ManosApp app)
		{
			try {
				callback (app, data);
			} catch (Exception e) {
				Console.Error.WriteLine ("Exception in timeout callback.");
				Console.Error.WriteLine (e);
			}
			
			repeat.RepeatPerformed ();
		}
		
		/// <summary>
		/// Inidicates that the IOLoop should retain this timeout, because it will be run again at some point in the future. Infrastructure.
		/// </summary>
		public bool ShouldContinueToRepeat ()
		{
			return repeat.ShouldContinueToRepeat ();	
		}
	}
}

