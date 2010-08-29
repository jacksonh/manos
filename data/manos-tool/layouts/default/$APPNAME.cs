
using Mango;


namespace $APPNAME {

	public class $APPNAME : MangoApp {

		public $APPNAME ()
		{
			Route ("Content/", new StaticContentModule ());
		}
	}
}
