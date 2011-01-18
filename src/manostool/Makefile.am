
manosdir=$(bindir)

pkglib_DATA = $(top_srcdir)/build/manostool.exe

manos_proj_file = manostool.csproj
manos_sources_in = AssemblyInfo.cs.in
manos_sources = $(manos_proj_file) $(shell  $(XBUILD) /verbosity:minimal /target:ListSources /nologo | sed -e '1,3d' -e "s/\;/ /g")

EXTRA_DIST = $(manos_sources) $(manos_sources_in)
CLEANFILES = $(top_srcdir)/build/manostool.exe


$(top_srcdir)/build/manostool.exe: $(manos_sources)
	$(XBUILD) $(manos_proj_file)
