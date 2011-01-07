//
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
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Http;
using MarkdownSharp;


namespace Manos.Tool {

	public class DocsModule : ManosApp {

		private string docs_dir;

		private Dictionary<string,string> manuals;
		private List<string> tutorial_pages;

		public DocsModule (string docs_dir)
		{
			this.docs_dir = docs_dir;

			manuals = new Dictionary<string,string> ();

			manuals.Add ("Installation", "installation.md");
			manuals.Add ("Routing", "routing.md");
			manuals.Add ("Caching Objects", "object-cache.md");
			manuals.Add ("Pipes", "pipes.md");
			manuals.Add ("Timeouts", "timeouts.md");

			tutorial_pages = new List<string> () {
				"Getting Started With Manos",
				"Building the Shorty Application"
			};
		}

		[Route ("/$", "/Index")]
		public void Index (IManosContext ctx)
		{
			WriteMarkdownDocsPage (ctx.Response, "intro.md");
		}

		[Route ("/Manuals/{manual}")]
		public void Manual (IManosContext ctx, string manual)
		{
			string md_page;

			if (!manuals.TryGetValue (manual, out md_page)) {
				ctx.Response.StatusCode = 404;
				ctx.Response.End ();
				return;
			}

			WriteMarkdownDocsPage (ctx.Response, md_page);
		}

		[Route ("/Tutorial/{page}")]
		public void Tutorial (IManosContext ctx, string page)
		{
			WriteMarkdownDocsPage (ctx.Response, "tutorial/" + page);
		}		

		private void WriteMarkdownDocsPage (IHttpResponse response, string page)
		{
			page = Path.Combine (docs_dir, page);
			if (!File.Exists (page)) {
				response.StatusCode = 404;
				return;
			}

			string markdown = File.ReadAllText (page);
			
			Markdown md_processor = new Markdown ();
			string html = md_processor.Transform (markdown);

			WritePage (response, html);
		}

		private void WritePage (IHttpResponse response, string body)
		{
			response.Write (@"<html>
					   <head>	
					    <meta http-equiv=""Content-Type"" content=""text/html; charset=iso-8859-1"">
					    <title>Manos Documentation Browser</title>
					    <style type=""text/css"">
						html,
						body {
						 font-family: Arial, Helvetica, sans-serif;
						 font-size: 13px;
						}
                                                h2 {
						 margin-top: 10px;
                                                }
						#wrapper { 
						 margin: 0 auto;
						 width: 700px;
						}
						#header {
						 color: #333;
						 width: 700px;
						 padding: 10px;
						 height: 100px;
						 margin: 10px 0px 5px 0px;
						 background: #D1DBDB;
						}
						#nav { 
						 display: inline;
						 color: #333;
						 margin: 10px;
						 padding: 0px;
						 width: 220px;
						 float: right;
						}
						#main { 
						 float: left;
						 color: #333;
						 margin: 10px;
						 padding: 0px;
						 width: 400px;
						 display: inline;
						 position: relative;
						}
						.clear { clear: both; background: none; }
                                           </style>
					   </head>
					   <body>
 					    <div id=""wrapper"">
	  				     <div id=""header"">
			                      <h1>Manos Documentation Browser</h1>
					     </div>
					     <div id=""nav"">");
			WriteNavigation (response);

			response.Write (@"   </div>
					     <div id=""main"">");
			response.Write (body);
	    		response.Write (@"   </div>
					    </div>
					   </body>
					  </html>");

			response.End ();
		}

		private void WriteNavigation (IHttpResponse response)
		{
			response.WriteLine ("<h2>Tutorial</h2>");
			response.WriteLine ("<ul>");
			for (int i = 0; i < tutorial_pages.Count; i++) {
				response.WriteLine ("<li><a href='/Tutorial/page-{0}.md'>{1}</a></li>", i + 1, tutorial_pages [i]);
			}
			response.WriteLine ("</ul>");
			
			response.WriteLine ("<h2>Manuals</h2>");
			response.WriteLine ("<ul>");
			foreach (string manual in manuals.Keys) {
				response.WriteLine ("<li><a href='/Manuals/{0}'>{0}</a></li>", manual);
			}
			response.WriteLine ("</ul>");
		}
	}
}
