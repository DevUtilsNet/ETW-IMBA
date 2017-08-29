@echo off
msbuild ..\DevUtils.ETWIMBA.Build\DevUtils.ETWIMBA.Build.csproj
if errorlevel 1 (
   echo Failure Reason Given is %errorlevel%
   exit /b %errorlevel%
)
msbuild /p:IMBAEnabled=true DevUtils.ETWIMBA.Sample.csproj /preprocess > DevUtils.ETWIMBA.Sample.proj
if errorlevel 1 (
   echo Failure Reason Given is %errorlevel%
   exit /b %errorlevel%
)
msbuild @@MSBuildCommandLine.txt
