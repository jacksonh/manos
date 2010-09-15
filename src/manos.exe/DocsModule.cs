
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Manos;
using Manos.Server;
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
				"Getting Started With Manos."
			};
		}

		[Route ("/$", "/Index")]
		public void Index (IManosContext ctx)
		{
			WriteMarkdownDocsPage (ctx.Response, "intro.md");
		}

		[Route ("/Manuals/{manual}")]
		public void Manual (DocsModule docs, IManosContext ctx, string manual)
		{
			string md_page;

			if (!manuals.TryGetValue (manual, out md_page)) {
				ctx.Response.StatusCode = 404;
				return;
			}

			WriteMarkdownDocsPage (ctx.Response, md_page);
		}

		[Route ("/Tutorial/{page}")]
		public void Tutorial (DocsModule docs, IManosContext ctx, int page)
		{
			WriteMarkdownDocsPage (ctx.Response, "tutorial/page-" + page + ".md");
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
		}

		private void WriteNavigation (IHttpResponse response)
		{
			response.WriteLine ("<h2>Tutorial</h2>");
			response.WriteLine ("<ul>");
			for (int i = 0; i < tutorial_pages.Count; i++) {
				response.WriteLine ("<li><a href='/Tutorial/{0}'>{1}</a></li>", i + 1, tutorial_pages [i]);
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
