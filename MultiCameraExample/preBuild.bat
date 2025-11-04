@ECHO off
Echo start pre Build
REM Option to call this batch with parameters
IF "%~1"=="" (SET CONFIGURATION=Release) ELSE (SET CONFIGURATION=%~1)
REM Get all DLLs from pco_runtime
xcopy /Y /D /E  %~dp0..\..\bin\*.dll        %~dp0bin\%CONFIGURATION%\
xcopy /Y /D /E  %~dp0..\..\bin\genicam\*.dll     %~dp0bin\%CONFIGURATION%\genicam\
Echo finished pre Build