//
// Based on http_parser.java: http://github.com/a2800276/http-parser.java
// which is based on http_parser: http://github.com/ry/http-parser
//
//
// Copyright 2009,2010 Ryan Dahl <ry@tinyclouds.org>
// Copyright (C) 2010 Tim Becker 
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//

using System;
using System.IO;

using Manos.IO;

namespace Manos.Http {

	public class HttpParser {

		private ParserType type;

		private int http_major;
		private int http_minor;
		private int status_code;
		private HttpMethod method;

		/* true  = Upgrade header was present and the parser has exited because of that.
		 * false = No upgrade header present.
		 * Should be checked when http_parser_execute() returns in addition to
		 * error checking.
		 */
		protected bool upgrade; 

		public HttpParser () : this (ParserType.HTTP_BOTH)
		{
		}

		public HttpParser (ParserType type)
		{
			this.type = type;

			switch (type) {
			case ParserType.HTTP_REQUEST:
				this.state = State.start_req;
				break;
			case ParserType.HTTP_RESPONSE:
				this.state = State.start_res;
				break;
			case ParserType.HTTP_BOTH:
				this.state = State.start_res_or_res;
				break;
			default:
				throw new HttpException ("can't happen, invalid ParserType enum");
			}
		}

		public bool Strict {
			get { return strict; }
			set { strict = value; }
		}

		public int Major {
			get { return http_major; }
		}

		public int Minor {
			get { return http_minor; }
		}

		public int StatusCode {
			get { return status_code; }
		}

		public HttpMethod HttpMethod {
			get { return method; }
		}

		public bool Upgrade {
			get { return upgrade; }
		}

		public bool KeepAlive {
			get { return http_should_keep_alive (); }
		}
		
		//
		// The state machine 
		//

		State state;
		HState header_state;
		bool strict;

		int index; 
		int flags; // TODO	

		int nread;
		int content_length;


		/* 
		 * technically we could combine all of these (except for url_mark) into one
		 * variable, saving stack space, but it seems more clear to have them
		 * separated. 
		 */
		int header_field_mark = -1;
		int header_value_mark = -1;
		int fragment_mark = -1;
		int query_string_mark = -1;
		int path_mark = -1;
		int url_mark = -1;
	
	
		static void p (object o)
		{
			Console.WriteLine (o);
		}


		/** Execute the parser with the currently available data contained in
		 * the buffer. The buffers position() and limit() need to be set
		 * correctly (obviously) and a will be updated approriately when the
		 * method returns to reflect the consumed data.
		 */
		public void Execute (ParserSettings settings, ByteBuffer data)
		{
			int p     = (int) data.Position;
			int p_err = p; // this is used for pretty printing errors.

			// In case the headers don't provide information about the content
			// length, `execute` needs to be called with an empty buffer to
			// indicate that all the data has been send be the client/server,
			// else there is no way of knowing the message is complete. 
			int len = (int) (data.Length - data.Position);
			if (0 == len) {
				if (State.body_identity_eof == state)
					settings.RaiseOnMessageComplete(this);
			}

		
			// in case the _previous_ call to the parser only has data to get to
			// the middle of certain fields, we need to update marks to point at
			// the beginning of the current buffer.
			switch (state) {
			case State.header_field:
				header_field_mark = p;
				break;
			case State.header_value:
				header_value_mark = p;
				break;
			case State.req_fragment:
				fragment_mark = p;
				url_mark = p;
				break;
			case State.req_query_string:
				query_string_mark = p;
				url_mark = p;
				break;
			case State.req_path:
				path_mark = p;
				// JACKSON ADDED, I assume java can fall through?
				url_mark = p;
				break;
			case State.req_host:
			case State.req_schema:
			case State.req_schema_slash:
			case State.req_schema_slash_slash:
			case State.req_port:
			case State.req_query_string_start:
			case State.req_fragment_start:
				url_mark = p;
				break;
			}

			// this is where the work gets done, traverse the available data...
			while (data.Position != data.Length) {

				p = (int) data.Position;
				int  pe = (int) data.Length;
      
				byte ch      = data.ReadByte ();  // the current character to process.
				int c       = -1;          // utility variably used for up- and downcasing etc.
				int to_read =  0;          // used to keep track of how much of body, etc. is left to read

				if (parsing_header (state)) {
					++nread;
					if (nread > HTTP_MAX_HEADER_SIZE) {
						settings.RaiseOnError (this, "possible buffer overflow", data, p_err);
					}
				}

				switch (state) {
					/*
					 * this state is used after a 'Connection: close' message
					 * the parser will error out if it reads another message
					 */
				case State.dead:
					settings.RaiseOnError (this, "Connection already closed", data, p_err);
					// JACKSON: Added this break
					break;

				case State.start_res_or_res:
					if (CR == ch || LF == ch){
						break;
					}
					flags = 0;
					content_length = -1;

					settings.RaiseOnMessageBegin (this);
          
					if (H == ch) 
						state = State.res_or_resp_H;
					else {
						type   = ParserType.HTTP_REQUEST;  
						method = start_req_method_assign (ch);
						if (HttpMethod.ERROR == method)
							settings.RaiseOnError (this, "invalid method", data, p_err);
						index  = 1;
						state  = State.req_method;
					}
					break;

				case State.res_or_resp_H:
					if (T == ch) {
						type  = ParserType.HTTP_RESPONSE;
						state = State.res_HT;
					} else {
						if (E != ch)
							settings.RaiseOnError (this, "not E", data, p_err);

						type   = ParserType.HTTP_REQUEST;
						method = HttpMethod.HTTP_HEAD;
						index  = 2;
						state  = State.req_method;
					}
					break;

				case State.start_res:
					flags = 0;
					content_length = -1;

					settings.RaiseOnMessageBegin (this);
					
					switch (ch) {
					case H:
						state = State.res_H;
						break;
					case CR:
					case LF:
						break;
					default:
						settings.RaiseOnError (this, "Not H or CR/LF", data, p_err);
						break;
					}
					break;


				case State.res_H:
					if (strict && T != ch)
						settings.RaiseOnError (this, "Not T", data, p_err);
					state = State.res_HT;
					break;
				case State.res_HT:
					if (strict && T != ch)
						settings.RaiseOnError (this, "Not T2", data, p_err);
					state = State.res_HTT;
					break;
				case State.res_HTT:
					if (strict && P != ch)
						settings.RaiseOnError (this, "Not P", data, p_err);
					state = State.res_HTTP;
					break;
				case State.res_HTTP:
					if (strict && SLASH != ch)
						settings.RaiseOnError (this, "Not '/'", data, p_err);
					state = State.res_first_http_major;
					break;

					
					
				case State.res_first_http_major:
					if (!isDigit (ch))
						settings.RaiseOnError (this, "Not a digit", data, p_err);
					http_major = (int) ch - 0x30;
					state = State.res_http_major;
					break;

					/* major HTTP version or dot */
				case State.res_http_major:
					if (DOT == ch) {
						state = State.res_first_http_minor;
						break;
					}
					if (!isDigit (ch))
						settings.RaiseOnError(this, "Not a digit", data, p_err);
					http_major *= 10;
					http_major += (ch - 0x30);

					if (http_major > 999)
						settings.RaiseOnError(this, "invalid http major version: " + http_major, data, p_err);
					break;
          
					/* first digit of minor HTTP version */
				case State.res_first_http_minor:
					if (!isDigit (ch))
						settings.RaiseOnError (this, "Not a digit", data, p_err);
					http_minor = (int)ch - 0x30;
					state = State.res_http_minor;
					break;

					/* minor HTTP version or end of request line */
				case State.res_http_minor:
					if (SPACE == ch) {
						state = State.res_first_status_code;
						break;
					}
					if (!isDigit (ch))
						settings.RaiseOnError(this, "Not a digit", data, p_err);
					http_minor *= 10;
					http_minor += (ch - 0x30);

					if (http_minor > 999)
						settings.RaiseOnError(this, "invalid http minor version: " + http_minor, data, p_err);
					break;



				case State.res_first_status_code:
					if (!isDigit (ch)) {
						if (SPACE == ch)
							break;
						settings.RaiseOnError (this, "Not a digit (status code)", data, p_err);
					}
					status_code = (int)ch - 0x30;
					state = State.res_status_code;
					break;

				case State.res_status_code:
					if (!isDigit (ch)) {
						switch (ch) {
						case SPACE:
							state = State.res_status;
							break;
						case CR:
							state = State.res_line_almost_done;
							break;
						case LF:
							state = State.header_field_start;
							break;
						default:
							settings.RaiseOnError(this, "not a valid status code", data, p_err);
							break;
						}
						break;
					}
					status_code *= 10;
					status_code += (int)ch - 0x30;
					if (status_code > 999)
						settings.RaiseOnError(this, "ridiculous status code:"+status_code, data, p_err);
					break;

				case State.res_status:
					/* the human readable status. e.g. "NOT FOUND"
					 * we are not humans so just ignore this
					 * we are not men, we are devo. */

					if (CR == ch) {
						state = State.res_line_almost_done;
						break;
					}
					if (LF == ch) { 
						state = State.header_field_start;
						break;
					}
					break;

				case State.res_line_almost_done:
					if (strict && LF != ch)
						settings.RaiseOnError (this, "not LF", data, p_err);
					state = State.header_field_start;
					break;



				case State.start_req:
					if (CR==ch || LF == LF)
						break;
					flags = 0;
					content_length = -1;
					settings.RaiseOnMessageBegin (this);
					method = start_req_method_assign (ch);
					if (HttpMethod.ERROR == method)
						settings.RaiseOnError (this, "invalid method", data, p_err);

					index  = 1;
					state  = State.req_method;
					break;
				


				case State.req_method:
					if (0 == ch)
						settings.RaiseOnError( this, "NULL in method", data, p_err);
          
					byte [] arr = HttpMethodBytes.GetBytes (method);

					if (SPACE == ch && index == arr.Length)
						state = State.req_spaces_before_url;
					else if (arr[index] == ch) {
						// wuhu!	
					} else if (HttpMethod.HTTP_CONNECT == method) {
						if (1 == index && H == ch) {
							method = HttpMethod.HTTP_CHECKOUT;
						} else if (2 == index && P == ch) {
							method = HttpMethod.HTTP_COPY;
						}
					} else if (HttpMethod.HTTP_MKCOL == method) {
						if        (1 == index && O == ch) {
							method = HttpMethod.HTTP_MOVE;
						} else if (1 == index && E == ch) {
							method = HttpMethod.HTTP_MERGE;
						} else if (2 == index && A == ch) {
							method = HttpMethod.HTTP_MKACTIVITY;
						}
					} else if (1 == index && HttpMethod.HTTP_POST == method && R == ch) {
						method = HttpMethod.HTTP_PROPFIND;
					} else if (1 == index && HttpMethod.HTTP_POST == method && U == ch) {
						method = HttpMethod.HTTP_PUT;
					} else if (4 == index && HttpMethod.HTTP_PROPFIND == method && P == ch) {
						method = HttpMethod.HTTP_PROPPATCH;
					} else {
						settings.RaiseOnError (this, "Invalid HTTP method", data, p_err);
					}

					++index;
					break;
      


				/*__________________URL__________________*/
				case State.req_spaces_before_url:
					if (SPACE == ch)
						break;
					if (SLASH == ch) {
						url_mark  = p;
						path_mark = p;
						state = State.req_path;
						break;
					}
					if (isAtoZ (ch)) {
						url_mark = p;
						state = State.req_schema;
						break;
					}
					settings.RaiseOnError (this, "Invalid something", data, p_err);
					break;

				case State.req_schema:
					if (isAtoZ (ch))
						break;
					if (COLON == ch) {
						state = State.req_schema_slash;
						break;
					} else if (DOT == ch) {
						state = State.req_host;
						break;
					}
					settings.RaiseOnError (this, "invalid char in schema: "+ch, data, p_err);
					break;

				case State.req_schema_slash:
					if (strict && SLASH != ch)
						settings.RaiseOnError (this, "invalid char in schema, not /", data, p_err);
					state = State.req_schema_slash_slash;
					break;

				case State.req_schema_slash_slash:
					if (strict && SLASH != ch)
						settings.RaiseOnError(this, "invalid char in schema, not /", data, p_err);
					state = State.req_host;
					break;
        
				case State.req_host:
					if (isAtoZ (ch))
						break;
					if (isDigit (ch) || DOT == ch || DASH == ch)
						break;
					switch (ch) {
					case COLON:
						state = State.req_port;
						break;
					case SLASH:
						path_mark = p;
						break;
					case SPACE:
						/* The request line looks like:
						 *   "GET http://foo.bar.com HTTP/1.1"	
						 * That is, there is no path.	
						 */
						settings.RaiseOnUrl (this, data, url_mark, p-url_mark);
						url_mark = -1;
						state = State.req_http_start;
						break;
					default:
						settings.RaiseOnError(this, "host error in method line", data, p_err);
						break;
					}
					break;

				case State.req_port:
					if (isDigit (ch))
						break;
					switch (ch) {
					case SLASH:
						path_mark = p; 
						state = State.req_path;
						break;
					case SPACE:
						/* The request line looks like:
						 *   "GET http://foo.bar.com:1234 HTTP/1.1"
						 * That is, there is no path.
						 */
						settings.RaiseOnUrl (this,data,url_mark,p-url_mark);
						url_mark = -1;
						state = State.req_http_start;
						break;
					default:
						settings.RaiseOnError (this, "invalid port", data, p_err);
						break;
					}
					break;
      
				case State.req_path:
					if (usual (ch))
						break;
					switch (ch) {
					case SPACE:
						settings.RaiseOnUrl (this,data,url_mark, p-url_mark);
						url_mark = -1;

						settings.RaiseOnPath(this,data,path_mark, p-path_mark);
						path_mark = -1;

						state = State.req_http_start;
						break;

					case CR:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1;
              
						settings.RaiseOnPath(this,data,path_mark, p-path_mark);
						path_mark = -1;
              
						http_minor = 9;
						state = State.res_line_almost_done;
						break;

					case LF:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1;
					
						settings.RaiseOnPath(this,data,path_mark, p-path_mark);
						path_mark = -1;
              
						http_minor = 9;
						state = State.header_field_start;
						break;

					case QMARK:
						settings.RaiseOnPath(this,data,path_mark, p-path_mark);
						path_mark = -1;
              
						state = State.req_query_string_start;
						break;
            
					case HASH:
						settings.RaiseOnPath(this,data,path_mark, p-path_mark);
						path_mark = -1;
              
						state = State.req_fragment_start;
						break;
            
					default:
						settings.RaiseOnError(this, "unexpected char in path", data, p_err);
						break;
					}
					break;
      
				case State.req_query_string_start:
					if (usual(ch)) {
						query_string_mark = p;
						state = State.req_query_string;
						break;
					}

					switch (ch) {
					case QMARK: break;
					case SPACE: 
						settings.RaiseOnUrl(this, data, url_mark, p-url_mark);
						url_mark = -1;
						state = State.req_http_start;
						break;
					case CR:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1; 
						http_minor = 9;
						state = State.res_line_almost_done;
						break;
					case LF:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1;
						http_minor = 9;
						state = State.header_field_start;
						break;
					case HASH:
						state = State.req_fragment_start;
						break;
					default:
						settings.RaiseOnError(this, "unexpected char in path", data, p_err);
						break;
					}
					break;
        
				case State.req_query_string:
					if (usual(ch)) {
						break;
					}

					switch (ch) {
					case QMARK: break; // allow extra '?' in query string
					case SPACE: 
						settings.RaiseOnUrl(this, data, url_mark, p-url_mark);
						url_mark = -1;

						settings.RaiseOnQueryString(this, data, query_string_mark, p-query_string_mark);
						query_string_mark = -1;

						state = State.req_http_start;
						break;
					case CR:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1; 

						settings.RaiseOnQueryString(this, data, query_string_mark, p-query_string_mark);
						query_string_mark = -1;
              
						http_minor = 9;
						state = State.res_line_almost_done;
						break;
					case LF:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1;

						settings.RaiseOnQueryString(this, data, query_string_mark, p-query_string_mark);
						query_string_mark = -1;
						http_minor = 9;

						state = State.header_field_start;
						break;
					case HASH:
						settings.RaiseOnQueryString(this, data, query_string_mark, p-query_string_mark);
						query_string_mark = -1;
              
						state = State.req_fragment_start;
						break;
					default:
						settings.RaiseOnError(this, "unexpected char in path", data, p_err);
						break;
					}
					break;

				case State.req_fragment_start:
					if (usual(ch)) {
						fragment_mark = p;
						state = State.req_fragment;
						break;
					}

					switch (ch) {
					case SPACE: 
						settings.RaiseOnUrl(this, data, url_mark, p-url_mark);
						url_mark = -1;
     
						state = State.req_http_start;
						break;
					case CR:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1; 

						http_minor = 9;
						state = State.res_line_almost_done;
						break;
					case LF:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1;
              
						http_minor = 9;
						state = State.header_field_start;
						break;
					case QMARK:
						fragment_mark = p;
						state = State.req_fragment;
						break;
					case HASH:
						break;
					default:
						settings.RaiseOnError(this, "unexpected char in path", data, p_err);
						break;
					}
					break;

				case State.req_fragment:
					if (usual(ch)) {
						break;
					}

					switch (ch) {
					case SPACE: 
						settings.RaiseOnUrl(this, data, url_mark, p-url_mark);
						url_mark = -1;
          
						settings.RaiseOnFragment(this, data, fragment_mark, p-fragment_mark);
						fragment_mark = -1;
              
						state = State.req_http_start;
						break;
					case CR:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1; 
              
						settings.RaiseOnFragment(this, data, query_string_mark, p-query_string_mark);
						fragment_mark = -1;
              
						http_minor = 9;
						state = State.res_line_almost_done;
						break;
					case LF:
						settings.RaiseOnUrl(this,data,url_mark, p-url_mark);
						url_mark = -1;
              
						settings.RaiseOnFragment(this, data, query_string_mark, p-query_string_mark);
						fragment_mark = -1;
              
						http_minor = 9;
						state = State.header_field_start;
						break;
					case QMARK:
					case HASH:
						break;
					default:
						settings.RaiseOnError(this, "unexpected char in path", data, p_err);
						break;
					}
					break;
				/******************* URL *******************/



				/******************* HTTP 1.1 *******************/
				case State.req_http_start:
					switch (ch) {
					case H:
						state = State.req_http_H;
						break;
					case SPACE:
						break;
					default:
						settings.RaiseOnError(this, "error in req_http_H", data, p_err);
						break;
					}
					break;

				case State.req_http_H:
					if (strict && T != ch)
						settings.RaiseOnError(this, "unexpected char", data, p_err);
					state = State.req_http_HT;
					break;

				case State.req_http_HT:
					if (strict && T != ch)
						settings.RaiseOnError(this, "unexpected char", data, p_err);
					state = State.req_http_HTT;
					break;

				case State.req_http_HTT:
					if (strict && P != ch)
						settings.RaiseOnError(this, "unexpected char", data, p_err);
					state = State.req_http_HTTP;
					break;

				case State.req_http_HTTP:
					if (strict && SLASH != ch)
						settings.RaiseOnError(this, "unexpected char", data, p_err);
					state = State.req_first_http_major;
					break;

				/* first digit of major HTTP version */
				case State.req_first_http_major:
					if (!isDigit(ch))
						settings.RaiseOnError(this, "non digit in http major", data, p_err);
					http_major = (int)ch - 0x30;
					state = State.req_http_major;
					break;

				/* major HTTP version or dot */
				case State.req_http_major:
					if (DOT == ch) {
						state = State.req_first_http_minor;
						break;
					}

					if (!isDigit(ch))
						settings.RaiseOnError(this, "non digit in http major", data, p_err);

					http_major *= 10;
					http_major += (int)ch - 0x30;

					if (http_major > 999)
						settings.RaiseOnError(this, "ridiculous http major", data, p_err);
					break;
        
				/* first digit of minor HTTP version */
				case State.req_first_http_minor:
					if (!isDigit(ch))
						settings.RaiseOnError(this, "non digit in http minor", data, p_err);
					http_minor = (int)ch - 0x30;
					state = State.req_http_minor;
					break;

				case State.req_http_minor:
					if (ch == CR) {
						state = State.req_line_almost_done;
						break;
					}

					if (ch == LF) {
						state = State.header_field_start;
						break;
					}

				/* XXX allow spaces after digit? */

					if (!isDigit(ch))
						settings.RaiseOnError(this, "non digit in http minor", data, p_err);

					http_minor *= 10;
					http_minor += (int)ch - 0x30;

         
					if (http_minor > 999)
						settings.RaiseOnError(this, "ridiculous http minor", data, p_err);
   
					break;

				/* end of request line */
				case State.req_line_almost_done:
				{
					if (ch != LF)
						settings.RaiseOnError(this, "missing LF after request line", data, p_err);
					state = State.header_field_start;
					break;
				}

				/******************* HTTP 1.1 *******************/



			/******************* Header *******************/
				case State.header_field_start:
				{
					if (ch == CR) {
						state = State.headers_almost_done;
						break;
					}

					if (ch == LF) {
						/* they might be just sending \n instead of \r\n so this would be
						 * the second \n to denote the end of headers*/
						state = State.headers_almost_done;
						if (!headers_almost_done(ch, settings, data, p_err))
							return;
						break;
					}

					c = upper(ch);

					if (c == 0) {
						settings.RaiseOnError(this, "invalid char in header", data, p_err);
					};

					header_field_mark = p;

					index = 0;
					state = State.header_field;

					switch (c) {
					case C: 
						header_state = HState.C;
						break;

					case P:
						header_state = HState.matching_proxy_connection;
						break;

					case T:
						header_state = HState.matching_transfer_encoding;
						break;

					case U:
						header_state = HState.matching_upgrade;
						break;

					default:
						header_state = HState.general;
						break;
					}
					break;
				}



				case State.header_field:
				{
					c = UPCASE[ch];

					if (0 != c) {  
						switch (header_state) {
						case HState.general:
							break;

						case HState.C:
							index++;
							header_state = (O == c ? HState.CO : HState.general);
							break;

						case HState.CO:
							index++;
							header_state = (N == c ? HState.CON : HState.general);
							break;

						case HState.CON:
							index++;
							switch (c) {
							case N:
								header_state = HState.matching_connection;
								break;
							case T:
								header_state = HState.matching_content_length;
								break;
							default:
								header_state = HState.general;
								break;
							}
							break;

							/* connection */

						case HState.matching_connection:
							index++;
							if (index > CONNECTION.Length || c != CONNECTION[index]) {
								header_state = HState.general;
							} else if (index == CONNECTION.Length-1) {
								header_state = HState.connection;
							}
							break;

							/* proxy-connection */

						case HState.matching_proxy_connection:
							index++;
							if (index > PROXY_CONNECTION.Length || c != PROXY_CONNECTION[index]) {
								header_state = HState.general;
							} else if (index == PROXY_CONNECTION.Length-1) {
								header_state = HState.connection;
							}
							break;

							/* content-length */

						case HState.matching_content_length:
							index++;
							if (index > CONTENT_LENGTH.Length || c != CONTENT_LENGTH[index]) {
								header_state = HState.general;
							} else if (index == CONTENT_LENGTH.Length-1) {
								header_state = HState.content_length;
							}
							break;

							/* transfer-encoding */

						case HState.matching_transfer_encoding:
							index++;
							if (index > TRANSFER_ENCODING.Length || c != TRANSFER_ENCODING[index]) {
								header_state = HState.general;
							} else if (index == TRANSFER_ENCODING.Length-1) {
								header_state = HState.transfer_encoding;
							}
							break;

							/* upgrade */

						case HState.matching_upgrade:
							index++;
							if (index > UPGRADE.Length || c != UPGRADE[index]) {
								header_state = HState.general;
							} else if (index == UPGRADE.Length-1) {
								header_state = HState.upgrade;
							}
							break;

						case HState.connection:
						case HState.content_length:
						case HState.transfer_encoding:
						case HState. upgrade:
							if (SPACE != ch) header_state = HState.general;
							break;

						default:
							settings.RaiseOnError(this, "Unknown Header State", data, p_err);
							break;
						} // switch: header_state
						break;
					} // 0 != c

					if (COLON == ch)  {
						settings.RaiseOnHeaderField(this, data, header_field_mark, p-header_field_mark);
						header_field_mark = -1;

						state = State.header_value_start;
						break;
					}

					if (CR == ch) {
						state = State.header_almost_done;
						settings.RaiseOnHeaderField(this, data, header_field_mark, p-header_field_mark);
            
						header_field_mark = -1;
						break;
					}

					if (ch == LF) {
						settings.RaiseOnHeaderField(this, data, header_field_mark, p-header_field_mark);
						header_field_mark = -1;
            
						state = State.header_field_start;
						break;
					}

					settings.RaiseOnError(this, "invalid header field", data, p_err);
					break;
				}



				case State.header_value_start:
				{
					if (SPACE == ch) break;

					header_value_mark = p;

					state = State.header_value;
					index = 0;

					c = UPCASE[ch];

					if (c == 0) {
						if (CR == ch) {
							settings.RaiseOnHeaderValue(this, data, header_value_mark, p-header_value_mark);
							header_value_mark = -1;

							header_state = HState.general;
							state = State.header_almost_done;
							break;
						}

						if (LF == ch) {
							settings.RaiseOnHeaderValue(this, data, header_value_mark, p-header_value_mark);
							header_value_mark = -1;
              
							state = State.header_field_start;
							break;
						}

						header_state = HState.general;
						break;
					}

					switch (header_state) {
					case HState.upgrade:
						flags |= F_UPGRADE;
						header_state = HState.general;
						break;

					case HState.transfer_encoding:
						/* looking for 'Transfer-Encoding: chunked' */
						if (C == c) {
							header_state = HState.matching_transfer_encoding_chunked;
						} else {
							header_state = HState.general;
						}
						break;

					case HState.content_length:
						if (!isDigit(ch)) {
							settings.RaiseOnError(this, "Content-Length not numeric", data, p_err);
						} 
						content_length = (int)ch - 0x30;
						break;

					case HState.connection:
						/* looking for 'Connection: keep-alive' */
						if (K == c) {
							header_state = HState.matching_connection_keep_alive;
							/* looking for 'Connection: close' */
						} else if (C == c) {
							header_state = HState.matching_connection_close;
						} else {
							header_state = HState.general;
						}
						break;

					default:
						header_state = HState.general;
						break;
					}
					break;
				} // header value start



				case State.header_value:
				{
					c = UPCASE[ch];
					if (c == 0) {
						if (CR == ch) {
							settings.RaiseOnHeaderValue(this, data, header_value_mark, p-header_value_mark);
							header_value_mark = -1;

							state = State.header_almost_done;
							break;
						}

						if (LF == ch) {
							settings.RaiseOnHeaderValue(this, data, header_value_mark, p-header_value_mark);
							header_value_mark = -1;
              
							if (!header_almost_done(ch)) {
								settings.RaiseOnError(this,"incorrect header ending, expection LF", data, p_err);
							}
							break;
						}
						break;
					}

					switch (header_state) {
					case HState.general:
						break;

					case HState.connection:
					case HState.transfer_encoding:
						settings.RaiseOnError(this, "Shouldn't be here", data, p_err);
						break;

					case HState.content_length:
						if (ch == ' ') break;
						if (!isDigit(ch))
							settings.RaiseOnError(this, "Content-Length not numeric", data, p_err);

						content_length *= 10;
						content_length += (int)ch - 0x30;
						break;

						/* Transfer-Encoding: chunked */
					case HState.matching_transfer_encoding_chunked:
						index++;
						if (index > CHUNKED.Length || c != CHUNKED[index]) {
							header_state = HState.general;
						} else if (index == CHUNKED.Length-1) {
							header_state = HState.transfer_encoding_chunked;
						}
						break;

						/* looking for 'Connection: keep-alive' */
					case HState.matching_connection_keep_alive:
						index++;
						if (index > KEEP_ALIVE.Length || c != KEEP_ALIVE[index]) {
							header_state = HState.general;
						} else if (index == KEEP_ALIVE.Length-1) {
							header_state = HState.connection_keep_alive;
						}
						break;

						/* looking for 'Connection: close' */
					case HState.matching_connection_close:
						index++;
						if (index > CLOSE.Length || c != CLOSE[index]) {
							header_state = HState.general;
						} else if (index == CLOSE.Length-1) {
							header_state = HState.connection_close;
						}
						break;

					case HState.transfer_encoding_chunked:
					case HState.connection_keep_alive:
					case HState.connection_close:
						if (SPACE != ch) header_state = HState.general;
						break;

					default:
						state = State.header_value;
						header_state = HState.general;
						break;
					}
					break;
				} // header_value



				case State.header_almost_done:
					if (!header_almost_done(ch))
						settings.RaiseOnError(this,"incorrect header ending, expection LF", data, p_err);
					break;

				case State.headers_almost_done:
					if (!headers_almost_done(ch, settings, data, p_err))
						return;
					break;

				/******************* Header *******************/




				/******************* Body *******************/
				case State.body_identity:
					to_read = min(pe - p, content_length); //TODO change to use buffer? 

					if (to_read > 0) {
						settings.RaiseOnBody(this, data, p, to_read); 
						data.Position = p+to_read;
						content_length -= to_read;
						if (content_length == 0) {
							settings.RaiseOnMessageComplete(this);
							state = new_message(); 
						}
					}
					break;



				case State.body_identity_eof:
					to_read = pe - p;  // TODO change to use buffer ?
					if (to_read > 0) {
						settings.RaiseOnBody(this, data, p, to_read); 
						data.Position = p+to_read;
					}
					break;
				/******************* Body *******************/



				/******************* Chunk *******************/
				case State.chunk_size_start:
					if (0 == (flags & F_CHUNKED))
						settings.RaiseOnError(this, "not chunked", data, p_err);

					c = UNHEX[ch];
					if (c == -1) {
						settings.RaiseOnError(this, "invalid hex char in chunk content length", data, p_err);
					}
					content_length = c;
					state = State.chunk_size;
					break;



				case State.chunk_size:
					if (0 == (flags & F_CHUNKED))
						settings.RaiseOnError(this, "not chunked", data, p_err);

					if (CR == ch) {
						state = State.chunk_size_almost_done;
						break;
					}

					c = UNHEX[ch];

					if (c == -1) {
						if (SEMI == ch || SPACE == ch) {
							state = State.chunk_parameters;
							break;
						}
						settings.RaiseOnError(this, "invalid hex char in chunk content length", data, p_err);
					}

					content_length *= 16;
					content_length += c;
					break;



				case State.chunk_parameters:
					if (0 == (flags & F_CHUNKED))
						settings.RaiseOnError(this, "not chunked", data, p_err);

				/* just ignore this shit. TODO check for overflow */
					if (CR == ch) {
						state = State.chunk_size_almost_done;
						break;
					}
					break;
          


				case State.chunk_size_almost_done:
					if (0 == (flags & F_CHUNKED)) {
						settings.RaiseOnError(this, "not chunked", data, p_err);
					}
					if (strict && LF != ch) {
						settings.RaiseOnError(this, "expected LF at end of chunk size", data, p_err);
					}

					if (0 == content_length) {
						flags |= F_TRAILING;
						state = State.header_field_start;
					} else {
						state = State.chunk_data;
					}
					break;



				case State.chunk_data:
				{
					if (0 == (flags & F_CHUNKED)) {
						settings.RaiseOnError(this, "not chunked", data, p_err);
					}

					to_read = min(pe-p, content_length);
					if (to_read > 0) {
						settings.RaiseOnBody(this, data, p, to_read);
						data.Position = p+to_read;
					}

					if (to_read == content_length) {
						state = State.chunk_data_almost_done;
					}

					content_length -= to_read;
					break;
				}



				case State.chunk_data_almost_done:
					if (0 == (flags & F_CHUNKED)) {
						settings.RaiseOnError(this, "not chunked", data, p_err);
					}
					if (strict && CR != ch) {
						settings.RaiseOnError(this, "chunk data terminated incorrectly, expected CR", data, p_err);
					}
					state = State.chunk_data_done;
					break;



				case State.chunk_data_done:
					if (0 == (flags & F_CHUNKED)) {
						settings.RaiseOnError(this, "not chunked", data, p_err);
					}
					if (strict && LF != ch) {
						settings.RaiseOnError(this, "chunk data terminated incorrectly, expected LF", data, p_err);
					}
					state = State.chunk_size_start;
					break;
				/******************* Chunk *******************/
    
        
        
				default:
					settings.RaiseOnError(this, "unhandled state", data, p_err);
					break;
          
				} // switch
			} // while

			p = (int) data.Position;


		/* Reaching this point assumes that we only received part of a
		 * message, inform the callbacks about the progress made so far*/
    
			settings.RaiseOnHeaderField (this, data, header_field_mark, p-header_field_mark);
			settings.RaiseOnHeaderValue (this, data, header_value_mark, p-header_value_mark);
			settings.RaiseOnFragment    (this, data, fragment_mark,     p-fragment_mark);
			settings.RaiseOnQueryString (this, data, query_string_mark, p-query_string_mark);
			settings.RaiseOnPath        (this, data, path_mark,         p-path_mark);
			settings.RaiseOnUrl         (this, data, url_mark,          p-url_mark);
	
		} // execute

		/* If http_should_keep_alive() in the on_headers_complete or
	 * on_message_complete callback returns true, then this will be should be
	 * the last message on the connection.
	 * If you are the server, respond with the "Connection: close" header.
	 * If you are the client, close the connection.
	 */
		bool http_should_keep_alive() {
			if (http_major > 0 && http_minor > 0) {
				/* HTTP/1.1 */
				if ( 0 != (flags & F_CONNECTION_CLOSE) ) {
					return false;
				} else {
					return true;
				}
			} else {
				/* HTTP/1.0 or earlier */
				if ( 0 != (flags & F_CONNECTION_KEEP_ALIVE) ) {
					return true;
				} else {
					return false;
				}
			}
		}

		bool isDigit(int b) {
			if (b >= 0x30 && b <=0x39) {
				return true;
			}
			return false;
		}

		bool isAtoZ(int b) {
			byte c = lower(b);
			return (c>= 0x61 /*a*/ && c <=  0x7a /*z*/);
		}

		bool usual(int b) {

			//static const uint32_t  usual[] = {
			//    0xffffdbfe, /* 1111 1111 1111 1111  1101 1011 1111 1110 */
			//
			//                /* ?>=< ;:98 7654 3210  /.-, +*)( '&%$ #"!  */
			//    0x7ffffff6, /* 0111 1111 1111 1111  1111 1111 1111 0110 */
			//
			//                /* _^]\ [ZYX WVUT SRQP  ONML KJIH GFED CBA@ */
			//    0xffffffff, /* 1111 1111 1111 1111  1111 1111 1111 1111 */
			//
			//                /*  ~}| {zyx wvut srqp  onml kjih gfed cba` */
			//    0xffffffff, /* 1111 1111 1111 1111  1111 1111 1111 1111 */
			//
			//    0xffffffff, /* 1111 1111 1111 1111  1111 1111 1111 1111 */
			//    0xffffffff, /* 1111 1111 1111 1111  1111 1111 1111 1111 */
			//    0xffffffff, /* 1111 1111 1111 1111  1111 1111 1111 1111 */
			//    0xffffffff  /* 1111 1111 1111 1111  1111 1111 1111 1111 */
			//};
			//
			//#define USUAL(c) (usual[c >> 5] & (1 << (c & 0x1f)))

			switch (b) {
			case NULL:
			case CR:
			case LF:
			case SPACE:
			case QMARK:
			case HASH:
				return false;
			}
			return true;

		}

		byte lower (int b) {
			return (byte)(b|0x20);
		}

		byte upper(int b) {
			char c = (char)(b);
			return (byte)Char.ToUpper (c);
		}
	

		HttpMethod start_req_method_assign(int c){
			switch (c) {
			case C: return HttpMethod.HTTP_CONNECT;  /* or COPY, CHECKOUT */
			case D: return HttpMethod.HTTP_DELETE;  
			case G: return HttpMethod.HTTP_GET;     
			case H: return HttpMethod.HTTP_HEAD;    
			case L: return HttpMethod.HTTP_LOCK;    
			case M: return HttpMethod.HTTP_MKCOL;    /* or MOVE, MKACTIVITY, MERGE */
			case O: return HttpMethod.HTTP_OPTIONS; 
			case P: return HttpMethod.HTTP_POST;     /* or PROPFIND, PROPPATH, PUT */
			case R: return HttpMethod.HTTP_REPORT;
			case T: return HttpMethod.HTTP_TRACE;   
			case U: return HttpMethod.HTTP_UNLOCK;  
			}
			return HttpMethod.ERROR; // ugh.
		}

		bool header_almost_done(int ch) {
			if (strict && LF != ch) {
				return false;
			}

			state = State.header_field_start;
			// TODO java enums support some sort of bitflag mechanism !?
			switch (header_state) {
			case HState.connection_keep_alive:
				flags |= F_CONNECTION_KEEP_ALIVE;
				break;
			case HState.connection_close:
				flags |= F_CONNECTION_CLOSE;
				break;
			case HState.transfer_encoding_chunked:
				flags |= F_CHUNKED;
				break;
			default:
				break;
			}
			return true;
		}

		// Return true if we should continue processing
		bool headers_almost_done (int ch, ParserSettings settings, ByteBuffer data, int p_err) {

			if (strict && LF != ch) {
				settings.RaiseOnError (this, "header not properly completed", data, p_err);
				return false;
			}

			if (0 != (flags & F_TRAILING)) {
				/* End of a chunked request */

				settings.RaiseOnHeadersComplete(this);
				settings.RaiseOnMessageComplete(this);

				state = new_message(); 

				return true;
			}

			nread = 0;

			if (0 != (flags & F_UPGRADE) || HttpMethod.HTTP_CONNECT == method) upgrade = true;

			/* Here we call the headers_complete callback. This is somewhat
			 * different than other callbacks because if the user returns 1, we
			 * will interpret that as saying that this message has no body. This
			 * is needed for the annoying case of recieving a response to a HEAD
			 * request.
			 */

			/* (responses to HEAD request contain a CONTENT-LENGTH header
			 * but no content)
			 *
			 * Consider what to do here: I don't like the idea of the callback
			 * interface having a different contract in the case of HEAD
			 * responses. The alternatives would be either to:
			 *
			 * a.) require the header_complete callback to implement a different
			 * interface or
			 *
			 * b.) provide an overridden execute(bla, bla, bool
			 * parsingHeader) implementation ...
			 */

			if (null != settings.OnHeadersComplete) {
				if (settings.RaiseOnHeadersComplete (this) == 1)
					flags |= F_SKIPBODY;
			}


			// Exit, the rest of the connect is in a different protocol.
			if (upgrade) {
				settings.RaiseOnMessageComplete(this);
				state = new_message ();
				return false;
			}

			if (0 != (flags & F_SKIPBODY)) {
				settings.RaiseOnMessageComplete(this);
				state = new_message(); 
			} else if (0 != (flags & F_CHUNKED)) {
				/* chunked encoding - ignore Content-Length header */
				state = State.chunk_size_start;
			} else {
				if (content_length == 0) {
				/* Content-Length header given but zero: Content-Length: 0\r\n */
					settings.RaiseOnMessageComplete(this);
					state = new_message(); 
				} else if (content_length > 0) {
				/* Content-Length header given and non-zero */
					state = State.body_identity;
				} else {
					if (type == ParserType.HTTP_REQUEST || http_should_keep_alive()) {
						/* Assume content-length 0 - read the next */
						settings.RaiseOnMessageComplete(this);
						state = new_message(); 
					} else {
						/* Read body until EOF */
						state = State.body_identity_eof;
					}
				}
			}
			return true;
		} // headers_almost_fone


		private static int min (int a, int b)
		{
			return a < b ? a : b;
		}
  
		/* probably not the best place to hide this ... */
		public bool HTTP_PARSER_STRICT;

		State new_message() {
			if (HTTP_PARSER_STRICT){
				return http_should_keep_alive() ? start_state() : State.dead;
			} else {
				return start_state();
			}

		}
	
		State start_state() {
			return type == ParserType.HTTP_REQUEST ? State.start_req : State.start_res;
		}


		bool parsing_header(State state) {

			switch (state) {
			case State.chunk_size_start :
			case State.chunk_size :
			case State.chunk_size_almost_done :
			case State.chunk_parameters :
			case State.chunk_data :
			case State.chunk_data_almost_done :
			case State.chunk_data_done :
			case State.body_identity :
			case State.body_identity_eof :
				return false;

			}
			return (0==(flags & F_TRAILING));
		}

		/* "Dial CONST for Constants" */
		const int HTTP_MAX_HEADER_SIZE = 80 * 1024;

		const int F_CHUNKED               = 1 << 0;
		const int F_CONNECTION_KEEP_ALIVE = 1 << 1;
		const int F_CONNECTION_CLOSE      = 1 << 2;
		const int F_TRAILING              = 1 << 3;
		const int F_UPGRADE               = 1 << 4;
		const int F_SKIPBODY              = 1 << 5;

		static readonly byte [] UPCASE = new byte [] {
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x20,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x2d,0x00,0x2f,
			0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,  0x38,0x39,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x41,0x42,0x43,0x44,0x45,0x46,0x47,  0x48,0x49,0x4a,0x4b,0x4c,0x4d,0x4e,0x4f,
			0x50,0x51,0x52,0x53,0x54,0x55,0x56,0x57,  0x58,0x59,0x5a,0x00,0x00,0x00,0x00,0x5f,
			0x00,0x41,0x42,0x43,0x44,0x45,0x46,0x47,  0x48,0x49,0x4a,0x4b,0x4c,0x4d,0x4e,0x4f,
			0x50,0x51,0x52,0x53,0x54,0x55,0x56,0x57,  0x58,0x59,0x5a,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
				0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,  0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
			};
			static readonly byte [] CONNECTION = new byte [] {
				0x43, 0x4f, 0x4e, 0x4e, 0x45, 0x43, 0x54, 0x49, 0x4f, 0x4e, 
			};
			static readonly byte [] PROXY_CONNECTION = new byte [] {
				0x50, 0x52, 0x4f, 0x58, 0x59, 0x2d, 0x43, 0x4f, 0x4e, 0x4e, 0x45, 0x43, 0x54, 0x49, 0x4f, 0x4e, 
			};
			static readonly byte [] CONTENT_LENGTH =  new byte [] {
				0x43, 0x4f, 0x4e, 0x54, 0x45, 0x4e, 0x54, 0x2d, 0x4c, 0x45, 0x4e, 0x47, 0x54, 0x48, 
			};
			static readonly byte [] TRANSFER_ENCODING =  new byte [] {
				0x54, 0x52, 0x41, 0x4e, 0x53, 0x46, 0x45, 0x52, 0x2d, 0x45, 0x4e, 0x43, 0x4f, 0x44, 0x49, 0x4e, 0x47, 
			};
			static readonly byte [] UPGRADE =  new byte [] {
				0x55, 0x50, 0x47, 0x52, 0x41, 0x44, 0x45, 
			};
			static readonly byte [] CHUNKED =  new byte [] {
				0x43, 0x48, 0x55, 0x4e, 0x4b, 0x45, 0x44, 
			};
			static readonly byte [] KEEP_ALIVE =  new byte [] {
				0x4b, 0x45, 0x45, 0x50, 0x2d, 0x41, 0x4c, 0x49, 0x56, 0x45, 
			};
			static readonly byte [] CLOSE =  new byte [] {
				0x43, 0x4c, 0x4f, 0x53, 0x45, 
			};

			static readonly int [] UNHEX = new int [] 
			{    -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
			     ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
			     ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
			     , 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,-1,-1,-1,-1,-1,-1
			     ,-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1
			     ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
			     ,-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1
			     ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
			};
			public const byte A = 0x41;
			public const byte B = 0x42;
			public const byte C = 0x43;
			public const byte D = 0x44;
			public const byte E = 0x45;
			public const byte F = 0x46;
			public const byte G = 0x47;
			public const byte H = 0x48;
			public const byte I = 0x49;
			public const byte J = 0x4a;
			public const byte K = 0x4b;
			public const byte L = 0x4c;
			public const byte M = 0x4d;
			public const byte N = 0x4e;
			public const byte O = 0x4f;
			public const byte P = 0x50;
			public const byte Q = 0x51;
			public const byte R = 0x52;
			public const byte S = 0x53;
			public const byte T = 0x54;
			public const byte U = 0x55;
			public const byte V = 0x56;
			public const byte W = 0x57;
			public const byte X = 0x58;
			public const byte Y = 0x59;
			public const byte Z = 0x5a;
			public const byte CR = 0x0d;
			public const byte LF = 0x0a;
			public const byte DOT = 0x2e;
			public const byte SPACE = 0x20;
			public const byte SEMI = 0x3b;
			public const byte COLON = 0x3a;
			public const byte HASH = 0x23;
			public const byte QMARK = 0x3f;
			public const byte SLASH = 0x2f;
			public const byte DASH = 0x2d;
			public const byte NULL = 0x00;
	}

}
