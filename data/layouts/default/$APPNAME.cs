
using Manos;
using System;

namespace $APPNAME {

	public class $APPNAME : ManosApp {

		public $APPNAME ()
		{
			Route ("/Content/", new StaticContentModule ());
		}
	}
}
