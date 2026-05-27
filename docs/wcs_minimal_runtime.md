# WCS Minimal Runtime

## Goal

This document defines the first executable delivery for the WCS slimming work:

- one minimal solution for core runtime work
- one console host as the primary non-UI entry point
- one validation script for repeatable host checks

The purpose is to give follow-up refactors a stable runtime baseline before more projects are removed.

## Included projects

`艾芬达6号库WCS/WCS.MinimalRuntime.sln` keeps the smallest project set that is still aligned with the current site runtime model:

- `v1.2/Wcs`
- `v1.2/Wcs.Framework`
- `v1.2/Wcs.FrameworkExtend`
- `v1.2/WcsConsoleApplication`
- `ZHQXC`
- `Wcs.DefaultImplementCollection.AGV`
- `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Business`
- `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Conveyor`
- `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Crane`
- `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Scanner`
- `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Robot`

## Excluded projects

The minimal solution intentionally excludes these groups:

- desktop UI hosts such as `WCS.DEVAPP`
- legacy clients under `v1.2/Client`
- WCF service projects
- old MVC/Web UI projects
- remote diagnose projects
- plugin-only operations UI
- simulation, debug, and test tools

They are not part of the first runtime convergence target.

## Primary runtime entry

Use `v1.2/WcsConsoleApplication` as the current primary runtime host candidate.

It now supports:

- `--config <path>` for an external config file
- `--base-directory <path>` for deployment-style runtime directories
- `--validate-config` for loading config without starting schedulers
- `--no-wait` for non-blocking start

## Validation script

Use `scripts/validate_wcs_host.sh` for repeatable checks.

Examples:

- rebuild host
  - `bash scripts/validate_wcs_host.sh build`
- inspect host CLI
  - `bash scripts/validate_wcs_host.sh help`
- validate external config
  - `bash scripts/validate_wcs_host.sh validate`

## Current validation status

The host build and CLI entry are working in the current environment.

Validation against `ZHQXC/Wcs.App.exe.config` now reaches the real external site config path and deployment base directory. The remaining failure is inside site-specific startup type resolution in the current local validation environment, not in argument parsing, config switching, or `xmlFile` path expansion.

## Next recommended steps

1. make `WCS.MinimalRuntime.sln` the default solution for runtime-side refactors
2. continue moving startup/runtime verification to `WcsConsoleApplication`
3. reduce binary `HintPath` dependencies inside `ZHQXC` where project references are preferable
4. split API-facing work from UI/plugin-facing work on top of the minimal runtime baseline
