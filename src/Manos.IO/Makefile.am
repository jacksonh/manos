
manosdir=$(bindir)

pkglib_DATA = $(top_srcdir)/build/Manos.IO.dll

manos_proj_file = Manos.IO.csproj
manos_sources_in = Assembly/AssemblyInfo.cs.in
manos_sources = $(manos_proj_file) $(shell  xbuild /verbosity:minimal /target:ListSources | sed -e '1,3d' -e "s/\;/ /g")

EXTRA_DIST = $(manos_sources) $(manos_sources_in)
CLEANFILES = $(top_srcdir)/build/Manos.IO.dll

all: $(top_srcdir)/build/Manos.IO.dll

$(top_srcdir)/build/Manos.IO.dll: $(manos_sources)
	$(XBUILD) $(srcdir)/$(manos_proj_file)
