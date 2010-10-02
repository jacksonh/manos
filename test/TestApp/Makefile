
all: TestApp.dll TestRunner.exe

TestApp.dll: TestApp.cs StreamTestsModule.cs
	dmcs -pkg:manos -target:library -out:TestApp.dll TestApp.cs StreamTestsModule.cs

TestRunner.exe: TestRunner.cs StreamTests.cs
	dmcs -out:TestRunner.exe TestRunner.cs StreamTests.cs

clean:
	rm -f TestRunner.exe
	rm -f TestApp.dll