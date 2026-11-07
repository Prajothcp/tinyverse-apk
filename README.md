# TINYVERSE — Pure C# (.NET 8 + Godot 4 C#) Zero-Install Cloud APK Builder

This package compiles **100% Native C# (`.cs` + `.csproj` on Microsoft .NET 8.0)** directly into a native Android ARM64 `.apk` (`Tinyverse-CSharp-ARM64.apk`) in the cloud using **GitHub Actions**—with **0 MB installed on your device** and **zero license keys required**!

---

## Included C# Source Architecture
- `TinyverseCSharp.csproj` — Official Microsoft `.NET 8.0` C# project definition.
- `Scripts/Core/Vector3d.cs` — 64-bit `double` precision 3D math struct for jitter-free cosmic scales.
- `Scripts/Core/TinyverseCSharpMain.cs` — Full C# 3D cosmic engine with 64-bit floating-origin camera, GPU `MultiMesh` instanced galaxies, procedural 3D Simplex noise planets, relativistic black holes, orbital physics, and mobile multi-touch + desktop controls.
- `.github/workflows/build-csharp-apk.yml` — Automated cloud compiler workflow that installs `.NET 8.0`, `JDK 17`, `Android SDK`, and `Godot 4.3 .NET (C#)` on Ubuntu cloud servers and compiles your `.apk`.

---

## How to Compile Your Pure C# `.apk` in the Cloud (3 Steps)

1. **Create a Free GitHub Repository**:
   - Go to [github.com/new](https://github.com/new), name it `tinyverse-csharp-apk`, and click **Create repository**.
2. **Upload the Extracted Files**:
   - Extract `Tinyverse_Pure_CSharp_Cloud_APK_Builder.zip` (make sure the `.github/workflows/build-csharp-apk.yml` folder is included!) and upload all files to your repository.
3. **Download `Tinyverse-CSharp-ARM64.apk`**:
   - Open the **Actions** tab in your GitHub repository.
   - Wait ~2 minutes for **"Build Pure C# Tinyverse Android APK (.NET 8 + Godot 4 C#)"** to finish with a green checkmark (✅).
   - Scroll down to **Artifacts** and click **`Tinyverse-Pure-CSharp-Android-APK`** to download your compiled C# Android `.apk`!
