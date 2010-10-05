#
# spec file for package manos
#
# Copyright (c) 2010 Jackson Harper (jackson@novell.com)
#
#

Name:           manos
Version:        0.0.4
Release:        1
License:        MIT/X11
BuildRoot:      %{_tmppath}/%{name}-%{version}-build
BuildRequires:  mono-devel >= 2.6
Source0:        %{name}-%{version}.tar.bz2
Summary:        The Manos Web Application Framework
Group:          Programming
BuildArch:      noarch

%description
Manos is an easy to use, easy to test, high performance web application framework that stays out of your way and makes your life ridiculously simple.

%files
%defattr(-, root, root)
%{_prefix}/lib/%{name}
%{_bindir}/%{name}
%{_datadir}/%{name}

%prep
%setup -q -n %{name}-%{version}


%build
./configure --prefix=%{prefix}
make

%install
make install

%clean
rm -rf %{buildroot}

%changelog