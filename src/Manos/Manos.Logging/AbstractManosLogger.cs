// 
// AbstractManosLogger.cs
//  
// Author:
//       Anirudh Sanjeev <anirudh@anirudhsanjeev.org>
// 
// Copyright (c) 2010 Anirudh Sanjeev
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

namespace Manos
{
	public abstract class AbstractManosLogger : IManosLogger
	{
		int threshold;
		string name;
		
		public abstract void OutputString (string message);
		
		private void WriteString(string level, string message, params object[] args)
		{
			string formattedMessage = String.Format (message, args);
			OutputString (String.Format("{0} [{1}] {2} - {3}", DateTime.Now.ToString("'HH':'mm':'ss.fffffff"), level, name, formattedMessage));			
		}
		
		#region IManosLogger implementation
		void IManosLogger.Fatal (string message, params object[] args)
		{
			if(threshold >= 4)
			{
				WriteString ("FATAL", message, args);
			}
		}
		
		
		void IManosLogger.Error (string message, params object[] args)
		{
			if(threshold >= 3)
			{
				WriteString ("FATAL", message, args);
			}
		}
		
		
		void IManosLogger.Warn (string message, params object[] args)
		{
			if(threshold >= 2)
			{
				WriteString ("FATAL", message, args);
			}
		}
		
		
		void IManosLogger.Info (string message, params object[] args)
		{
			if(threshold >= 1)
			{
				WriteString ("FATAL", message, args);
			}
		}
		
		
		void IManosLogger.Debug (string message, params object[] args)
		{
			if(threshold >= 0)
			{
				WriteString ("FATAL", message, args);
			}
		}
		
		
		void IManosLogger.Exception (string message, Exception ex)
		{
			if(threshold >= 5)
			{
				WriteString ("FATAL", "{0} - {1}", message, ex.Message, ex.StackTrace);
			}
		}
		#endregion
		public AbstractManosLogger (string name_, int threshold_)
		{
			name = name_;
			threshold = threshold_;
		}
	}
	
	public abstract class AbstractManosLoggerFactory
	{		
		public int DefaultThreshold{get;set;}
		public AbstractManosLoggerFactory()
		{
			
		}
		public abstract AbstractManosLogger CreateManosLoggerObject(string name, int threshold);
		
		public AbstractManosLogger CreateManosLogger (string name)
		{
			return CreateManosLoggerObject (name, DefaultThreshold);
		}
		
		public AbstractManosLogger CreateManosLogger (string name, int threshold)
		{
			return CreateManosLoggerObject (name, threshold);
		}
	}
}

