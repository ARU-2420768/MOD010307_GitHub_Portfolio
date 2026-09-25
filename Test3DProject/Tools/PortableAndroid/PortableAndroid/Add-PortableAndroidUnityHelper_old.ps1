param(
    [Parameter(Mandatory = $true)]
    [string]$ProjectPath,

    [string]$AndroidToolsRoot = "$env:LOCALAPPDATA\Development\AndroidTools",

    [string]$MenuPrefix = "Lesson Tools"
)

$ErrorActionPreference = "Stop"

$projectAssets = Join-Path $ProjectPath "Assets"
if (-not (Test-Path $projectAssets)) {
    throw "Unity project not found at $ProjectPath (missing Assets folder)."
}

$editorPath = Join-Path $projectAssets "Editor"
New-Item -ItemType Directory -Force -Path $editorPath | Out-Null

$target = Join-Path $editorPath "PortableAndroidTools.cs"

$normalizedRoot = $AndroidToolsRoot.Replace('\\', '/')
$menu = $MenuPrefix.Replace('"', '\\"')

$content = @"
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class PortableAndroidTools
{
    static readonly string[] CandidateRoots =
    {
        "$normalizedRoot",
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Development", "AndroidTools")
    };

    [MenuItem("$menu/Use Portable Android Tools")]
    public static void ConfigurePortableAndroidTools()
    {
        string root = ResolveRoot();
        string sdk = NormalizePath(Path.Combine(root, "SDK"));
        string jdk = NormalizePath(Path.Combine(root, "JDK"));
        string ndk = ResolveNdkPath(sdk);

        ValidatePath(sdk, "SDK");
        ValidatePath(jdk, "JDK");
        ValidatePath(ndk, "NDK");
        ValidatePath(NormalizePath(Path.Combine(sdk, "platform-tools")), "SDK platform-tools");
        ValidateFile(NormalizePath(Path.Combine(jdk, "bin", "java.exe")), "JDK java.exe");

        ApplyViaUnityAndroidApi("sdkRootPath", sdk);
        ApplyViaUnityAndroidApi("jdkRootPath", jdk);
        ApplyViaUnityAndroidApi("ndkRootPath", ndk);

        EditorPrefs.SetBool("JdkUseEmbedded", false);
        EditorPrefs.SetString("JdkPath", jdk);

        EditorPrefs.SetBool("AndroidSdkUseEmbedded", false);
        EditorPrefs.SetString("AndroidSdkRoot", sdk);

        EditorPrefs.SetBool("AndroidNdkUseEmbedded", false);
        EditorPrefs.SetString("AndroidNdkRoot", ndk);
        EditorPrefs.SetString("AndroidNdkRootR16b", ndk);
        EditorPrefs.SetString("AndroidNdkRootR19", ndk);
        EditorPrefs.SetString("AndroidNdkRootR21D", ndk);
        EditorPrefs.SetString("AndroidNdkRootR23", ndk);

        Environment.SetEnvironmentVariable("JAVA_HOME", jdk);
        Environment.SetEnvironmentVariable("ANDROID_SDK_ROOT", sdk);
        Environment.SetEnvironmentVariable("ANDROID_HOME", sdk);
        Environment.SetEnvironmentVariable("ANDROID_NDK_ROOT", ndk);

        Debug.Log($"Portable Android tools configured. Root={root} SDK={sdk} JDK={jdk} NDK={ndk}");
    }

    static void ValidatePath(string path, string label)
    {
        if (!Directory.Exists(path))
        {
            throw new InvalidOperationException($"Portable {label} not found at: {path}");
        }
    }

    static void ValidateFile(string path, string label)
    {
        if (!File.Exists(path))
        {
            throw new InvalidOperationException($"Portable {label} not found at: {path}");
        }
    }

    static string ResolveRoot()
    {
        foreach (var root in CandidateRoots)
        {
            if (Directory.Exists(root)) return root;
        }

        throw new InvalidOperationException(
            "AndroidTools root folder not found. Expected one of: " +
            string.Join(", ", CandidateRoots));
    }

    static string ResolveNdkPath(string sdk)
    {
        string pinned = NormalizePath(Path.Combine(sdk, "ndk", "27.2.12479018"));
        if (Directory.Exists(pinned)) return pinned;

        string ndkRoot = NormalizePath(Path.Combine(sdk, "ndk"));
        if (!Directory.Exists(ndkRoot))
        {
            throw new InvalidOperationException("NDK root not found at: " + ndkRoot);
        }

        var versions = Directory.GetDirectories(ndkRoot);
        if (versions.Length == 0)
        {
            throw new InvalidOperationException("No NDK versions found under: " + ndkRoot);
        }

        Array.Sort(versions, StringComparer.OrdinalIgnoreCase);
        return NormalizePath(versions[versions.Length - 1]);
    }

    static void ApplyViaUnityAndroidApi(string propertyName, string value)
    {
        var type = Type.GetType("UnityEditor.Android.AndroidExternalToolsSettings,UnityEditor.Android.Extensions");
        var prop = type?.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(null, value);
        }
    }

    static string NormalizePath(string path)
    {
        return path.Replace('\\', '/');
    }
}
"@

Set-Content -Path $target -Value $content -Encoding UTF8
Write-Host "Created: $target"
Write-Host "Open project in Unity, then click: $MenuPrefix > Use Portable Android Tools"
