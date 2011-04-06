AC_INIT(README)
AC_CANONICAL_SYSTEM
AM_INIT_AUTOMAKE(manos, 0.1.3)
AM_MAINTAINER_MODE

AC_PROG_CC
AC_PROG_INSTALL
AM_PROG_LIBTOOL

dnl pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
	AC_MSG_ERROR([You need to install pkg-config])
fi

dnl C# compiler
AC_PATH_PROG(DMCS, dmcs, no)
CS="C#"
if test "x$DMCS" = "xno"  ; then
	dnl AC_MSG_ERROR([You need to install a C# compiler])
	AC_MSG_ERROR([No $CS compiler (dmcs) found])
fi

AC_SUBST(DMCS)



AC_PATH_PROG(XBUILD, xbuild, no)
if test "x$XBUILD" = "xno" ; then
   AC_MSG_ERROR([No xbuild found])
fi

AC_SUBST(XBUILD)


AC_ARG_WITH([gnutls],
	[AS_HELP_STRING([--with-gnutls],
		[support SSL/TLS with GnuTLS @<:@default: check@:>@])],
	[],
	[with_gnutls=check])

AS_IF([test "x$with_gnutls" != xno],
	[AC_CHECK_LIB([gnutls], [gnutls_global_init],
		[AC_DEFINE([HAVE_LIBGNUTLS], [1], [Define if you have libgnutls])
		 PKG_CHECK_MODULES([GNUTLS], [gnutls >= 2.8])
		],
		[if test "x$with_gnutls" != xcheck; then
			AC_MSG_FAILURE([--with-gnutls was given, but test for gnutls failed])
		 fi],
		[-lgnutls])])


case "$host" in
	*-*-linux*)
		CPPFLAGS="$CPPFLAGS -DGC_LINUX_THREADS -D_GNU_SOURCE -D_REENTRANT -DUSE_MMAP"
		if test "x$disable_munmap" != "xyes"; then
			CPPFLAGS="$CPPFLAGS -DUSE_MUNMAP"
		fi
		;;
	*-*-darwin*)
		platform_darwin=yes
		CPPFLAGS="$CPPFLAGS -no-cpp-precomp -D_THREAD_SAFE -DGC_MACOSX_THREADS -DPLATFORM_MACOSX -DUSE_MMAP -DUSE_MUNMAP"
		LDFLAGS="$LDFLAGS -pthread"
		need_link_unlink=yes
		AC_DEFINE(USE_MACH_SEMA, 1, [...])
		has_dtrace=yes
		if test "x$cross_compiling" = "xyes"; then
			has_broken_apple_cpp=yes
		fi
		dnl Snow Leopard is horribly broken -- it reports itself as i386-apple-darwin10.0.0, but
		dnl its gcc defaults to 64-bit mode.  They have also deprecated the usage of ucontext
		dnl we need to set some flags to build our 32-bit binaries on 10.6 properly
		case "$host" in
			dnl Snow Leopard config.guess reports as this
			i*86-*-darwin10*)
				BROKEN_DARWIN_FLAGS="-arch i386 -D_XOPEN_SOURCE -mmacosx-version-min=10.5"
				CPPFLAGS="$CPPFLAGS $BROKEN_DARWIN_FLAGS"
				CFLAGS="$CFLAGS $BROKEN_DARWIN_FLAGS"
				CXXFLAGS="$CXXFLAGS $BROKEN_DARWIN_FLAGS"
				CCASFLAGS="$CCASFLAGS $BROKEN_DARWIN_FLAGS"
				;;
		esac
		;;

esac

m4_include([src/libeio/libeio.m4])
m4_include([src/libev/libev.m4])

AC_CONFIG_HEADERS([src/libev/config.h])
AC_CONFIG_HEADERS([src/libeio/config.h])
AC_CONFIG_HEADERS([src/libmanos/config.h])

AC_OUTPUT([
Makefile
docs/Makefile
man/Makefile
scripts/Makefile
scripts/manos
data/Makefile
data/manos.pc
src/Makefile
src/Manos/Makefile
src/Manos/Assembly/AssemblyInfo.cs
src/manostool/Makefile
src/manostool/AssemblyInfo.cs
src/libev/Makefile
src/libeio/Makefile
src/libmanos/Makefile
])
