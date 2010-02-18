
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;


namespace Mango.Templates.Minge {

	public class MingeTokenizer {

		private TokenOperators token_ops;
		private Token current;

		private int col;
		private int line;
		private int position;

		private bool have_unget;
		private int unget_value;

		private Environment environment;

		private string source;
		private TextReader reader;
		private StringBuilder builder;


		public MingeTokenizer (Environment env, TextReader reader)
		{
			this.environment = env;
			this.reader = reader;
			builder = new StringBuilder ();

			token_ops = new TokenOperators (environment);
		}

		public Token Current {
			get { return current; }
		}

		public int Line {
			get { return line; }
		}

		public int Column {
			get { return col; }
		}

		public Token GetNextToken ()
		{
			current = _GetNextToken ();
			return current;
		}

		private Token _GetNextToken ()
		{
			int c;
			Token tok = null;
			TokenType tok_type;

			builder.Length = 0;

			while ((c = ReadChar ()) != -1) {

				if (c == '\'' || c == '\"') {
					string str = ReadQuotedString (c);
					if (str == null)
						return new Token (line, col, TokenType.TOKEN_EOF, String.Empty);
					tok = new Token (line, col, TokenType.TOKEN_QUOTED_STRING, str);
					return tok;
				}

				int d = reader.Peek ();
				if (IsNumberChar (c) && IsNumberChar (d)) {
					object number = ReadNumber (c, d, out tok_type);
					tok = new Token (line, col, tok_type, number.ToString (), number);
					return tok;
				}

				string two_chars = String.Concat ((char) c, (char) d);
				if (token_ops.DoubleCharOps.TryGetValue (two_chars, out tok_type)) {
					tok = new Token (line, col, tok_type, two_chars);
					reader.Read ();
					return tok;
				}

				if (token_ops.SingleCharOps.TryGetValue (c, out tok_type)) {
					tok = new Token (line, col, tok_type, "" + (char) c);
					return tok;
				}

				if (Char.IsWhiteSpace ((char) c)) {
					tok = new Token (line, col, TokenType.TOKEN_WHITESPACE, "" + (char) c);
					return tok;
				}

				if (IsNameStartChar (c)) {
					do {
						builder.Append ((char) c);
						c = ReadChar ();
					} while (IsNameChar (c));
					PutbackChar (c);
					tok = new Token (line, col, TokenType.TOKEN_NAME, builder.ToString ());
					return tok;
				}

				tok = new Token (line, col, TokenType.TOKEN_DATA, "" + (char) c);
				return tok;
			}

			return new Token (line, col, TokenType.TOKEN_EOF, "");
		}

		private void PutbackChar (int c)
		{
			have_unget = true;
			unget_value = c;
		}

		private int ReadChar ()
		{
			int c;
			if (have_unget) {
				c = unget_value;
				have_unget = false;
			} else {
				c = reader.Read ();
			}
			
			if (c == '\r' && reader.Peek () == '\n') {
				c = reader.Read ();
				position++;
			}
			
			if (c == '\n'){
				col = -1;
				line++;
			}
			
			if (c != -1) {
				col++;
				position++;
			}

			return c;
		}

		private string ReadQuotedString (int c)
		{
			int quote_char = c;

			do {
				builder.Append ((char) c);
				c = ReadChar ();
			} while (c != quote_char && c != -1);

			if (c == -1)
				return null;

			builder.Append ((char) c);
			return builder.ToString ();
		}

		private static readonly string idchars = "_";

		private static bool IsNameStartChar (int ch)
                {
                        return (Char.IsLetter ((char) ch) || (idchars.IndexOf ((char) ch) != -1));
                }

                private static bool IsNameChar (int ch)
                {
                        return (Char.IsLetterOrDigit ((char) ch) || (idchars.IndexOf ((char) ch) != -1));
                }

		private static readonly string numstartchars = "-";
		private static bool IsNumberStartChar (int c)
		{
			return (Char.IsDigit ((char) c) || (numstartchars.IndexOf ((char) c) != -1));
		}

		
		private static readonly string numchars = "-.";
		private static bool IsNumberChar (int c)
		{
			return (Char.IsDigit ((char) c) || (numchars.IndexOf ((char) c) != -1));
		}

		private object ReadNumber (int c, int d, out TokenType tok_type)
		{
			StringBuilder builder = new StringBuilder ();
			bool is_double = false;
			object number;

			builder.Append ((char) c);

			c = ReadChar ();
			while (IsNumberChar (c)) {
				if (c == '.')
					is_double = true;
				builder.Append ((char) c);
				c = ReadChar ();
			}

			if (is_double) {
				tok_type = TokenType.TOKEN_DOUBLE;
				number = Double.Parse (builder.ToString ());
			} else {
				tok_type = TokenType.TOKEN_INTEGER;
				number = Int32.Parse (builder.ToString ());
			}

			PutbackChar (c);

			return number;
		}
	}
}

