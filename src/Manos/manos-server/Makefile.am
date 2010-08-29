
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/mango-server.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../bin/Debug/Mango.dll
BUILD_DIR = bin/Debug

MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MANGO_DLL_SOURCE=../bin/Debug/Mango.dll
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MANGO_DLL_MDB_SOURCE=../bin/Debug/Mango.dll.mdb
MANGO_DLL_MDB=$(BUILD_DIR)/Mango.dll.mdb
MANGO_SERVER_EXE_MDB_SOURCE=bin/Debug/mango-server.exe.mdb
MANGO_SERVER_EXE_MDB=$(BUILD_DIR)/mango-server.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/mango-server.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../bin/Release/Mango.dll
BUILD_DIR = bin/Release

MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MANGO_DLL_SOURCE=../bin/Release/Mango.dll
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MANGO_DLL_MDB=
MANGO_SERVER_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(MONO_POSIX_DLL) \
	$(MONO_POSIX_DLL_MDB) \
	$(MANGO_DLL) \
	$(MONO_CECIL_DLL) \
	$(MONO_CECIL_DLL_MDB) \
	$(MANGO_DLL_MDB) \
	$(MANGO_SERVER_EXE_MDB)  

BINARIES = \
	$(MANGO_SERVER)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	AssemblyInfo.cs \
	mango.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	mango-server.in 

REFERENCES =  \
	System

DLL_REFERENCES =  \
	../../../deps/Mono.Posix.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
MONO_POSIX_DLL_MDB = $(BUILD_DIR)/Mono.Posix.dll.mdb
MANGO_DLL = $(BUILD_DIR)/Mango.dll
MONO_CECIL_DLL = $(BUILD_DIR)/Mono.Cecil.dll
MONO_CECIL_DLL_MDB = $(BUILD_DIR)/Mono.Cecil.dll.mdb
MANGO_SERVER = $(BUILD_DIR)/mango-server

$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL_MDB))
$(eval $(call emit-deploy-target,MANGO_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL_MDB))
$(eval $(call emit-deploy-target,MANGO_DLL_MDB))
$(eval $(call emit-deploy-wrapper,MANGO_SERVER,mango-server,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
