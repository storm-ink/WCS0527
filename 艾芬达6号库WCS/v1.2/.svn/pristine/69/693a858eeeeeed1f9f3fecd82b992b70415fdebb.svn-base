@echo off
call "%VS100COMNTOOLS%/../../VC/vcvarsall.bat"
set projectFile=%~dp0Wcs¿ò¼Ü.sln
devenv "%projectFile%" /clean
devenv "%projectFile%" /build Release

if not %errorlevel% == 0 echo %projectFile% failed!   Error: %errorlevel%
if %errorlevel% == 0 echo %projectFile% compiled successful
if %errorlevel% == 0 @start %~dp0Wcs.App\bin\Release
if not %errorlevel% == 0 pause