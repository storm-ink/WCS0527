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

Validation against `ZHQXC/Wcs.App.exe.config` is now successful through the `WcsConsoleApplication` host. The validate-only path can:

- load the real external site config
- resolve site assemblies from both deployment and development output directories
- expand `xmlFile` references against the intended base directory
- parse startups, devices, locations, and routes without starting runtime schedulers or touching DB-backed runtime bootstrap

This means the minimal runtime baseline is now suitable as the execution anchor for Phase 3 API consolidation work.

## Next recommended steps

1. make `WCS.MinimalRuntime.sln` the default solution for runtime-side refactors
2. continue moving startup/runtime verification to `WcsConsoleApplication`
3. inventory and consolidate the active API surface around `ZHQXC/WebAPI`
4. reduce binary `HintPath` dependencies inside `ZHQXC` where project references are preferable
