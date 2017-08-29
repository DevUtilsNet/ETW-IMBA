@echo off
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /t:build ..\DevUtils.ETWIMBA.sln /p:Configuration=Release
if errorlevel 1 (
   echo Failure Reason Given is %errorlevel%
   exit /b %errorlevel%
)

"NuGet.exe" Pack "DevUtils.ETWIMBA.nuspec"