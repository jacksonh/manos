
include config.make
conf=Debug
SLN=src/Manos.sln
VERBOSITY=normal
version=0.0.1
install_bin_dir = "$(prefix)/lib/manos/"
install_data_dir = "$(prefix)/share/manos/"
install_script_dir = "$(prefix)/bin/"
install_man_dir = "$(prefix)/share/man/man1"
distdir = "manos-$(version)"

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
"#!/bin/bash"
exec mono $(install_bin_dir)manos.exe "\$@"
endef
export MANOS_EXEC_SCRIPT

all: 
	$(XBUILD) $(SLN) /property:Configuration=$(conf)

run-tests: all
	$(NUNIT_CONSOLE) build/Manos.Tests.dll

clean:
	$(XBUILD) $(SLN) /property:Configuration=$(conf) /t:Clean
	rm -rf build/*

install: install-data install-bin install-script install-man


install-data:
	test -d "$(install_data_dir)" || mkdir "$(install_data_dir)"
	cp -rf ./data/* "$(install_data_dir)"

install-bin: all
	test -d "$(install_bin_dir)" || mkdir "$(install_bin_dir)"
	cp -rf ./build/* "$(install_bin_dir)"

install-script:
	test -d "$(install_script_dir)" || mkdir "$(install_script_dir)"
	echo "$$MANOS_EXEC_SCRIPT" > $(install_script_dir)manos
	chmod +x "$(install_script_dir)"manos

install-man:
	test -d "$(install_man_dir)" || mkdir "$(install_man_dir)"
	cp -rf ./man/* "$(install_man_dir)"

uninstall:
	rm -rf "$(installdir)"

dist: clean
	rm -rf "$(distdir)"
	mkdir "$(distdir)"
	cp -rf ./src/ ./data/ ./man "$(distdir)"
	cp -rf configure Makefile "$(distdir)"
	tar cjvf manos-"$(version)".tar.bz2 manos-"$(version)"

release: dist
	cp manos-"$(version)".tar.bz2 release/.
	cd release && rpmbuild -ba manos.spec