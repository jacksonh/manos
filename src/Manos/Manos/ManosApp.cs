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

using Manos.Server;
using Manos.Routing;

namespace Manos {

	public class ManosApp : ManosModule, IManosPipe {
		IManosLogger Log;

		public ManosApp ()
		{
			Log = ManosLogManager.GetLogger ("ManosApp");
		}

		public void HandleTransaction (ManosApp app, IHttpTransaction con)
		{
			if (con == null)
				throw new ArgumentNullException ("con");

			OnPreProcessRequest (this, con);
			if (con.Aborted)
				goto cleanup;
			
			var ctx = new ManosContext (con);
			
			var handler = OnPreProcessTarget (ctx);
			if (handler == null)
				handler = Routes.Find (con.Request);
			
			if (handler == null) {
				con.Response.StatusCode = 404;
				con.Response.Finish ();
				return;
			}

			OnPostProcessTarget (ctx, handler);
			
			try {
				handler.Invoke (app, new ManosContext (con));
			} catch (Exception e) {
				Log.Error ("Exception in transaction handler:");
				Log.Exception (e);
				con.Response.StatusCode = 500;
				//
				// TODO: Maybe the cleanest thing to do is
				// have a HandleError, HandleException thing
				// on HttpTransaction, along with an UnhandledException
				// method/event on ManosModule.
				//
			}

		cleanup:
			con.Response.Finish ();
			
			OnPostProcessRequest (this, con);
		}

		public void OnPreProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnPreProcessRequest (app, transaction);
					
					if (transaction.Aborted)
						return;
					
				} catch (Exception e) {
					Log.Error ("Exception in {0}::OnPreProcessRequest.", pipe);
					Log.Exception (e);
				}
			}
		}

		public IManosTarget OnPreProcessTarget (IManosContext ctx)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					var found = pipe.OnPreProcessTarget (ctx);
					
					if (found != null)
						return found;

					if (ctx.Transaction.Aborted)
						return null;
				} catch (Exception e) {
					Log.Error ("Exception in {0}::OnPreProcessTarget.", pipe);
					Log.Exception (e);
				}
			}
			
			return null;
		}

		public void OnPostProcessTarget (IManosContext ctx, IManosTarget target)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnPostProcessTarget (ctx, target);
					
					if (ctx.Transaction.Aborted)
						return;
				} catch (Exception e) {
					Log.Error ("Exception in {0}::OnPostProcessTarget.", pipe);
					Log.Exception (e);
				}
			}
		}

		public void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnPostProcessRequest (this, transaction);
				} catch (Exception e) {
					Log.Error ("Exception in {0}::OnPostProcessRequest.", pipe);
					Log.Exception (e);
				}
			}
		}

		public void OnError (IManosContext ctx)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnError (ctx);
				} catch (Exception e) {
					Log.Error ("Exception in {0}::OnError", pipe);
					Log.Exception (e);
				}
			}
		}
	}
}

