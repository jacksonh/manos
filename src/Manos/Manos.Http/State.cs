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


namespace Manos.Http {

	enum State {

		dead               

		, start_res_or_res
		, res_or_resp_H
		, start_res
		, res_H
		, res_HT
		, res_HTT
		, res_HTTP
		, res_first_http_major
		, res_http_major
		, res_first_http_minor
		, res_http_minor
		, res_first_status_code
		, res_status_code
		, res_status
		, res_line_almost_done
		
		, start_req
		
		, req_method
		, req_spaces_before_url
		, req_schema
		, req_schema_slash
		, req_schema_slash_slash
		, req_host
		, req_port
		, req_path
		, req_query_string_start
		, req_query_string
		, req_fragment_start
		, req_fragment
		, req_http_start
		, req_http_H
		, req_http_HT
		, req_http_HTT
		, req_http_HTTP
		, req_first_http_major
		, req_http_major
		, req_first_http_minor
		, req_http_minor
		, req_line_almost_done

		, header_field_start
		, header_field
		, header_value_start
		, header_value

		, header_almost_done

		, headers_almost_done

		, chunk_size_start
		, chunk_size
		, chunk_size_almost_done
		, chunk_parameters
		, chunk_data
		, chunk_data_almost_done
		, chunk_data_done

		, body_identity
		, body_identity_eof
	}

}

