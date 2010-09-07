
using System;

namespace Manos.Routing.Testing
{
	public class MockManosTarget : IManosTarget
	{
		public MockManosTarget ()
		{
		}
		
		public void Invoke (ManosApp app, IManosContext ctx)
		{
			throw new System.NotImplementedException();
		}
		
		
		public Delegate Action {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
	}
}
