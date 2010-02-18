
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace Mango.Templates.Minge {

	public enum TokenRuleState {
		None,
		Pop,
		ByGroup
	}

	public class TokenRule {

		public TokenRule (string regex, TokenType start_token, TokenType end_token, TokenRuleState new_state)
		{
			Regex = new Regex (regex);
			StartToken = start_token;
			EndToken = end_token;
			NewState = new_state;
		}

		public Regex Regex {
			get;
			private set;
		}

		public TokenType StartToken {
			get;
			private set;
		}

		public TokenType EndToken {
			get;
			private set;
		}

		public TokenRuleState NewState {
			get;
			private set;
		}
	}

	public class TokenRules : Dictionary<TokenType, TokenRule> {

		public TokenRules (Environment env)
		{
			string block_suffix_re = "";

			Add (TokenType.TOKEN_COMMENT_BEGIN,
					new TokenRule (String.Format (@"(.*?)((?:\-{0}\s*|{1}){2})", env.CommentStartString, env.CommentEndString, block_suffix_re),
					TokenType.TOKEN_COMMENT_BEGIN, TokenType.TOKEN_COMMENT_END, TokenRuleState.Pop));

			Add (TokenType.TOKEN_BLOCK_BEGIN,
					new TokenRule (String.Format (@"(?:\-{0}\s*|{1}){2}", env.BlockStartString, env.BlockEndString, block_suffix_re),
							TokenType.TOKEN_BLOCK_BEGIN, TokenType.TOKEN_BLOCK_END, TokenRuleState.Pop));

			Add (TokenType.TOKEN_VARIABLE_BEGIN,
					new TokenRule (String.Format (@"\-{0}\s*|{1}", env.VariableStartString, env.VariableEndString),
							TokenType.TOKEN_VARIABLE_BEGIN, TokenType.TOKEN_VARIABLE_END, TokenRuleState.Pop));

		}

	}

}


