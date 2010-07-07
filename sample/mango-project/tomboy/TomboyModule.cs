

using System;

using Mango;


namespace MangoProject.Tomboy {

	public class TomboyModule : MangoModule {

		public override void OnStartup ()
		{
			Route ("notes/", new NotesModule ());
			// Route ("user/", new UserModule ());
		}
	}
}


