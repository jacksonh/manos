$(if $(wildcard config.make),,$(error You need to run './configure' before running 'make'.))
include config.make
conf=Debug
SLN=src/Manos.sln
VERBOSITY=normal
version=0.1
install_bin_dir = $(prefix)/lib/manos/
install_data_dir = $(prefix)/share/manos/
install_docs_dir = $(prefix)/share/manos/docs/
install_script_dir = $(prefix)/bin/
install_man_dir = $(prefix)/share/man/man1/
install_pc_dir = $(pkg_config_path)/
distdir = manos-$(version)

XBUILD_ARGS=/verbosity:$(VERBOSITY) /nologo

srcdir_abs=$(shell pwd)
LOCAL_CONFIG=$(srcdir_abs)/../../local-config

ifeq ($(strip $(wildcard "${LOCAL_CONFIG}/monodevelop.pc")),)
	XBUILD=PKG_CONFIG_PATH="${LOCAL_CONFIG}:${PKG_CONFIG_PATH}" xbuild $(XBUILD_ARGS)
else
	XBUILD=xbuild $(XBUILD_ARGS)
endif

NUNIT_CONSOLE = nunit-console4

define MANOS_EXEC_SCRIPT
#!/bin/bash
exec mono $$MONO_OPTIONS "$(install_bin_dir)manos.exe" "$$@"
endef
export MANOS_EXEC_SCRIPT

define MANOS_PC_SCRIPT
Name: Manos
Description: Manos
Version: $(version)
Libraries: $(install_bin_dir)Manos.dll $(install_bin_dir)Manos.dll.mdb 

Requires: 
Libs: -r:$(install_bin_dir)Manos.dll
endef
export MANOS_PC_SCRIPT


all: 
	@test -f config.make || (echo "You need to run ./configure." && exit 1)
	$(XBUILD) $(SLN) /property:Configuration=$(conf)

run-tests: all
	$(NUNIT_CONSOLE) lib/manos/Manos.Tests.dll

update-docs: all
	mdoc update -o ./docs/api/en ./lib/manos/Manos.dll

clean:
	$(XBUILD) $(SLN) /property:Configuration=$(conf) /t:Clean
	rm -rf lib/manos/*

install: install-bin install-data install-docs install-script install-man install-pkg-config

install-data:
	test -d $(install_data_dir) || install -d $(install_data_dir)
	cp -rf ./data/* "$(install_data_dir)"

install-docs:
	test -d $(install_docs_dir) || install -d $(install_docs_dir)
	cp -rf ./docs/* $(install_docs_dir)

install-bin: all
	test -d $(install_bin_dir) || install -d $(install_bin_dir)
	cp -rf ./lib/manos/* $(install_bin_dir)

install-script:
	test -d $(install_script_dir) || install -d $(install_script_dir)
	echo "$$MANOS_EXEC_SCRIPT" > $(install_script_dir)manos
	chmod +x $(install_script_dir)manos

install-man:
	test -d $(install_man_dir) || install -d $(install_man_dir)
	cp -rf ./man/* $(install_man_dir)

install-pkg-config:
	test -d $(install_pc_dir) || install -d $(install_pc_dir)
	echo "$$MANOS_PC_SCRIPT" > $(install_pc_dir)manos.pc


uninstall:
	echo "not implemented"
#	rm -rf "$(installdir)"

dist: clean update-docs
	rm -rf $(distdir)
	mkdir $(distdir)
	cp -rf ./src/ ./data/ ./man ./docs $(distdir)
	cp -rf configure Makefile $(distdir)
	tar cjvf manos-$(version).tar.bz2 manos-$(version)

release: dist
	cp manos-$(version).tar.bz2 release/.
	cd release && rpmbuild -ba manos.spec

gem: all
	cp release/manos.gemspec lib/manos/.
	cd build && gem build manos.gemspec
