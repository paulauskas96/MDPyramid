C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc /out:%~dp0Program.exe %~dp0Program.cs

@echo off

if errorlevel 1 (
    pause
    exit
)

start %~dp0/Program.exe