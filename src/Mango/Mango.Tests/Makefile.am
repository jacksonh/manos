
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/Mango.Tests.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../bin/Debug/Mango.dll
BUILD_DIR = bin/Debug

NUNIT_FRAMEWORK_DLL_SOURCE=../../../deps/nunit.framework.dll
MANGO_DLL_SOURCE=../bin/Debug/Mango.dll
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MANGO_DLL_MDB_SOURCE=../bin/Debug/Mango.dll.mdb
MANGO_DLL_MDB=$(BUILD_DIR)/Mango.dll.mdb
MANGO_TESTS_DLL_MDB_SOURCE=bin/Debug/Mango.Tests.dll.mdb
MANGO_TESTS_DLL_MDB=$(BUILD_DIR)/Mango.Tests.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Mango.Tests.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../bin/Release/Mango.dll
BUILD_DIR = bin/Release

NUNIT_FRAMEWORK_DLL_SOURCE=../../../deps/nunit.framework.dll
MANGO_DLL_SOURCE=../bin/Release/Mango.dll
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MANGO_DLL_MDB=
MANGO_TESTS_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(NUNIT_FRAMEWORK_DLL) \
	$(MANGO_DLL) \
	$(MONO_CECIL_DLL) \
	$(MONO_CECIL_DLL_MDB) \
	$(MONO_POSIX_DLL) \
	$(MONO_POSIX_DLL_MDB) \
	$(MANGO_DLL_MDB) \
	$(MANGO_TESTS_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(MANGO_TESTS_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Mango/MangoModuleTest.cs \
	Mango.Routing/RouteHandlerTest.cs \
	Mango/MangoTargetTest.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Mango.Routing \
	Mango \
	mango.tests.pc.in 

REFERENCES =  \
	System

DLL_REFERENCES =  \
	../../../deps/nunit.framework.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

NUNIT_FRAMEWORK_DLL = $(BUILD_DIR)/nunit.framework.dll
MANGO_DLL = $(BUILD_DIR)/Mango.dll
MONO_CECIL_DLL = $(BUILD_DIR)/Mono.Cecil.dll
MONO_CECIL_DLL_MDB = $(BUILD_DIR)/Mono.Cecil.dll.mdb
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
MONO_POSIX_DLL_MDB = $(BUILD_DIR)/Mono.Posix.dll.mdb
MANGO_TESTS_PC = $(BUILD_DIR)/mango.tests.pc

$(eval $(call emit-deploy-target,NUNIT_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,MANGO_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL_MDB))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL_MDB))
$(eval $(call emit-deploy-target,MANGO_DLL_MDB))
$(eval $(call emit-deploy-wrapper,MANGO_TESTS_PC,mango.tests.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
