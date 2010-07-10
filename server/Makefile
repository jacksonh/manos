
MANGO_API_SRC=MangoApp.cs		\
	      MangoModule.cs		\
	      MangoAction.cs		\
	      MangoContext.cs		\
	      TemplateData.cs		\
	      GetAttribute.cs		\
	      HeadAttribute.cs		\
	      PutAttribute.cs		\
	      DeleteAttribute.cs	\
	      IgnoreAttribute.cs	\
	      TraceAttribute.cs		\
	      OptionsAttribute.cs	\
	      PostAttribute.cs		\
	      HttpMethodAttribute.cs	\

MANGO_SERVER_SRC=Mango.Server/AssemblyInfo.cs		\
		 Mango.Server/HttpException.cs		\
		 Mango.Server/HttpRequest.cs		\
		 Mango.Server/HttpServer.cs		\
		 Mango.Server/IHttpConnection.cs	\
		 Mango.Server/IOLoop.cs			\
		 Mango.Server/UriParser.cs		\
		 Mango.Server/HttpConnection.cs  	\
		 Mango.Server/HttpHeaders.cs		\
		 Mango.Server/HttpResponse.cs		\
		 Mango.Server/HttpUtility.cs		\
		 Mango.Server/IOStream.cs

MANGO_ROUTING_SRC=Mango.Routing/RouteHandler.cs		\
		  Mango.Routing/HttpMethods.cs


Mango.dll: *.cs $(MANGO_SERVER_SRC) $(MANGO_API_SRC) $(MANGO_ROUTING_SRC)
	dmcs -debug -r:Mono.Posix.dll -r:Minge.dll -r:System.Core.dll -out:Mango.dll -target:library $(MANGO_API_SRC) $(MANGO_SERVER_SRC) $(MANGO_ROUTING_SRC)

mango.exe: Mango.Server/mango.cs
	dmcs -debug -r:Mango.dll -out:mango.exe Mango.Server/mango.cs

mango-tests.dll: tests/*.cs Mango.dll
	dmcs -debug -r:Mango.dll -r:nunit.framework.dll -out:mango-tests.dll -target:library tests/*.cs

run-tests: mango-tests.dll
	mono ../bin/nunit-console.exe mango-tests.dll