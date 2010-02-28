
using System;
using System.Collections.Generic;


namespace Mango.Templates.Minge {


	public class TokenOperators {

		private Dictionary<int,TokenType> single_char_ops = new Dictionary<int,TokenType> ();
		private Dictionary<string,TokenType> double_char_ops = new Dictionary<string,TokenType> ();

		public TokenOperators (Environment env)
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
