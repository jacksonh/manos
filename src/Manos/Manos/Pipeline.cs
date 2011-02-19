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
using System.Runtime.InteropServices;


using Manos.Http;
using Manos.Routing;

namespace Manos {

	public enum PipelineStep {
		PreProcess,
		Execute,
		WaitingForEnd,
		PostProcess,
		Complete
	}

	/// <summary>
	/// A pipeline coordinates an httprequest/response session.  Making sure all the ManosPipes are invoked
	/// and invoking the actual request exectution.
	/// </summary>
	/// <remarks>
	/// User code should typically not use this type.
	/// </remarks>
	public class Pipeline {

		private ManosApp app;
		private ManosContext ctx;
		private IHttpTransaction transaction;

		private int pending;
		private PipelineStep step;
		private GCHandle handle;

		public Pipeline (ManosApp app, IHttpTransaction transaction)
		{
			this.app = app;
			this.transaction = transaction;

			pending = 1;
			step = PipelineStep.PreProcess;
			handle = GCHandle.Alloc (this);

			transaction.Response.OnEnd += HandleEnd;
		}

		public void Begin ()
		{
			if (AppHost.Pipes == null) {
				StepCompleted ();
				return;
			}

			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnPreProcessRequest (app, transaction, StepCompleted);
				} catch (Exception e) {
					pending--;

					Console.Error.WriteLine ("Exception in {0}::OnPreProcessRequest.", pipe);
					Console.Error.WriteLine (e);
				}
			}
		}

		private void Execute ()
		{
			step = PipelineStep.WaitingForEnd;

			ctx = new ManosContext (transaction);
			
			// TODO:
			// var handler = OnPreProcessTarget (ctx);
			// if (handler == null)
			var handler = app.Routes.Find (transaction.Request);
			
			if (handler == null) {
				ctx.Response.StatusCode = 404;
				ctx.Response.End ();
				return;
			}

			ctx.Response.StatusCode = 200;

			// TODO: 
			// OnPostProcessTarget (ctx, handler);

			try {
				handler.Invoke (app, ctx);
			} catch (Exception e) {
				Console.Error.WriteLine ("Exception in transaction handler:");
				Console.Error.WriteLine (e);
				ctx.Response.StatusCode = 500;
				ctx.Response.End ();
				//
				// TODO: Maybe the cleanest thing to do is
				// have a HandleError, HandleException thing
				// on HttpTransaction, along with an UnhandledException
				// method/event on ManosModule.
				//
				// end = true;
			}

			if (ctx.Response.StatusCode == 404) {
				step = PipelineStep.WaitingForEnd;
				ctx.Response.End ();
				return;
			}
		}

		private void PostProcess ()
		{
			if (AppHost.Pipes == null) {
				StepCompleted ();
				return;
			}

			foreach (IManosPipe pipe in AppHost.Pipes) {
				try {
					pipe.OnPostProcessRequest (app, transaction, StepCompleted);
					
					if (ctx.Transaction.Aborted)
						return;
				} catch (Exception e) {
					pending--;

					Console.Error.WriteLine ("Exception in {0}::OnPostProcessRequest.", pipe);
					Console.Error.WriteLine (e);
				}
			}
		}

		private void Complete ()
		{
			transaction.Response.Complete (transaction.OnResponseFinished);

			handle.Free ();
		}

		private void StepCompleted ()
		{
			if (--pending > 0)
				return;

			pending = AppHost.Pipes == null ? 1 : AppHost.Pipes.Count;
			step++;

			switch (step) {
			case PipelineStep.Execute:
				Execute ();
				break;
			case PipelineStep.PostProcess:
				PostProcess ();
				break;
			case PipelineStep.Complete:
				Complete ();
				break;
			}
		}

		private void HandleEnd ()
		{
			pending = 0;
			StepCompleted ();
		}
	}

}


