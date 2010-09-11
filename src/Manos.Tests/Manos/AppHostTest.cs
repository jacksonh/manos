using System;
using System.Threading;

using NUnit.Framework;

using Manos.Server;
using Manos.ShouldExt;

namespace Manos.Tests
{
	[TestFixture()]
	public class AppHostTest
	{
		[Test()]
		public void Start_NullApp_Throws ()
		{
			Should.Throw<ArgumentNullException> (() => AppHost.Start (null));
		}
		
#if NO
		[Test]
		public void AddTimeout_TimeoutAddedBeforeStart_TimeoutIsFiredAfterStart ()
		{
			TimeSpan t = TimeSpan.FromMilliseconds (1);
			
			bool timeout_called = false;
			AppHost.AddTimeout (t, RepeatBehavior.Single, null, (app, data) => timeout_called = true);

			ThreadPool.QueueUserWorkItem (cb => AppHost.Start (new ManosAppStub ()));
			Thread.Sleep (TimeSpan.FromSeconds (5));
			AppHost.Stop ();
			
			Assert.IsTrue (timeout_called);
		}
#endif
	}
}

