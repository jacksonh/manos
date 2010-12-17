//
// Copyright (C) 2010 Anirudh Sanjeev (a@ninjagod.com)
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

namespace Manos.Logging
{
	public interface IManosLogger
	{
		LogLevel Level {
			get;
		}

		/// <summary>
		///  An error that can not be recovered from has occurred.  The application is
		///  now in a volatile state or has crashed.
		/// </summary>
		void Critical (string message, params object[] args);

		/// <summary>
		///  An application error has occurred.  The application should be able to recover
		///  from this type of error, but there might be an interuption in service.  Such
		///  as a request not being processed.
		/// </summary>
		void Error (string message, params object[] args);

		/// <summary>
		///  Application information, that might be useful to administrators when setting
		///  up or tuning an application.
		/// </summary>
		void Info (string message, params object[] args);

		/// <summary>
		///  Debug information useful to developers.  Should only be enabled when an
		///  error is being diagnosed.
		/// </summary>
		void Debug (string message, params object[] args);
	}
}

