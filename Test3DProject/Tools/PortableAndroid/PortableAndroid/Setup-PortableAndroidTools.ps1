param(
    [string]$Root = "$env:LOCALAPPDATA\Development\AndroidTools",
    [string]$CmdlineToolsUrl = "https://dl.google.com/android/repository/commandlinetools-win-13114758_latest.zip",
    [string]$JdkUrl = "https://api.adoptium.net/v3/binary/latest/17/ga/windows/x64/jdk/hotspot/normal/eclipse?project=jdk",
    [string]$Platform = "platforms;android-36",
    [string]$BuildTools = "build-tools;36.0.0",
    [string]$Ndk = "ndk;27.2.12479018",
    [string]$Cmake = "cmake;3.22.1",
    [switch]$ForceDownload
)

$ErrorActionPreference = "Stop"
$ProgressPreference = "SilentlyContinue"

function Ensure-Directory([string]$Path) {
    New-Item -ItemType Directory -Force -Path $Path | Out-Null
}

function Download-IfMissing([string]$Url, [string]$Destination, [switch]$Force) {
    if ((-not $Force) -and (Test-Path $Destination)) {
        Write-Host "Using cached download: $Destination"
        return
    }

    Write-Host "Downloading: $Url"
    Invoke-WebRequest -UseBasicParsing -Uri $Url -OutFile $Destination
}

function Expand-Fresh([string]$ZipPath, [string]$DestPath) {
    if (Test-Path $DestPath) {
        Remove-Item $DestPath -Recurse -Force
    }
    Expand-Archive -Path $ZipPath -DestinationPath $DestPath -Force
}

Ensure-Directory $Root

$sdkRoot = Join-Path $Root "SDK"
$jdkRoot = Join-Path $Root "JDK"
$tempRoot = Join-Path $Root "_tmp"

Ensure-Directory $sdkRoot
Ensure-Directory $jdkRoot
Ensure-Directory $tempRoot

$cmdZip = Join-Path $Root "commandlinetools.zip"
$jdkZip = Join-Path $Root "jdk17.zip"

Download-IfMissing -Url $CmdlineToolsUrl -Destination $cmdZip -Force:$ForceDownload
Download-IfMissing -Url $JdkUrl -Destination $jdkZip -Force:$ForceDownload

$cmdExtract = Join-Path $tempRoot "cmdline-tools-extract"
Expand-Fresh -ZipPath $cmdZip -DestPath $cmdExtract

$latest = Join-Path $sdkRoot "cmdline-tools\latest"
Ensure-Directory $latest
$cmdSource = Join-Path $cmdExtract "cmdline-tools"
Copy-Item (Join-Path $cmdSource "*") $latest -Recurse -Force

$jdkExtract = Join-Path $tempRoot "jdk-extract"
Expand-Fresh -ZipPath $jdkZip -DestPath $jdkExtract

$jdkTop = Get-ChildItem $jdkExtract -Directory | Select-Object -First 1
if (-not $jdkTop) {
    throw "JDK archive did not extract correctly."
}

Copy-Item (Join-Path $jdkTop.FullName "*") $jdkRoot -Recurse -Force

$env:JAVA_HOME = $jdkRoot
$env:PATH = "$jdkRoot\bin;$env:PATH"

$sdkManager = Join-Path $sdkRoot "cmdline-tools\latest\bin\sdkmanager.bat"
if (-not (Test-Path $sdkManager)) {
    throw "sdkmanager not found at $sdkManager"
}

# Accept all SDK licenses non-interactively.
$yes = (1..120 | ForEach-Object { "y" }) -join "`n"
$yes | & $sdkManager --sdk_root=$sdkRoot --licenses | Out-Host

$packages = @("platform-tools", $Platform, $BuildTools, $Ndk, $Cmake)
Write-Host "Installing packages: $($packages -join ', ')"
& $sdkManager --sdk_root=$sdkRoot @packages | Out-Host

Write-Host "Portable Android tools are ready."
Write-Host "Root: $Root"
Write-Host "SDK:  $sdkRoot"
Write-Host "JDK:  $jdkRoot"

$checks = @(
    (Join-Path $sdkRoot "platform-tools\adb.exe"),
    (Join-Path $sdkRoot "cmake\3.22.1"),
    (Join-Path $jdkRoot "bin\java.exe")
)

foreach ($check in $checks) {
    $exists = Test-Path $check
    Write-Host "$check => $exists"
}

Write-Host "Next step: run Add-PortableAndroidUnityHelper.ps1 for each Unity project."
