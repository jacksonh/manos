

using System;

using Mango;


namespace MangoProject.Tomboy {

	public class NotesModule : MangoModule {

		public override void OnStartup ()
		{
			Get (@"(?P<note_id>\d+)/$", GetNote);
			Post (@"(?P<note_id>\d+)/$", PostNote);
		}

		public static void GetNote (MangoContext context)
		{

		}

		public static void PostNote (MangoContext context)
		{

		}
	}
}


