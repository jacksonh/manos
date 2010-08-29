
using System;

namespace Manos.Testing
{


	public class MockManosTarget : IManosTarget
	{
		public MockManosTarget ()
		{
		}
		
		public void Invoke (IManosContext ctx)
		{
			throw new System.NotImplementedException();
		}
		
		
		public ManosAction Action {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
	}
}
