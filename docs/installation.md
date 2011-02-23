Installing Manos
================

Currently Manos can be installed on Linux, OS X and Windows.

You should read the whole guide before reading your operating system specific instructions.


Dependencies
------------

Manos requires the unmanaged libev library. Libev should be available in a packages format from your distro or homebrew for OS X users. Windows users will have to download and compile libev.

Libev can be found here: <http://software.schmorp.de/pkg/libev.html>

Files
-----

A properly installed Manos on OS X and Linux should install the following files:

    <prefix>/lib/Manos.dll
    <prefix>/lib/manos.exe
    <prefix>/lib/pkgconfig/manos.pc
    <prefix>/bin/manos
    <prefix>/share/manos/docs/<documentation files>
    <prefix>/share/manos/layouts/default/<the default layout files for new apps>

On windows all files will be installed in the same directory:

    <prefix>/Manos.dll
    <prefix>/manos.exe
    <prefix>/docs/<documentation files>
    <prefix>/layouts/default/<the default layout files>


Layouts
-------

Layouts are the files that are copied to your new application directory when you use the manos --init command.


Installing from source
----------------------

To build Manos from source you must run configure and make from the top level directory. Installation should be as simple as:

   ./configure
   sudo make install

The configure script also supports changing the installation prefix:

    ./configure --prefix=/tmp/install

To verify your installation you can use the manos --docs command.  This will create a new server running on http://localhost:8181/ you should be able to navigate there in your browser and view the manos documentation.



Installing Manos on OS X
------------------------

### Install Mono 2.8

Grab the Mono 2.8 OSX package from the [Mono Downloads Page](http://mono-project.com/Downloads).

You need to have Mono 2.8 installed on your system. An older Mono install wont cut it. Also, if you've install Mono from source on your Mac, things might work, things might not work.  This guide assumes you have it installed from packages.



### Install Manos

Now that all the dependencies are installed you should be able to build and install Manos.

    git clone https://jacksonh@github.com/jacksonh/manos.git
    ./autogen.sh
    make
    sudo make install

This will install Manos.dll and manos.exe into /usr/local/lib/manos. As well as a .pc file and a manos script for invoking manos.exe

### Confirm your installation

You should now be able to run the manos documentation server:

    manos --docs

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

     MONO_LOG_MASK=dll MONO_LOG_LEVEL=debug manos --docs




Installation on Linux
---------------------

### Install Mono 2.8

Follow the distro specific instructions on <http://www.mono-project.com/download> to install Mono 2.8.

Once Mono 2.8 is installed you can verify your installation by typing mono on the command line:

    jackson@erm:~$ mono --version
    Mono JIT compiler version 2.8 (mono-2-8/57dae7a Mon Oct  4 18:24:09 EDT 2010)


### Install Manos

Checkout Manos from github at <http://github.com/jacksonh/manos/> and build/install it:

    jackson@erm:manos$ ./autogen.sh
    ...
    jackson@erm:manos$ make && sudo make install
    ...

To verify your installation you can use the manos --docs command.  This will create a new server running on
http://localhost:8181/ you should be able to navigate there in your browser and view the manos documentation.



Installation on OpenBSD
-----------------------

### Install prerequisites
From ports or packages install mono 2.8

### Install Manos
Checkout Manos from github at <http://github.com/jacksonh/manos/> and build/install it:

    $ ./autogen.sh
    ...
    $ su
    ...
    $ gmake install
    ...

To verify your installation you can use the manos --docs command.  This will create a new server running on
http://localhost:8181/ you should be able to navigate there in your browser and view the manos documentation.



Installation on Windows
---------------------

### Install Mono 2.8 (Optional)

Follow the Windows specific instructions on <http://www.mono-project.com/download> to install Mono 2.8.
This step is optional but you will need the Mono.Posix.dll and PosixHelper.dll when you want to compile
a single Manos dll for both Windows and Linux/MacOS.

### Install libev

Download the libev package from: <http://software.schmorp.de/pkg/libev.html>
Extract it and place it next to the libev in the source dir, so both libev-4.00 and libev are in the same directory.

### Install Manos

Checkout Manos from github at <http://github.com/jacksonh/manos/> and build/install it using Visual Studio.

If you do no have Mono installed you can define DISABLE_POSIX to remove the Mono.Posix dependency.

### Copy all files to your install directory

Create a directory for the Manos binary files and copy Manos.dll manos.exe and the entire manos/data/layouts
directory to your new directory.  Your structure should look something like this:

    C:\Program Files\Manos\manos.exe
    C:\Program Files\Manos\Manos.dll
    C:\Program Files\layouts\default\<layout files>

