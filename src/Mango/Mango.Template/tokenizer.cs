
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Mango.Templates {

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

	public class MingeTokenizer {

		private TokenOperators token_ops;
		private Token current;

		private int col;
		private int line;
		private int position;

		private bool have_unget;
		private int unget_value;

		private MingeEnvironment environment;

		private string source;
		private TextReader reader;
		private StringBuilder builder;

		bool in_code_block = false;

		public MingeTokenizer (MingeEnvironment env, TextReader reader)
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

				if (in_code_block && (c == '\'' || c == '\"')) {
					string str = ReadQuotedString (c);
					Console.WriteLine ("READ QUOTED STRING:  {0}", str);
					if (str == null)
						return new Token (line, col, TokenType.TOKEN_EOF, String.Empty);
					tok = new Token (line, col, TokenType.TOKEN_QUOTED_STRING, str);
					return tok;
				}
				
				int d = reader.Peek ();
				if (in_code_block && IsNumberStartChar (c) && IsNumberChar (d)) {
					object number = ReadNumber (c, d, out tok_type);
					tok = new Token (line, col, tok_type, number.ToString (), number);
					return tok;
				}

				string two_chars = String.Concat ((char) c, (char) d);
				if (token_ops.DoubleCharOps.TryGetValue (two_chars, out tok_type)) {
					tok = new Token (line, col, tok_type, two_chars);
					reader.Read ();
					UpdateInCodeBlock (tok_type);
					return tok;
				}

				if (token_ops.SingleCharOps.TryGetValue (c, out tok_type)) {
					tok = new Token (line, col, tok_type, "" + (char) c);
					return tok;
				}

				if (in_code_block) {
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

		private void UpdateInCodeBlock (TokenType tok_type)
		{
			switch (tok_type) {
			case TokenType.TOKEN_BLOCK_END:
			case TokenType.TOKEN_VARIABLE_END:
				in_code_block = false;
				break;
			case TokenType.TOKEN_BLOCK_BEGIN:
			case TokenType.TOKEN_VARIABLE_BEGIN:
				in_code_block = true;
				break;
			default:
				break;
			}
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

		
		private static readonly string numchars = ".";
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

	
	public enum TokenType {
		TOKEN_ADD,
		TOKEN_ASSIGN,
		TOKEN_COLON,
    		TOKEN_COMMA,
    		TOKEN_DIV,
		TOKEN_DOT,
		TOKEN_DOUBLE,
		TOKEN_EQ,
		TOKEN_FLOORDIV,
		TOKEN_GT,
		TOKEN_GTEQ,
		TOKEN_LBRACE,
		TOKEN_LBRACKET,
		TOKEN_LPAREN,
		TOKEN_LT,
		TOKEN_LTEQ,
		TOKEN_MOD,
		TOKEN_MUL,
		TOKEN_NE,
		TOKEN_PIPE,
		TOKEN_POW,
		TOKEN_RBRACE,
		TOKEN_RBRACKET,
		TOKEN_RPAREN,
		TOKEN_SEMICOLON,
		TOKEN_SUB,
		TOKEN_TILDE,
		TOKEN_WHITESPACE,
		TOKEN_INTEGER,
		TOKEN_NAME,
		TOKEN_STRING,
		TOKEN_QUOTED_STRING,
		TOKEN_OPERATOR,
		TOKEN_BLOCK_BEGIN,
		TOKEN_BLOCK_END,
		TOKEN_VARIABLE_BEGIN,
		TOKEN_VARIABLE_END,
		TOKEN_RAW_BEGIN,
		TOKEN_RAW_END,
		TOKEN_COMMENT_BEGIN,
		TOKEN_COMMENT_END,
		TOKEN_COMMENT,
		TOKEN_DATA,
		TOKEN_INITIAL,
		TOKEN_EOF,
    	}

	
	public class TokenOperators {

		private Dictionary<int,TokenType> single_char_ops = new Dictionary<int,TokenType> ();
		private Dictionary<string,TokenType> double_char_ops = new Dictionary<string,TokenType> ();

		public TokenOperators (MingeEnvironment env)
		{
			single_char_ops.Add ('+', TokenType.TOKEN_ADD);
			single_char_ops.Add ('-', TokenType.TOKEN_SUB);
			single_char_ops.Add ('/', TokenType.TOKEN_DIV);
			single_char_ops.Add ('*', TokenType.TOKEN_MUL);
			single_char_ops.Add ('%', TokenType.TOKEN_MOD);
			single_char_ops.Add ('~', TokenType.TOKEN_TILDE);
			single_char_ops.Add ('[', TokenType.TOKEN_LBRACKET);
			single_char_ops.Add (']', TokenType.TOKEN_RBRACKET);
			single_char_ops.Add ('(', TokenType.TOKEN_LPAREN);
			single_char_ops.Add (')', TokenType.TOKEN_RPAREN);
			single_char_ops.Add ('{', TokenType.TOKEN_LBRACE);
			single_char_ops.Add ('}', TokenType.TOKEN_RBRACE);
			single_char_ops.Add ('>', TokenType.TOKEN_GT);
			single_char_ops.Add ('<', TokenType.TOKEN_LT);
			
			single_char_ops.Add ('=', TokenType.TOKEN_ASSIGN);
			single_char_ops.Add ('.', TokenType.TOKEN_DOT);
			single_char_ops.Add (':', TokenType.TOKEN_COLON);
			single_char_ops.Add ('|', TokenType.TOKEN_PIPE);
			single_char_ops.Add (',', TokenType.TOKEN_COMMA);
			single_char_ops.Add (';', TokenType.TOKEN_SEMICOLON);

			double_char_ops.Add ("//", TokenType.TOKEN_FLOORDIV);
			double_char_ops.Add ("**", TokenType.TOKEN_POW);
			double_char_ops.Add ("==", TokenType.TOKEN_EQ);
			double_char_ops.Add ("!=", TokenType.TOKEN_NE);
			double_char_ops.Add (">=", TokenType.TOKEN_GTEQ);
			double_char_ops.Add ("<=", TokenType.TOKEN_LTEQ);

			// These should be environment dependendant
			double_char_ops.Add (env.BlockStartString, TokenType.TOKEN_BLOCK_BEGIN);
			double_char_ops.Add (env.BlockEndString, TokenType.TOKEN_BLOCK_END);
			double_char_ops.Add (env.VariableStartString, TokenType.TOKEN_VARIABLE_BEGIN);
			double_char_ops.Add (env.VariableEndString, TokenType.TOKEN_VARIABLE_END);
			double_char_ops.Add (env.CommentStartString, TokenType.TOKEN_COMMENT_BEGIN);
			double_char_ops.Add (env.CommentEndString, TokenType.TOKEN_COMMENT_END);
		}

		public Dictionary<int,TokenType> SingleCharOps {
			get { return single_char_ops; }
		}

		public Dictionary<string,TokenType> DoubleCharOps {
			get { return double_char_ops; }
		}
	}
}

