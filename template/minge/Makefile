
SOURCES=codegen.cs 		\
	parser.cs  		\
	token.cs      		\
	token-operators.cs  	\
	token-type.cs		\
	environment.cs  	\
	compile.cs		\
	library.cs		\
	tokenizer.cs  		\
	token-rules.cs


all: compile.exe render.exe

run-test: all
	mono --debug compile.exe mingetest.dll template2.html
	mono --debug render.exe mingetest.dll templates.page_template2 foo '' bar 'param two'

compile.exe: $(SOURCES)
	gmcs -debug -out:compile.exe -r:Mono.Cecil.dll $(SOURCES)

render.exe: render.cs
	gmcs -debug -out:render.exe render.cs

stream-test: stream-test.cs library.cs
	gmcs -debug -out:stream-test.dll -target:library stream-test.cs library.cs