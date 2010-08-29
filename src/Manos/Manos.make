

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/Manos.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

MONO_POSIX_DLL_SOURCE=../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../deps/Mono.Posix.dll.mdb
MONO_CECIL_DLL_SOURCE=../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../deps/Mono.Cecil.dll.mdb
MANOS_DLL_MDB_SOURCE=bin/Debug/Manos.dll.mdb
MANOS_DLL_MDB=$(BUILD_DIR)/Manos.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Manos.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

MONO_POSIX_DLL_SOURCE=../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../deps/Mono.Posix.dll.mdb
MONO_CECIL_DLL_SOURCE=../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../deps/Mono.Cecil.dll.mdb
MANOS_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(MONO_POSIX_DLL) \
	$(MONO_POSIX_DLL_MDB) \
	$(MONO_CECIL_DLL) \
	$(MONO_CECIL_DLL_MDB) \
	$(MANOS_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(MANOS_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Manos/DeleteAttribute.cs \
	Manos/GetAttribute.cs \
	Manos/HeadAttribute.cs \
	Manos/HttpMethodAttribute.cs \
	Manos/IgnoreAttribute.cs \
	Manos/ManosAction.cs \
	Manos/ManosApp.cs \
	Manos/ManosContext.cs \
	Manos/ManosModule.cs \
	Manos/OptionsAttribute.cs \
	Manos/PostAttribute.cs \
	Manos/PutAttribute.cs \
	Manos/TraceAttribute.cs \
	Manos.Routing/RouteHandler.cs \
	Manos.Server/HttpException.cs \
	Manos.Server/HttpHeaders.cs \
	Manos.Server/HttpRequest.cs \
	Manos.Server/HttpResponse.cs \
	Manos.Server/HttpServer.cs \
	Manos.Server/HttpTransaction.cs \
	Manos.Server/HttpUtility.cs \
	Manos.Server/IHttpRequest.cs \
	Manos.Server/IHttpResponse.cs \
	Manos.Server/IHttpTransaction.cs \
	Manos.Server/IOLoop.cs \
	Manos.Server/IOStream.cs \
	Manos.Server/UriParser.cs \
	Manos.Template/codegen.cs \
	Manos.Template/environment.cs \
	Manos.Template/library.cs \
	Manos.Template/parser.cs \
	Manos.Template/tokenizer.cs \
	Manos/IManosContext.cs \
	Manos.Testing/MockManosModule.cs \
	Manos.Server.Testing/MockHttpTransaction.cs \
	Manos.Server.Testing/MockHttpRequest.cs \
	Manos/ManosTarget.cs \
	Manos/IManosTarget.cs \
	Manos.Testing/MockManosTarget.cs \
	Manos.Server/WriteBytesOperation.cs \
	Manos.Server/WriteFileOperation.cs \
	Manos.Server/IWriteOperation.cs \
	Manos.Routing/IMatchOperation.cs \
	Manos.Routing/RegexMatchOperation.cs \
	Manos.Routing/MatchOperationFactory.cs \
	Manos.Routing/StringMatchOperation.cs \
	Manos.Routing/NopMatchOperation.cs \
	Manos/HttpMethods.cs \
	Manos.Template/Engine.cs \
	Assembly/AssemblyInfo.cs \
	Manos.Template/ManosTemplate.cs \
	Manos.Template/IManosTemplate.cs \
	Manos.Template/TemplateFactory.cs \
	Manos.Templates.Testing/ManosTemplateStub_1.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Manos.Testing \
	Manos.Server.Testing \
	Assembly \
	Manos.Templates.Testing \
	manos.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES =  \
	../../deps/Mono.Posix.dll \
	../../deps/Mono.Cecil.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
MONO_POSIX_DLL_MDB = $(BUILD_DIR)/Mono.Posix.dll.mdb
MONO_CECIL_DLL = $(BUILD_DIR)/Mono.Cecil.dll
MONO_CECIL_DLL_MDB = $(BUILD_DIR)/Mono.Cecil.dll.mdb
MANOS_PC = $(BUILD_DIR)/manos.pc

$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL_MDB))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL_MDB))
$(eval $(call emit-deploy-wrapper,MANOS_PC,manos.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
