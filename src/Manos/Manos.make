

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/Mango.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

MONO_CECIL_DLL_SOURCE=../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../deps/Mono.Cecil.dll.mdb
MONO_POSIX_DLL_SOURCE=../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../deps/Mono.Posix.dll.mdb
MANGO_DLL_MDB_SOURCE=bin/Debug/Mango.dll.mdb
MANGO_DLL_MDB=$(BUILD_DIR)/Mango.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Mango.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

MONO_CECIL_DLL_SOURCE=../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../deps/Mono.Cecil.dll.mdb
MONO_POSIX_DLL_SOURCE=../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../deps/Mono.Posix.dll.mdb
MANGO_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(MONO_CECIL_DLL) \
	$(MONO_CECIL_DLL_MDB) \
	$(MONO_POSIX_DLL) \
	$(MONO_POSIX_DLL_MDB) \
	$(MANGO_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(MANGO_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Mango/DeleteAttribute.cs \
	Mango/GetAttribute.cs \
	Mango/HeadAttribute.cs \
	Mango/HttpMethodAttribute.cs \
	Mango/IgnoreAttribute.cs \
	Mango/MangoAction.cs \
	Mango/MangoApp.cs \
	Mango/MangoContext.cs \
	Mango/MangoModule.cs \
	Mango/OptionsAttribute.cs \
	Mango/PostAttribute.cs \
	Mango/PutAttribute.cs \
	Mango/TemplateData.cs \
	Mango/TraceAttribute.cs \
	Mango.Routing/HttpMethods.cs \
	Mango.Routing/RouteHandler.cs \
	Mango.Server/AssemblyInfo.cs \
	Mango.Server/HttpException.cs \
	Mango.Server/HttpHeaders.cs \
	Mango.Server/HttpRequest.cs \
	Mango.Server/HttpResponse.cs \
	Mango.Server/HttpServer.cs \
	Mango.Server/HttpTransaction.cs \
	Mango.Server/HttpUtility.cs \
	Mango.Server/IHttpRequest.cs \
	Mango.Server/IHttpResponse.cs \
	Mango.Server/IHttpTransaction.cs \
	Mango.Server/IOLoop.cs \
	Mango.Server/IOStream.cs \
	Mango.Server/UriParser.cs \
	Mango.Template/api.cs \
	Mango.Template/codegen.cs \
	Mango.Template/environment.cs \
	Mango.Template/library.cs \
	Mango.Template/parser.cs \
	Mango.Template/tokenizer.cs \
	Mango/IMangoContext.cs \
	Mango.Testing/MockMangoModule.cs \
	Mango.Testing/Server/MockHttpTransaction.cs \
	Mango.Testing/Server/MockHttpRequest.cs \
	Mango/MangoTarget.cs \
	Mango/IMangoTarget.cs \
	Mango.Testing/MockMangoTarget.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Mango.Testing \
	Mango.Testing/Server \
	mango.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES =  \
	../../deps/Mono.Cecil.dll \
	../../deps/Mono.Posix.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

MONO_CECIL_DLL = $(BUILD_DIR)/Mono.Cecil.dll
MONO_CECIL_DLL_MDB = $(BUILD_DIR)/Mono.Cecil.dll.mdb
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
MONO_POSIX_DLL_MDB = $(BUILD_DIR)/Mono.Posix.dll.mdb
MANGO_PC = $(BUILD_DIR)/mango.pc

$(eval $(call emit-deploy-target,MONO_CECIL_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL_MDB))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL_MDB))
$(eval $(call emit-deploy-wrapper,MANGO_PC,mango.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
