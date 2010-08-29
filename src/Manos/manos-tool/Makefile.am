
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/mango-tool.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../bin/Debug/Manos.dll
BUILD_DIR = bin/Debug

MONO_MERGE_DLL_SOURCE=../../../deps/Mono.Merge.dll
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MANOS_DLL_SOURCE=../bin/Debug/Manos.dll
MANOS_DLL_MDB_SOURCE=../bin/Debug/Manos.dll.mdb
MANOS_DLL_MDB=$(BUILD_DIR)/Manos.dll.mdb
MANGO_TOOL_EXE_MDB_SOURCE=bin/Debug/mango-tool.exe.mdb
MANGO_TOOL_EXE_MDB=$(BUILD_DIR)/mango-tool.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/mango-tool.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../bin/Release/Manos.dll
BUILD_DIR = bin/Release

MONO_MERGE_DLL_SOURCE=../../../deps/Mono.Merge.dll
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MANOS_DLL_SOURCE=../bin/Release/Manos.dll
MANOS_DLL_MDB=
MANGO_TOOL_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(MONO_MERGE_DLL) \
	$(MONO_CECIL_DLL) \
	$(MONO_CECIL_DLL_MDB) \
	$(MONO_POSIX_DLL) \
	$(MONO_POSIX_DLL_MDB) \
	$(MANOS_DLL) \
	$(MANOS_DLL_MDB) \
	$(MANGO_TOOL_EXE_MDB)  

BINARIES = \
	$(MANOS_TOOL)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Driver.cs \
	Options.cs \
	InitCommand.cs \
	Environment.cs \
	BuildCommand.cs \
	ServerCommand.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	manos-tool.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES =  \
	../../../deps/Mono.Merge.dll \
	../../../deps/Mono.Posix.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

MONO_MERGE_DLL = $(BUILD_DIR)/Mono.Merge.dll
MONO_CECIL_DLL = $(BUILD_DIR)/Mono.Cecil.dll
MONO_CECIL_DLL_MDB = $(BUILD_DIR)/Mono.Cecil.dll.mdb
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
MONO_POSIX_DLL_MDB = $(BUILD_DIR)/Mono.Posix.dll.mdb
MANOS_DLL = $(BUILD_DIR)/Manos.dll
MANOS_TOOL = $(BUILD_DIR)/manos-tool

$(eval $(call emit-deploy-target,MONO_MERGE_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL_MDB))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL_MDB))
$(eval $(call emit-deploy-target,MANOS_DLL))
$(eval $(call emit-deploy-target,MANOS_DLL_MDB))
$(eval $(call emit-deploy-wrapper,MANOS_TOOL,manos-tool,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
