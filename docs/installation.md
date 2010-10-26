Installing Manos
================

Currently Manos can only be installed on Linux and OS X. Windows is not supported yet, but might be possible with a little bit of work.

You should read the whole guide before reading your operating system specific instructions.

Installing Manos on OS X

Installing Manos on Linux


Dependencies
------------

Manos's requires Mono 2.8 or higher. Mono is available from <http://mono-project.com/>

Mano's also requires the unmanaged libev library. Libev should be available in a packages format from your distro or homebrew for OS X users.

Libev can be found here: <http://software.schmorp.de/pkg/libev.html>

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



Installing Manos on OS X
------------------------

### Install Mono 2.8

Grab the Mono 2.8 OSX package from the [Mono Downloads Page](http://mono-project.com/Downloads).

You need to have Mono 2.8 installed on your system. An older Mono install wont cut it. Also, if you've install Mono from source on your Mac, things might work, things might not work.  This guide assumes you have it installed from packages.


### Install libev

libev is Manos's one native dependency.  I installed this guy using macports:

    sudo port install libev +universal

The key part of this is that we are installing the universal build of libev. If you leave that part out you could get a 64bit version and Mono won't be able to load it.

You should now have a libev.dylib in /opt/local/lib/ to make sure Mono knows where to find that library, update your DYLD_FALLBACK_LIBRARY_PATH.

    export DYLD_FALLBACK_LIBRARY_PATH=/opt/local/lib


### Install Manos

Now that all the dependencies are installed you should be able to build and install Manos.

    git clone https://jacksonh@github.com/jacksonh/manos.git
    cd manos
    ./configue
    make
    sudo make install

This will install Manos.dll and manos.exe into /usr/local/lib/manos. As well as a .pc file and a manos script for invoking manos.exe

### Confirm your installation

You should now be able to run the manos documentation server:

    manos -docs

and navigate to http://localhost:8181/ in your browser.  


### Trouble Shooting


1.  If you are getting a type load exception, make sure you have the universal libev installed:

    erp:~ jackson$ cd /opt/local/lib
    
    erp:lib jackson$ file libev.dylib 

        libev.dylib: symbolic link to libev.3.0.0.dylib



    erp:lib jackson$ file libev.3.0.0.dylib 
        
        libev.3.0.0.dylib: Mach-O universal binary with 2 architectures

        libev.3.0.0.dylib (for architecture x86_64):    Mach-O 64-bit dynamically linked shared library x86_64
        
        libev.3.0.0.dylib (for architecture i386):      Mach-O dynamically linked shared library i386


Another trick is to use Mono's logging to see where Mono is looking for libev.dylib

     MONO_LOG_MASK=dll MONO_LOG_LEVEL=debug manos -docs




Installation on Linux
---------------------

### Install Mono 2.8

Follow the distro specific instructions on <http://www.mono-project.com/download> to install Mono 2.8.

Once Mono 2.8 is installed you can verify your installation by typing mono on the command line:

    jackson@erm:~$ mono --version
    Mono JIT compiler version 2.8 (mono-2-8/57dae7a Mon Oct  4 18:24:09 EDT 2010)

### Install libev

You should install the native libev library from packages you can find OpenSuse packages for libev by
searching <http://software.opensuse.org>. Ubuntu users should install the 'libev-dev' package using
apt-get.

Verify that you have libev installed:

    jackson@erm:~$ ls /usr/lib/libev.so
    /usr/lib/libev.so

### Install Manos

Checkout Manos from github at <http://github.com/jacksonh/manos/> and build/install it:

    jackson@erm:manos$ ./configure
    ...
    jackson@erm:manos$ sudo make install
    ...

To verify your installation you can use the manos -docs command.  This will create a new server running on
http://localhost:8181/ you should be able to navigate there in your browser and view the manos documentation.

