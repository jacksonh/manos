
all: TestApp.dll TestRunner.exe

TestApp.dll: TestApp.cs StreamTestsModule.cs HttpTestsModule.cs
	dmcs -pkg:manos -target:library -out:TestApp.dll TestApp.cs StreamTestsModule.cs HttpTestsModule.cs

TestRunner.exe: TestRunner.cs StreamTests.cs HttpTests.cs
	dmcs -out:TestRunner.exe TestRunner.cs StreamTests.cs HttpTests.cs 

clean:
	rm -f TestRunner.exe
	rm -f TestApp.dll