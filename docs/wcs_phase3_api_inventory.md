# WCS Phase 3 API Inventory

## Goal

This document records the current interface surface that Phase 3 needs to unify.

It focuses on three questions:

1. which API entry points are active for `ZHQXC`
2. which WCF and legacy service contracts still overlap with those APIs
3. how the future unified API should be split for B/S migration

## Current active API anchor

The active API anchor for this site is `ZHQXC/WebAPI`.

It is started by `ZHQXC.WEBAPIStartUp` through the WCS startup chain and uses the `wcsWebApiAddressPort` setting from `settings.xml`. For the current site, that port is `9898`.

This is the API surface that should be kept and normalized during Phase 3.

## Active HTTP surface

### Host and startup

| Area | File | Role |
| --- | --- | --- |
| startup | `艾芬达6号库WCS/ZHQXC/启动程序/WEBAPIStartUp.cs` | site startup item that starts self-hosted Web API |
| host | `艾芬达6号库WCS/ZHQXC/WebAPI/Base/HttpService.cs` | self-hosted `HttpSelfHostServer` configuration |
| helper | `艾芬达6号库WCS/ZHQXC/WebAPI/Base/WebApiSelfHostHelper.cs` | start and stop helper |
| assembly resolver | `艾芬达6号库WCS/ZHQXC/WebAPI/Base/UserResolver.cs` | loads controller assemblies from `webApiAssemblies` |
| config | `艾芬达6号库WCS/ZHQXC/系统配置/基本配置/startups.xml` | includes `ZHQXC.WEBAPIStartUp, ZHQXC` |
| config | `艾芬达6号库WCS/ZHQXC/系统配置/基本配置/settings.xml` | exposes `wcsWebApiAddressPort` |

### Controllers

| Controller | Route shape | Main purpose |
| --- | --- | --- |
| `WCSController` | `API/WCSWebApi/*` | operations API for tasks, devices, manual intervention, crane control |
| `WCSForWMSWebApiController` | `API/WCSForWMSWebApi/*` | inbound WMS integration |
| `AgvApiController` | `API/Agv/*` and `api/Agv/*` | AGV task-status callback |
| `MessageController` | `api/Message/*` | active alarm message feed |
| `StatisticsController` | `api/Statistics/*` | dashboard statistics aggregation |
| `TestController` | `API/Test/test` | simple health or time probe |

## Active capability buckets

### 1. Operations and manual intervention

Primary controller: `WCSController`

Current route groups include:

- device list and running actions
- scheduler and state-machine introspection
- lock and unlock taskable devices
- live pre-task and task queries
- history task queries
- manual task creation
- task suspend, resume, cancel, complete, archive
- logic movement and equipment action force-complete or cancel
- task priority change
- single-fork crane remote hand operations

This controller is the strongest candidate for the future operator-facing unified API module.

### 2. WMS inbound integration

Primary controller: `WCSForWMSWebApiController`

Current active routes are:

- `EquipmentTaskRequest`
- `issue_non_compliant`
- `box_arrive_info`

These are already the main WMS-to-WCS HTTP entry points for this site and should become the future `/integrations/wms/*` boundary.

### 3. AGV inbound integration

Primary controller: `AgvApiController`

Current active route:

- `ReportTaskStatus`

This is the inbound AGV callback that updates WCS task execution state. It should become the future `/integrations/agv/*` inbound module.

### 4. Monitoring and dashboard

Primary controllers:

- `MessageController`
- `StatisticsController`

These controllers provide:

- active alarm list
- dashboard totals and chart-like aggregates

They should become the future `/monitoring/*` unified API module.

## Outbound integration surface

### WCS -> WMS

Primary helper: `艾芬达6号库WCS/ZHQXC/WebAPI/Interfaces/ReqeustWMSHelper.cs`

Active or implemented outbound calls include:

- `BoxSupply`
- `EquipmentTaskStatusChangeReport`
- `ScanCodeReporting`

The repo also contains DTOs and helper methods for additional WMS-facing calls that look contract-ready but need business confirmation before they are kept or removed.

### WCS -> AGV

Primary helper: `艾芬达6号库WCS/v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.AGV/Http/AgvHttpApiClient.cs`

Current outbound dispatch path is:

- `DispatchTask`

The AGV implementation is dual-channel today:

- HTTP dispatch through `AgvHttpApiClient`
- middle-table persistence through AGV support classes

This dual-channel behavior should be preserved behind a single unified AGV integration boundary instead of being exposed directly to callers.

### WCS -> PLC / conveyor / 专机

The PLC side is not currently a public HTTP API. It is mostly internal device communication through:

- device command classes
- conveyor device write calls
- startup handlers that bridge WMS events to PLC instructions

This should stay behind the internal device adapter layer and should not be promoted as a first-class external unified API unless there is a real integration consumer for it.

## Legacy overlap to be replaced

### WCF client services

The following legacy WCF contracts overlap heavily with the active `ZHQXC/WebAPI` surface:

- `v1.2/WcfServices/WcsServiceForClient/ITaskServiceForClient.cs`
- `v1.2/WcfServices/WcsServiceForClient/IDeviceServiceForClient.cs`
- `v1.2/WcfServices/WcsServiceForClient/IConveyorServiceForClient.cs`
- `v1.2/WcfServices/WcsServiceForClient/ICraneServiceForClient.cs`
- `v1.2/TaskService/IWcsTaskService.cs`

These should be treated as the main replacement set for the unified API.

### WCF WMS service

Legacy inbound WMS WCF contract:

- `v1.2/WcfServices/WcsServiceForWms/IWcfWcsServiceForWms.cs`

For this site, the active path has already shifted to `WCSForWMSWebApiController`, so this WCF service should be considered a compatibility or deprecation target instead of the future direction.

### Duplicate HTTP infrastructure

The repo still contains older HTTP host shells:

- `v1.2/WEBAPI`
- `v1.2/Wcs.WebApi`

They do not represent the active site runtime path and should not be used as the Phase 3 baseline.

## Unified API first draft boundary

Phase 3 should split the future unified API into four main surfaces:

| Unified module | Current source | Primary consumers |
| --- | --- | --- |
| `/operations/*` | `WCSController` | browser operator UI, admin tools |
| `/integrations/wms/*` | `WCSForWMSWebApiController` + `ReqeustWMSHelper` contract set | WMS |
| `/integrations/agv/*` | `AgvApiController` + `AgvHttpApiClient` contract set | AGV scheduler |
| `/monitoring/*` | `MessageController` + `StatisticsController` | dashboards and monitoring UI |

## Replacement map

| Legacy interface | Unified API target | Notes |
| --- | --- | --- |
| `ITaskServiceForClient` | `/operations/tasks/*` | mostly already represented in `WCSController` |
| `IWcsTaskService` | `/operations/tasks/*` | duplicate task-control surface |
| `IDeviceServiceForClient` | `/operations/devices/*` | merge into operations API |
| `IConveyorServiceForClient` | `/operations/devices/*` or `/monitoring/*` | split by command vs read-only concerns |
| `ICraneServiceForClient` / `ICraneService` | `/operations/cranes/*` | keep manual intervention semantics |
| `IWcfWcsServiceForWms` | `/integrations/wms/*` | active site already uses HTTP instead of WCF |
| `IWarningReportService` | `/monitoring/alarms/*` | needs live vs historical alarm split |

## Phase 3 next implementation steps

1. define a canonical route and response-envelope standard for the four unified modules
2. produce a WCF-to-REST replacement table method by method for client-facing legacy contracts
3. normalize WMS and AGV DTOs as stable integration contracts instead of controller-local models
4. move remaining legacy WMS call paths onto the same HTTP contract family
5. keep PLC and device protocol details behind internal domain or adapter services
