@ECHO OFF

REM Create a registry entry to add managed assemblies in the Add Reference Dialog box in VS.NET
echo.
echo reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Oracle.ManagedDataAccess" /ve /t REG_SZ /d "%~dp0managed\common" /f
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Oracle.ManagedDataAccess" /ve /t REG_SZ /d "%~dp0managed\common" /f
echo.
echo reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Oracle.ManagedDataAccess.EntityFramework6" /ve /t REG_SZ /d "%~dp0managed\common\EF6" /f
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Oracle.ManagedDataAccess.EntityFramework6" /ve /t REG_SZ /d "%~dp0managed\common\EF6" /f
