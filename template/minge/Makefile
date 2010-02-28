
SOURCES=api.cs		\
	codegen.cs 	\
	parser.cs  	\
	environment.cs  \
	library.cs	\
	tokenizer.cs  		

DEMO_SOURCES=demo.cs


Minge.dll: $(SOURCES)
	gmcs -debug -target:library -r:Mono.Cecil.dll -out:Minge.dll $(SOURCES)


demo: Minge.dll $(DEMO_SOURCES)
	gmcs -debug -r:Minge.dll $(DEMO_SOURCES)
