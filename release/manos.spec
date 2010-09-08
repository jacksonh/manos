#
# spec file for package manos
#
# Copyright (c) 2010 Jackson Harper (jackson@novell.com)
#
#

Name:           manos
Version:        0.1
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
xbuild /p:Configuration=Debug Manos.sln

%install
install -d %{buildroot}%{_prefix}/lib/%{name}
for i in bin/*; do
install -c -m 644 $i %{buildroot}%{_prefix}/lib/%{name}/
done
# bin wrapper
install -d %{buildroot}%{_bindir}
cat << EOF > %{buildroot}%{_bindir}/%{name}
#!/bin/bash
exec mono %{_prefix}/lib/%{name}/manos.exe "\$@"
EOF
chmod +x %{buildroot}%{_bindir}/%{name}
# layout
install -d %{buildroot}%{_datadir}/%{name}/

%clean
rm -rf %{buildroot}

%changelog