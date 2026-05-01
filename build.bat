@echo off
setlocal

set "OUT=bin\publish"
set "ZIP=jellyfin-plugin-prerolls-v1.0.0.zip"

echo.
echo =========================================
echo  Building Jellyfin Prerolls Plugin
echo  (Jellyfin 10.11 requires .NET 9 SDK)
echo =========================================
echo.

:: Build targeting net9.0 (required by Jellyfin 10.11)
dotnet publish Jellyfin.Plugin.Prerolls\Jellyfin.Plugin.Prerolls.csproj ^
    --configuration Release ^
    --framework net9.0 ^
    --output "%OUT%" ^
    -p:GenerateDocumentationFile=false
if errorlevel 1 (
    echo.
    echo BUILD FAILED.
    echo If you see "net9.0 is not installed", download .NET 9 SDK from:
    echo   https://dotnet.microsoft.com/download/dotnet/9.0
    exit /b 1
)

:: Zip just the DLL
cd "%OUT%"
powershell -Command "Compress-Archive -Path 'Jellyfin.Plugin.Prerolls.dll' -DestinationPath '..\..\%ZIP%' -Force"
cd ..\..

:: Print MD5 checksum so you can paste it into manifest.json
echo.
echo =========================================
echo  Build complete: %ZIP%
echo  MD5 checksum (paste into manifest.json):
echo =========================================
powershell -Command "(Get-FileHash '%ZIP%' -Algorithm MD5).Hash.ToLower()"
echo.
echo  Steps after this:
echo  1. Upload %ZIP% as a GitHub Release asset (tag: v1.0.0)
echo  2. Replace sourceUrl in manifest.json with the GitHub release download URL
echo  3. Replace checksum in manifest.json with the MD5 above
echo  4. Push manifest.json to your repo root
echo  5. In Jellyfin: Dashboard - Plugins - Repositories - + - paste:
echo     https://raw.githubusercontent.com/YOUR_USERNAME/jellyfin-plugin-prerolls/main/manifest.json
echo.
pause
