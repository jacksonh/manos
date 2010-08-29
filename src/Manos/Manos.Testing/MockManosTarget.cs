
using System;

namespace Mango.Testing
{


	public class MockMangoTarget : IMangoTarget
	{
		public MockMangoTarget ()
		{
		}
		
		public void Invoke (IMangoContext ctx)
		{
			throw new System.NotImplementedException();
		}
		
		
		public MangoAction Action {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
	}
}
