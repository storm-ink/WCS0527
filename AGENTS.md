# AGENTS.md

## Cursor Cloud specific instructions

This is a **WCS (Warehouse Control System)** project for 艾芬达6号库 (Aifenda Warehouse #6). It is a **.NET Framework 4.x Windows desktop application** built with WinForms and DevExpress controls.

### Technology Stack

| Component | Technology |
|-----------|------------|
| Language | C# (.NET Framework 4.0–4.8) |
| UI | WinForms + DevExpress v18.1 (commercial) |
| ORM | NHibernate 3.x, SqlSugar 5.x |
| Database | PostgreSQL (via Npgsql) |
| Communication | WCF, MQTT (MQTTnet), HslCommunication |
| Logging | NLog 2.1 |
| Build System | Visual Studio / MSBuild (old-style .csproj) |

### Project Structure

- `艾芬达6号库WCS/ZHQXC/` — Main application project (targets .NET 4.8)
- `艾芬达6号库WCS/v1.2/` — Framework libraries, plugins, services
- `艾芬达6号库WCS/v1.2/Wcs/` — Core WCS plugin library (.NET 4.0)
- `艾芬达6号库WCS/v1.2/Wcs.Framework/` — WCS framework (.NET 4.0)
- `艾芬达6号库WCS/v1.2/Wcs.FrameworkExtend/` — Framework extensions (.NET 4.0)
- `艾芬达6号库WCS/v1.2/WEBAPI/` — Web API (.NET 4.7.2)
- `艾芬达6号库WCS/v1.2/Plugins/` — Plugin modules

### Linux (Cloud Agent) Limitations

This project is **Windows-only** and cannot fully build or run on Linux because:

1. **WinForms + DevExpress** — GUI requires Windows desktop
2. **WCF** — Windows Communication Foundation is Windows-only in .NET Framework
3. **NLog 2.1** — Incompatible with Mono runtime (reflection issue with Console target)
4. **DevExpress commercial DLLs** — No HintPath in .csproj; rely on Windows GAC

### What Works on Linux with Mono

- **C# syntax verification** via `mcs` (Mono C# compiler) using pre-built DLLs in `bin/Debug/` as references
- **NuGet package restore** — Packages are pre-committed in `packages/` directories
- **Assembly inspection** — `monodis` can inspect pre-built `.exe`/`.dll` assemblies
- **Individual library compilation** — The `Wcs` core library compiles successfully with `mcs`

### How to Verify C# Code (on Linux)

```bash
cd /workspace/艾芬达6号库WCS/v1.2/Wcs
mcs -target:library -out:/tmp/Wcs_test.dll \
  -reference:bin/Debug/DevExpress.Utils.v18.1.dll \
  -reference:bin/Debug/DevExpress.XtraBars.v18.1.dll \
  -reference:bin/Debug/DevExpress.XtraEditors.v18.1.dll \
  -reference:bin/Debug/DevExpress.Data.v18.1.dll \
  -reference:../packages/Iesi.Collections.3.2.0.4000/lib/Net35/Iesi.Collections.dll \
  -reference:../packages/NHibernate.3.3.3.4001/lib/Net35/NHibernate.dll \
  -reference:../packages/NLog.2.1.0/lib/net40/NLog.dll \
  -reference:../packages/Newtonsoft.Json.6.0.3/lib/net40/Newtonsoft.Json.dll \
  -r:System.Windows.Forms -r:System.Drawing -r:System.Data \
  -r:System.Configuration -r:System.Runtime.Serialization -sdk:4.5 \
  -resource:obj/x86/Debug/Wcs.Authority.Role.hbm.xml \
  -resource:obj/x86/Debug/Wcs.Authority.User.hbm.xml \
  Extentions/*.cs LogWriteToFileHelper.cs MethodTrack/*.cs NhLogger*.cs \
  NHUnitOfWork/*.cs Properties/AssemblyInfo.cs ReflectionHelper.cs \
  Security/*.cs WcsPlugin/*.cs Utils.cs Authority/*.cs
```

### Full Build/Run Requirements (Windows)

- Windows 10/11 with Visual Studio 2019+ (or JetBrains Rider)
- DevExpress WinForms v18.1 installed (registered in GAC)
- PostgreSQL database server
- Open solution: `艾芬达6号库WCS/ZHQXC.sln`
- Build configuration: Debug | Any CPU
- Main executable: `WCS.APP.exe`

### Pre-built Binaries

The repository contains pre-built binaries in:
- `艾芬达6号库WCS/ZHQXC/bin/Debug/` — Full application with all DLLs
- `艾芬达6号库WCS/ZHQXC/WCS.APP.exe` — Main executable (also at root of ZHQXC)

These can be used as references for compilation verification on Linux.
