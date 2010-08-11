
using System;
using System.IO;
using System.Text;

using System.Collections.Generic;


namespace Mango.Templates {

	public class MingeParser {

		private Application application;
		private MingeEnvironment environment;

		private Page current_page;

		public MingeParser (MingeEnvironment environment, Application application)
		{
			this.application = application;
			this.environment = environment;
		}

		public Page ParsePage (string name, TextReader reader)
		{
			Console.WriteLine ("parsing page:  {0}", name);
			MingeTokenizer tk = new MingeTokenizer (environment, reader);

			current_page = application.CreatePage (name);

			Token tok = null;
			StringBuilder data = new StringBuilder ();

			while (true) {
				tok = tk.GetNextToken ();

				switch (tok.Type) {
				case TokenType.TOKEN_VARIABLE_BEGIN:
					FlushData (data);
					ParseVariable (tk);
					break;
				case TokenType.TOKEN_COMMENT_BEGIN:
					FlushData (data);
					ParseComment (tk);
					break;
				case TokenType.TOKEN_BLOCK_BEGIN:
					FlushData (data);
					ParseControlBlock (tk);
					break;
				case TokenType.TOKEN_EOF:
					FlushData (data);
					current_page.Save ();
					return current_page;
				default:
					data.Append (tok.Value);
					break;
				}
			}

			return null;
		}

		public void FlushData (StringBuilder data)
		{
			current_page.AddData (data.ToString ());
//			Console.WriteLine ("DATA: {0}", data);
			data.Length = 0;
		}

		public void ParseVariable (MingeTokenizer tk)
		{
			Expression exp = ParseExpression (tk, TokenType.TOKEN_BLOCK_END);

			if (tk.Current.Type != TokenType.TOKEN_VARIABLE_END)
				RaiseFailure (tk, String.Format ("Invalid variable statement found, '{0}' token found when a {1} was expected.",
						tk.Current.Value, environment.VariableEndString));
						
			current_page.EmitSinglePrint (exp);

		}

		public void ParseComment (MingeTokenizer tk)
		{
			Token tok;
			StringBuilder builder = new StringBuilder ();
			do {
				tok = tk.GetNextToken ();

				if (tok.Type == TokenType.TOKEN_COMMENT_END) {
					return;
				}
					
				builder.Append (tok.Value);

			} while (tok.Type != TokenType.TOKEN_EOF);

			// FAIL
		}

		public void ParseControlBlock (MingeTokenizer tk)
		{
			Token tok;
			StringBuilder builder = new StringBuilder ();

			ParseStatement (tk);
			tok = tk.Current;

			do {
				if (tok.Type == TokenType.TOKEN_BLOCK_END) {
					return;
				}
					
				builder.Append (tok.Value);

				tok = tk.GetNextToken ();

			} while (tok.Type != TokenType.TOKEN_EOF);
		}

		public void ParseStatement (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_NAME) {
				// fail
				Console.WriteLine ("INVALID BLOCK TOKEN TYPE");
			}

			switch (tok.Value) {
			case "print":
				ParsePrint (tk);
				break;
			case "for":
				ParseForLoop (tk);
				break;
			case "endfor":
				ParseEndForLoop (tk);
				break;
			case "if":
				ParseIf (tk);
				break;
			case "else":
				ParseElse (tk);
				break;
			case "endif":
				ParseEndIf (tk);
				break;
			case "block":
				ParseBlock (tk);
				break;
			case "endblock":
				ParseEndBlock (tk);
				break;
			case "extends":
				ParseExtends (tk);
				break;
			case "macro":
				ParseMacro (tk);
				break;
			case "endmacro":
				ParseEndMacro (tk);
				break;			
			case "set":
				ParseSet (tk);
				break;
			case "include":
			case "from":
			case "import":
			case "call":
			case "filter":
			default:
				throw new InvalidOperationException (String.Format ("Unsupported operation {0}", tok.Value));
				break;
			}
		}

		public void ParsePrint (MingeTokenizer tk)
		{
			Token tok;
			bool first = true;
			List<Expression> expressions = new List<Expression> ();

			do {
				Expression exp = ParseExpression (tk, TokenType.TOKEN_BLOCK_END);

				if (exp != null)
					expressions.Add (exp);
				
				tok = tk.Current;
				if (tok.Type == TokenType.TOKEN_BLOCK_END)
					break;

				if (!first && tok.Type != TokenType.TOKEN_COMMA)
					RaiseFailure (tk, "Invalid token found in print statement '{0}'");

			} while (tok.Type != TokenType.TOKEN_EOF);

			if (tok.Type == TokenType.TOKEN_EOF)
				RaiseFailure (tk, "Unexpected end of file.");

			current_page.EmitPrint (expressions);
		}

		public void ParseSet (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);
			if (tok.Type != TokenType.TOKEN_NAME)
				RaiseFailure (tk, String.Format ("Invalid token found in set statement, expected a name got a {0}", tok.Value));

			NamedTarget target = new NamedTarget (tok.Value);

			tok = NextNonWhiteSpaceToken (tk);
			if (tok.Type != TokenType.TOKEN_ASSIGN)
				RaiseFailure (tk, String.Format ("Invalid token found in set statement, expected an = got a {0}", tok.Value));
			
			Expression expression = ParseExpression (tk, TokenType.TOKEN_BLOCK_END);

			current_page.EmitSet (target, expression);
		}

		public void ParseIf (MingeTokenizer tk)
		{
			Expression expression = ParseExpression (tk, TokenType.TOKEN_BLOCK_END);

			current_page.EmitIf (expression);
		}

		public void ParseElse (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);
			if (tok.Type != TokenType.TOKEN_BLOCK_END)
				RaiseFailure (tk, String.Format ("Invalid '{0}' token found in else statement.", tok.Value));

			current_page.EmitElse ();
		}

		public void ParseEndIf (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);
			while (tok.Type != TokenType.TOKEN_BLOCK_END) {

				tok = NextNonWhiteSpaceToken (tk);
			}

			current_page.EmitEndIf ();
		}

		public void ParseBlock (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_NAME)
				RaiseFailure (tk, String.Format ("Invalid '{0}' token found in block statement.", tok.Value));

			current_page.BeginBlock (tok.Value);

			tok = NextNonWhiteSpaceToken (tk);
			if (tok.Type != TokenType.TOKEN_BLOCK_END)
				RaiseFailure (tk, String.Format ("Invalid '{0}' token found in block statement.", tok.Value));
		}

		public void ParseEndBlock (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			string name = null;
			if (tok.Type == TokenType.TOKEN_NAME) {
				name = tok.Value;
				tok = NextNonWhiteSpaceToken (tk);
			}

			// Name matching is optional, we pass null if no name is supplied
			current_page.EndBlock (name);

			if (tok.Type != TokenType.TOKEN_BLOCK_END)
				RaiseFailure (tk, String.Format ("Invalid '{0}' token found in endblock statement.", tok.Value));
		}

		public void ParseExtends (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_QUOTED_STRING)
				RaiseFailure (tk, String.Format ("Invalid '{0}' token found in extends statement.", tok.Value));

			Console.WriteLine ("extending:  {0}", tok.Value);
			current_page.EmitExtends (ValueOfQuotedString (tok.Value));

			tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_BLOCK_END)
				RaiseFailure (tk, String.Format ("Invalid '{0}' token found in extends statement.", tok.Value));
		}

		public void ParseMacro (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_NAME)
				RaiseFailure (tk, String.Format ("Invalid macro definition, expected name got a '{0}'.", tok.Value));

			string name = tok.Value;
			List<ArgumentDefinition> args = null;

			tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type == TokenType.TOKEN_LPAREN) {
				args = ParseArgumentDefinitions (tk);
				tok = NextNonWhiteSpaceToken (tk);
			}

			current_page.BeginMacro (name, args);

			if (tok.Type != TokenType.TOKEN_BLOCK_END)
				RaiseFailure (tk, String.Format ("Invalid macro definition, expect block end got a '{0}'.", tok.Value));
		}

		public void ParseEndMacro (MingeTokenizer tk)
		{
			string name = null;
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type == TokenType.TOKEN_NAME) {
				name = tok.Value;
				tok = NextNonWhiteSpaceToken (tk);
			}

			if (tok.Type != TokenType.TOKEN_BLOCK_END)
				RaiseFailure (tk, String.Format ("Invalid endmacro definition, expected a block end got a '{0}'.", tok.Value));
			
			current_page.EndMacro (name);
		}

		public Expression ParseExpression (MingeTokenizer tk, TokenType end_token_type, bool allow_conditionals=true)
		{
			Value target_value = ParseRValue (tk);

			Expression expression = null;

			Token tok = NextNonWhiteSpaceToken (tk);
			if (tok.Type == TokenType.TOKEN_DOT) {
				tok = NextNonWhiteSpaceToken (tk);
				if (tok.Type != TokenType.TOKEN_NAME)
					RaiseFailure (tk, String.Format ("Invalid expression, token '{0}' found where a name was expected.", tok.Value));
				expression = new Expression (new PropertyAccessValue (target_value, tok.Value));
				NextNonWhiteSpaceToken (tk);
			} else if (tok.Type == TokenType.TOKEN_LBRACKET) {
				string prop_name = ParseSubscript (tk);
				expression = new Expression (new PropertyAccessValue (target_value, prop_name));
				NextNonWhiteSpaceToken (tk);
			} else if (tok.Type == TokenType.TOKEN_LPAREN) {

				VariableValue target = target_value as VariableValue;

				if (target == null)
					RaiseFailure (tk, String.Format ("Invalid invoke expression, expected a name got a {0}", target_value));

				List<Expression> args = ParseArguments (tk);

				if (tk.Current.Type != TokenType.TOKEN_RPAREN)
					RaiseFailure (tk, String.Format ("Invalid invoke expression, token '{0}' where a ) was expected.", tk.Current.Value));

				expression = new Expression (new InvokeValue (target.Name.Name, args));
				NextNonWhiteSpaceToken (tk);
			} else
				expression = new Expression (target_value);

			while (tok.Type != end_token_type && tok.Type != TokenType.TOKEN_EOF) {

				if (tok.Type == TokenType.TOKEN_PIPE) {
					Filter filter = ParseFilter (tk);
					expression.AddFilter (filter);
				} else if (allow_conditionals && IsConditionalToken (tok)) {
					CompareOperator compare = CompareOperatorFromToken (tok);
					// expression = new ConditionalExpression (expression, compare, ParseExpression (tk, end_token_type));
				} else 
					break;

				tok = tk.Current;
			}

			if (tok.Type == TokenType.TOKEN_EOF)
				RaiseFailure (tk, "Unexpected eof of file found while parsing expression.");

			return expression;
		}

		public bool IsConditionalToken (Token tok)
		{
			return CompareOperatorFromToken (tok) != CompareOperator.Invalid;
		}

		public CompareOperator CompareOperatorFromToken (Token tok)
		{
			CompareOperator res = CompareOperator.Invalid;

			switch (tok.Type) {
			case TokenType.TOKEN_NAME:
				if ("is" == tok.Value)
					res = CompareOperator.Is;
				break;
			case TokenType.TOKEN_EQ:
				res = CompareOperator.Equal;
				break;
			case TokenType.TOKEN_NE:
				res = CompareOperator.NotEqual;
				break;
			case TokenType.TOKEN_GTEQ:
				res = CompareOperator.GreaterThanOrEqual;
				break;
			case TokenType.TOKEN_LTEQ:
				res = CompareOperator.LessThanOrEqual;
				break;
			}

			return res;
		}

		public void ParseForLoop (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_NAME)
				RaiseFailure (tk, String.Format ("Invalid for loop, expected a name got token '{0}'", tok.Value));

			string variable_name = tok.Value;

			Expect (tk, TokenType.TOKEN_NAME, "in", "Invalid for loop, no 'in' statement found. '{0}' found instead.");

			Expression iter = ParseExpression (tk, TokenType.TOKEN_BLOCK_END);

			current_page.BeginForLoop (variable_name, iter);
		}

		public void ParseEndForLoop (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);
			while (tok.Type != TokenType.TOKEN_BLOCK_END) {

				tok = NextNonWhiteSpaceToken (tk);
			}

			current_page.EndForLoop ();
		}

		public string ParseSubscript (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_QUOTED_STRING)
				RaiseFailure (tk, "Invalid subscript expression, token '{0}' found where a quoted string was expected.");

			string value = ValueOfQuotedString (tok.Value);

			tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_RBRACKET)
				RaiseFailure (tk, "Invalid subscript expression, token '{0}' found where a ] was expected.");

			return value;
		}

		public Filter ParseFilter (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != TokenType.TOKEN_NAME)
				RaiseFailure (tk, String.Format ("Invalid filter expression, token '{0}' found where a name was expected.", tok.Value));

			string name = tok.Value;
			List<Expression> args = new List<Expression> ();

			tok = NextNonWhiteSpaceToken (tk);
			if (tok.Type == TokenType.TOKEN_LPAREN) {
				args = ParseArguments (tk);

				if (tk.Current.Type != TokenType.TOKEN_RPAREN)
					RaiseFailure (tk, String.Format ("Invalid filter expression, token '{0}' where a ) was expected.", tk.Current.Value));

				// Advance pass the RPAREN
				NextNonWhiteSpaceToken (tk);
			}

			return new Filter (name, args);
		}

		public List<Expression> ParseArguments (MingeTokenizer tk)
		{
			List<Expression> expressions = new List<Expression> ();

			Token tok = null;
			do {
				Expression expression = ParseExpression (tk, TokenType.TOKEN_COMMA);
				expressions.Add (expression);

				tok = tk.Current;
				if (tok.Type == TokenType.TOKEN_RPAREN)
					break;

				if (tok.Type != TokenType.TOKEN_COMMA)
					RaiseFailure (tk, String.Format ("Invalid argument list, expected comma got a {0}", tk.Current.Value));
			} while (tok.Type != TokenType.TOKEN_EOF);

			if (tk.Current.Type == TokenType.TOKEN_EOF)
				RaiseFailure (tk, String.Format ("Unexpected end of file."));

			return expressions;
		}

		public List<ArgumentDefinition> ParseArgumentDefinitions (MingeTokenizer tk)
		{
			Token tok = NextNonWhiteSpaceToken (tk);
			List<ArgumentDefinition> args = new List<ArgumentDefinition> ();

			do {
				if (tok.Type == TokenType.TOKEN_RPAREN)
					break;

				if (tok.Type != TokenType.TOKEN_NAME)
					RaiseFailure (tk, String.Format ("Invalid argument definition, expected a name got a '{0}'", tok.Value));

				string name = tok.Value;
				ConstantValue default_value = null;

				tok = NextNonWhiteSpaceToken (tk);

				if (tok.Type == TokenType.TOKEN_ASSIGN) {
					default_value = ParseConstantValue (tk);
					tok = NextNonWhiteSpaceToken (tk);
				}

				args.Add (new ArgumentDefinition (name, default_value));

				if (tok.Type == TokenType.TOKEN_RPAREN)
					break;
				
				if (tok.Type != TokenType.TOKEN_COMMA)
					RaiseFailure (tk, String.Format ("Invalid argument list, expected comma got a {0}", tk.Current.Type));

				tok = NextNonWhiteSpaceToken (tk);
			} while (tok.Type != TokenType.TOKEN_EOF);

			if (tk.Current.Type == TokenType.TOKEN_EOF)
				RaiseFailure (tk, String.Format ("Unexpected end of file."));

			return args;
		}

		public Value ParseRValue (MingeTokenizer tk)
		{
			Value value = null;

			Token tok = NextNonWhiteSpaceToken (tk);
			switch (tok.Type) {
			case TokenType.TOKEN_NAME:
				value = new VariableValue (new NamedTarget (tok.Value));
				break;
			case TokenType.TOKEN_QUOTED_STRING:
				value = new ConstantStringValue (ValueOfQuotedString (tok.Value));
				break;
			case TokenType.TOKEN_DOUBLE:
				value = new ConstantDoubleValue ((double) tok.TokenizedValue);
				break;
			case TokenType.TOKEN_INTEGER:
				value = new ConstantIntValue ((int) tok.TokenizedValue);
				break;
			default:
				RaiseFailure (tk, String.Format ("Unexpected token '{0}', in rvalue", tok.Value));
				break;
			}

			return value;
		}

		public ConstantValue ParseConstantValue (MingeTokenizer tk)
		{
			ConstantValue value = null;

			Token tok = NextNonWhiteSpaceToken (tk);
			switch (tok.Type) {
			case TokenType.TOKEN_QUOTED_STRING:
				value = new ConstantStringValue (ValueOfQuotedString (tok.Value));
				break;
			default:
				RaiseFailure (tk, String.Format ("Unexpected token '{0}', in constant value", tok.Value));
				break;
			}

			return value;
		}

		public Target ParseAssignmentTarget (MingeTokenizer tk, bool with_tuple=true, bool name_only=false, string extra_end_rules=null)
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type == TokenType.TOKEN_NAME)
				return new NamedTarget (tok.Value);

			return null;
		}

		public Token NextNonWhiteSpaceToken (MingeTokenizer tk)
		{
			Token tok;

			do {
				tok = tk.GetNextToken ();
			} while (tok.Type == TokenType.TOKEN_WHITESPACE);

			return tok;
		}

		private static string ValueOfQuotedString (string str)
		{
			string res = str.Substring (1, str.Length - 2);
			return res;
		}

		private void Expect (MingeTokenizer tk, TokenType type, string value, string error="Expected symbol {0} not found.")
		{
			Token tok = NextNonWhiteSpaceToken (tk);

			if (tok.Type != type || tok.Value != value)
				RaiseFailure (tk, String.Format (error, value));
		}
			
		private void RaiseFailure (MingeTokenizer tk, string error)
		{
			throw new Exception (String.Format ("({0}:{1}) FAILURE: {2}", tk.Line, tk.Column, error));
		}
	}
}

