
all: TestApp.dll TestRunner.exe

TestApp.dll: TestApp.cs StreamTestsModule.cs HttpTestsModule.cs FileSystemTestsModule.cs
	dmcs -pkg:manos -target:library -out:TestApp.dll TestApp.cs StreamTestsModule.cs HttpTestsModule.cs FileSystemTestsModule.cs

TestRunner.exe: TestRunner.cs StreamTests.cs HttpTests.cs FileSystemTests.cs
	dmcs -out:TestRunner.exe TestRunner.cs StreamTests.cs HttpTests.cs FileSystemTests.cs

clean:
	rm -f TestRunner.exe
	rm -f TestApp.dll