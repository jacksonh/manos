
MANGO_API_SRC=MangoApp.cs	\
	      MangoModule.cs	\
	      MangoAction.cs	\
	      MangoContext.cs	\
	      TemplateData.cs	\
	      GetAttribute.cs

MANGO_SERVER_SRC=Mango.Server/AssemblyInfo.cs		\
		 Mango.Server/HttpException.cs		\
		 Mango.Server/HttpRequest.cs		\
		 Mango.Server/HttpServer.cs		\
		 Mango.Server/IOLoop.cs			\
		 Mango.Server/UriParser.cs		\
		 Mango.Server/HttpConnection.cs  	\
		 Mango.Server/HttpHeaders.cs		\
		 Mango.Server/HttpResponse.cs		\
		 Mango.Server/HttpUtility.cs		\
		 Mango.Server/IOStream.cs


Mango.dll: *.cs $(MANGO_SERVER_SRC)
	gmcs -r:Mono.Posix.dll -r:Minge.dll -out:Mango.dll -target:library $(MANGO_API_SRC) $(MANGO_SERVER_SRC)

mango.exe: Mango.Server/mango.cs
	gmcs -r:Mango.dll -out:mango.exe Mango.Server/mango.cs