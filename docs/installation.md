Installing Manos
================

Currently Manos can only be installed on Linux. Packages are provided for OpenSUSE and Ubuntu.

Dependencies
------------

Manos's only only dependency is Mono 2.8 or higher. Mono is available from http://mono-project.com/


Files
-----

A properly installed Manos should install the following files:

<prefix>/lib/Manos.dll
<prefix>/lib/manos.exe
<prefix>/lib/pkgconfig/manos.pc
<prefix>/bin/manos
<prefix>/share/manos/docs/<documentation files>
<prefix>/share/manos/layouts/default/<the default layout files for new apps>


Layouts
-------

Layouts are the files that are copied to your new application directory when you use the manos -init command.


Installing from source
----------------------

To build Manos from source you must run configure and make from the top level directory. Installation should be as simple as:

   ./configure
   sudo make install

The configure script also supports changing the installation prefix:

    ./configure --prefix=/tmp/install

To verify your installation you can use the manos -docs command.  This will create a new server running on http://localhost:8181/ you should be able to navigate there in your browser and view the manos documentation.

