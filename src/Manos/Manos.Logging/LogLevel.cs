// 
// 
// Copyright (c) 2010 Jackson Harper (jackson@manosdemono.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


using System;

namespace Manos.Logging
{
	public enum LogLevel {

		/// <summary>
		/// Do not log any messages.
		/// </summary>
		None,
		
		/// <summary>
		///  Only log critical errors. These are errors that can not be recovered from.  Leaving
		///  the application in a volatile unusable state or crashed.
		/// </summary>
		Critical,

		/// <summary>
		///  Log Critical and normal errors. Normal errors should be able to recoverable
		///  but there might be an interuption in service.  Such as a request not being processed.
		/// </summary>
		Error,

		/// <summary>
		///  Log Application information, normal and critical errors.
		///  Application information is information that might be useful to administrators when
		///  settingup or tuning an application.
		/// </summary>
		Info,

		/// <summary>
		///  Log all messages including Debug information useful to developers.  This Should only be
		///  enabled when an error is being diagnosed.
		/// </summary>
		Debug,
	}
}



