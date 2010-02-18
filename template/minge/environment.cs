
using System;

namespace Mango.Templates.Minge {

	public class Environment {

		private static readonly string DefaultCommentStartString = "{#";
		private static readonly string DefaultCommentEndString = "#}";

		private static readonly string DefaultBlockStartString = "{%";
		private static readonly string DefaultBlockEndString = "%}";
		
		private static readonly string DefaultVariableStartString = "{{";
		private static readonly string DefaultVariableEndString = "}}";


		private string comment_start_string = DefaultCommentStartString;
		private string comment_end_string = DefaultCommentEndString;

		private string block_start_string = DefaultBlockStartString;
		private string block_end_string = DefaultBlockEndString;

		private string variable_start_string = DefaultVariableStartString;
		private string variable_end_string = DefaultVariableEndString;		


		private bool is_running = false;

		public string CommentStartString {
			get { return comment_start_string; }
			set {
				if (is_running)
					throw new Exception ();
				if (value.Length != 2)
					throw new ArgumentException ("CommentStartString", "Must be a two char string.");
				comment_start_string = value;
			}
		}

		public string CommentEndString {
			get { return comment_end_string; }
			set {
				if (is_running)
					throw new Exception ();
				if (value.Length != 2)
					throw new ArgumentException ("CommentEndString", "Must be a two char string.");
				comment_end_string = value;
			}
		}

		public string BlockStartString {
			get { return block_start_string; }
			set {
				if (is_running)
					throw new Exception ();
				if (value.Length != 2)
					throw new ArgumentException ("BlockStartString", "Must be a two char string.");
				block_start_string = value;
			}
		}

		public string BlockEndString {
			get { return block_end_string; }
			set {
				if (is_running)
					throw new Exception ();
				if (value.Length != 2)
					throw new ArgumentException ("BlockEndString", "Must be a two char string.");
				block_end_string = value;
			}
		}

		public string VariableStartString {
			get { return variable_start_string; }
			set {
				if (is_running)
					throw new Exception ();
				if (value.Length != 2)
					throw new ArgumentException ("VariableStartString", "Must be a two char string.");
				variable_start_string = value;
			}
		}

		public string VariableEndString {
			get { return variable_end_string; }
			set {
				if (is_running)
					throw new Exception ();
				if (value.Length != 2)
					throw new ArgumentException ("VariableEndString", "Must be a two char string.");
				variable_end_string = value;
			}
		}
	}

}

