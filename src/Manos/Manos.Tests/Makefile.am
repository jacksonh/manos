
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/Manos.Tests.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../bin/Debug/Manos.dll
BUILD_DIR = bin/Debug

NUNIT_FRAMEWORK_DLL_SOURCE=../../../deps/nunit.framework.dll
MANOS_DLL_SOURCE=../bin/Debug/Manos.dll
MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MANOS_DLL_MDB_SOURCE=../bin/Debug/Manos.dll.mdb
MANOS_DLL_MDB=$(BUILD_DIR)/Manos.dll.mdb
MANOS_TESTS_DLL_MDB_SOURCE=bin/Debug/Manos.Tests.dll.mdb
MANOS_TESTS_DLL_MDB=$(BUILD_DIR)/Manos.Tests.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Manos.Tests.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../bin/Release/Manos.dll
BUILD_DIR = bin/Release

NUNIT_FRAMEWORK_DLL_SOURCE=../../../deps/nunit.framework.dll
MANOS_DLL_SOURCE=../bin/Release/Manos.dll
MONO_POSIX_DLL_SOURCE=../../../deps/Mono.Posix.dll
MONO_POSIX_DLL_MDB_SOURCE=../../../deps/Mono.Posix.dll.mdb
MONO_CECIL_DLL_SOURCE=../../../deps/Mono.Cecil.dll
MONO_CECIL_DLL_MDB_SOURCE=../../../deps/Mono.Cecil.dll.mdb
MANOS_DLL_MDB=
MANOS_TESTS_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(NUNIT_FRAMEWORK_DLL) \
	$(MANOS_DLL) \
	$(MONO_POSIX_DLL) \
	$(MONO_POSIX_DLL_MDB) \
	$(MONO_CECIL_DLL) \
	$(MONO_CECIL_DLL_MDB) \
	$(MANOS_DLL_MDB) \
	$(MANOS_TESTS_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(MANOS_TESTS_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Manos/ManosModuleTest.cs \
	Manos.Routing/RouteHandlerTest.cs \
	Manos/ManosTargetTest.cs \
	Manos.Server/HttpRequestTest.cs \
	Manos.Routing/RegexMatchOperationTest.cs \
	Manos.Routing/MatchOperationFactoryTest.cs \
	Manos.Routing/StringMatchOperationTest.cs \
	Manos.Routing/NopMatchOperationTest.cs \
	Manos.Server/HttpHeadersTest.cs \
	Manos.Template/CodegenTest.cs \
	Manos.Template/TemplateFactoryTest.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Manos.Routing \
	Manos \
	Manos.Server \
	Manos.Template \
	manos.tests.pc.in 

REFERENCES =  \
	System

DLL_REFERENCES =  \
	../../../deps/nunit.framework.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

NUNIT_FRAMEWORK_DLL = $(BUILD_DIR)/nunit.framework.dll
MANOS_DLL = $(BUILD_DIR)/Manos.dll
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
MONO_POSIX_DLL_MDB = $(BUILD_DIR)/Mono.Posix.dll.mdb
MONO_CECIL_DLL = $(BUILD_DIR)/Mono.Cecil.dll
MONO_CECIL_DLL_MDB = $(BUILD_DIR)/Mono.Cecil.dll.mdb
MANOS_TESTS_PC = $(BUILD_DIR)/manos.tests.pc

$(eval $(call emit-deploy-target,NUNIT_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,MANOS_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL_MDB))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL))
$(eval $(call emit-deploy-target,MONO_CECIL_DLL_MDB))
$(eval $(call emit-deploy-target,MANOS_DLL_MDB))
$(eval $(call emit-deploy-wrapper,MANOS_TESTS_PC,manos.tests.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
