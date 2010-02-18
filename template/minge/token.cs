

using System;


namespace Mango.Templates.Minge {

	public class Token {

		public Token (int line, int col, TokenType type, string value)
		{
			Line = line;
			Column = col;
			Type = type;
			Value = value;
		}

		public Token (int line, int col, TokenType type, string value, object tok_value) : this (line, col, type, value)
		{
			TokenizedValue = tok_value;
		}

		public int Line {
			get;
			private set;
		}

		public int Column {
			get;
			private set;
		}

		public TokenType Type {
			get;
			private set;
		}

		public string Value {
			get;
			private set;
		}

		public object TokenizedValue {
			get;
			private set;
		}
	}
}

