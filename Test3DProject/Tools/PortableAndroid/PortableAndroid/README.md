# Portable Android Unity Toolkit (No Admin)

This toolkit sets up Android build tools in your user-writable folder and adds a Unity Editor menu helper to any project.

## What it gives you

- Portable JDK + Android SDK + NDK + CMake under your user profile
- Repeatable setup for lab machines with no admin rights
- One Unity menu click per project to point Unity at the portable toolchain

## 1) Install portable Android tools once per machine profile

Run in PowerShell:

```powershell
./Tools/PortableAndroid/Setup-PortableAndroidTools.ps1
```

Default install root:

- C:/Users/<you>/AppData/Local/Development/AndroidTools

Installed packages:

- platform-tools
- platforms;android-36
- build-tools;36.0.0
- ndk;27.2.12479018
- cmake;3.22.1

If PowerShell blocks script execution, run this once in your user scope:

```powershell
Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
```

## 2) Add Unity helper to each project

Run for each Unity project folder:

```powershell
./Tools/PortableAndroid/Add-PortableAndroidUnityHelper.ps1 -ProjectPath "C:/Path/To/Project"
```

This creates:

- Assets/Editor/PortableAndroidTools.cs

## 3) Use in Unity

Inside the target Unity project:

1. Wait for script compilation.
2. Click Lesson Tools > Use Portable Android Tools.
3. Build as normal using that project's own build menu/process.

## Optional: force re-download

If tools look corrupted, run:

```powershell
./Tools/PortableAndroid/Setup-PortableAndroidTools.ps1 -ForceDownload
```

## Notes for teaching labs

- Keep one shared process for all student projects.
- If USB ADB fails on locked-down PCs, use file-transfer install from Builds/*.apk.
- If phone says update invalid, uninstall previous app first, then install fresh.
