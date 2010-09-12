


using System;

using Manos.Server;
using Manos.Routing;

namespace Manos {

	public class ManosApp : ManosModule, IManosPipe {

		public ManosApp ()
		{
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
				Console.Error.WriteLine ("Exception in transaction handler:");
				Console.Error.WriteLine (e);
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
					Console.Error.WriteLine ("Exception in {0}::OnPreProcessRequest.", pipe);
					Console.Error.WriteLine (e);
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
					Console.Error.WriteLine ("Exception in {0}::OnPreProcessTarget.", pipe);
					Console.Error.WriteLine (e);
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
					Console.Error.WriteLine ("Exception in {0}::OnPostProcessTarget.", pipe);
					Console.Error.WriteLine (e);
				}
			}
		}

		public void OnPostProcessRequest (ManosApp app, IHttpTransaction transaction)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnPostProcessRequest (this, transaction);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception in {0}::OnPostProcessRequest.", pipe);
					Console.Error.WriteLine (e);
				}
			}
		}

		public void OnError (IManosContext ctx)
		{
			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnError (ctx);
				} catch (Exception e) {
					Console.Error.WriteLine ("Exception in {0}::OnError", pipe);
					Console.Error.WriteLine (e);
				}
			}
		}
	}
}

