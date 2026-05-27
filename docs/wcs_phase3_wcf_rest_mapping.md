# WCS Phase 3 WCF to REST Mapping

## Goal

This document maps legacy WCF contracts to the unified REST surface.

The purpose is not to preserve WCF forever. The purpose is to make the replacement path explicit and safe:

1. every important legacy method gets a REST target
2. compatibility can be retained temporarily where needed
3. decommissioning decisions can be made endpoint by endpoint

## Migration rule

Use the following default rule:

- if the capability already exists in `ZHQXC/WebAPI`, migrate callers to the unified REST route first
- if the capability does not exist yet, add it to the unified module before retiring WCF
- keep WCF only as a compatibility facade during consumer migration

## Task services

### `ITaskServiceForClient`

Source file:

- `v1.2/WcfServices/WcsServiceForClient/ITaskServiceForClient.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `GetTasks()` | `GET /api/v1/operations/tasks` | replace directly |
| `GetTaskDetails(taskNo)` | `GET /api/v1/operations/tasks/{taskCode}` | replace directly |
| `SuspendTask(taskNo)` | `POST /api/v1/operations/tasks/{taskCode}/suspend` | replace directly |
| `CompleteTask(id)` | `POST /api/v1/operations/tasks/by-id/{taskId}/complete` | replace directly |
| `CompleteMovenent(id)` | `POST /api/v1/operations/movements/{movementId}/complete` | replace directly |
| `CompleteAction(id)` | `POST /api/v1/operations/actions/{actionId}/complete` | replace directly |
| `CancelTask(taskNo)` | `POST /api/v1/operations/tasks/{taskCode}/cancel` | replace directly |
| `ResumeTask(taskNo)` | `POST /api/v1/operations/tasks/{taskCode}/resume` | replace directly |
| `ResumeTaskWithCurrentLocation(taskNo, currentLocation)` | `POST /api/v1/operations/tasks/{taskCode}/resume-at-location` | replace directly |

### `IWcsTaskService`

Source file:

- `v1.2/TaskService/IWcsTaskService.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `ArchiveFromTaskCode(taskCode)` | `POST /api/v1/operations/tasks/{taskCode}/archive` | replace directly |
| `ArchiveFromTaskId(taskId)` | `POST /api/v1/operations/tasks/by-id/{taskId}/archive` | replace directly |
| `SuspendTaskFromTaskCdoe(taskCode)` | `POST /api/v1/operations/tasks/{taskCode}/suspend` | merge with task module |
| `CompleteTaskFromTaskId(taskId)` | `POST /api/v1/operations/tasks/by-id/{taskId}/complete` | merge with task module |
| `CompleteMovenentFromLogicMovementId(logicMovementId)` | `POST /api/v1/operations/movements/{movementId}/complete` | merge with movement module |
| `CompleteActionFromActionId(actionId)` | `POST /api/v1/operations/actions/{actionId}/complete` | merge with action module |
| `CancelTaskFromTaskCode(taskCode)` | `POST /api/v1/operations/tasks/{taskCode}/cancel` | merge with task module |
| `CancelTaskFromTaskId(taskId)` | `POST /api/v1/operations/tasks/by-id/{taskId}/cancel` | merge with task module |
| `CancelMovementFromLogicMovementId(logicMovementId)` | `POST /api/v1/operations/movements/{movementId}/cancel` | merge with movement module |
| `CancelActionFromActionId(actionId)` | `POST /api/v1/operations/actions/{actionId}/cancel` | merge with action module |
| `ResumeTaskFromTaskCodeWithCurrentLocation(taskCode, currentLocation)` | `POST /api/v1/operations/tasks/{taskCode}/resume-at-location` | merge with task module |

## Device services

### `IDeviceServiceForClient`

Source file:

- `v1.2/WcfServices/WcsServiceForClient/IDeviceServiceForClient.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `GetStatusByName(deviceName)` | `GET /api/v1/operations/devices/{deviceName}` | replace directly |
| `GetAllStatus()` | `GET /api/v1/operations/devices` | replace directly |

### `ICraneServiceForClient`

Source file:

- `v1.2/WcfServices/WcsServiceForClient/ICraneServiceForClient.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `GetStatusByName(deviceName)` | `GET /api/v1/operations/cranes/{deviceName}` | replace directly |
| `GetAllStatus()` | `GET /api/v1/operations/cranes` | replace directly |

### `IConveyorServiceForClient`

Source file:

- `v1.2/WcfServices/WcsServiceForClient/IConveyorServiceForClient.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `GetAlarms(deviceName)` | `GET /api/v1/monitoring/conveyors/{deviceName}/alarms` | move to monitoring |
| `GetHoldSignals(deviceName)` | `GET /api/v1/operations/conveyors/{deviceName}/hold-signals` | keep operational |
| `GetLocationStatus(deviceName)` | `GET /api/v1/operations/conveyors/{deviceName}/locations/status` | replace directly |
| `GetLocationTasks(deviceName)` | `GET /api/v1/operations/conveyors/{deviceName}/locations/tasks` | replace directly |
| `GetOccupys(deviceName)` | `GET /api/v1/operations/conveyors/{deviceName}/occupancies` | replace directly |
| `GetTasks(deviceName)` | `GET /api/v1/operations/conveyors/{deviceName}/tasks` | replace directly |
| `ClearTask(deviceName, equipmentTaskId)` | `POST /api/v1/operations/conveyors/{deviceName}/tasks/{equipmentTaskId}/clear` | keep as imperative action |
| `DeleteTask(deviceName, equipmentTaskId)` | `DELETE /api/v1/operations/conveyors/{deviceName}/tasks/{equipmentTaskId}` | replace directly |
| `DeleteLocationTask(deviceName, posNo, equipmentTaskId)` | `DELETE /api/v1/operations/conveyors/{deviceName}/locations/{posNo}/tasks/{equipmentTaskId}` | replace directly |

## Crane manual service

### `ICraneService`

Source file:

- `v1.2/WcfServices/CraneService/SingleForkCrane/ICraneService.cs`

This interface is broader than `ICraneServiceForClient` and should be migrated with a compatibility window.

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `LoadCraneInfos()` | `GET /api/v1/operations/cranes/metadata` | add unified equivalent |
| `ReadStatus()` | `GET /api/v1/operations/cranes/status` | add unified equivalent |
| `BackToTheOrigin(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/back-to-origin` | compatibility-first |
| `EmergencyStop(craneName)` | `POST /api/v1/operations/cranes/{craneName}/emergency-stop` | compatibility-first |
| `CancelEmergencyStop(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/cancel-emergency-stop` | compatibility-first |
| `Move(craneName, ..., userColumn, userLevel)` | `POST /api/v1/operations/cranes/{craneName}/move` | compatibility-first |
| `Pick(craneName, ..., forkDirection)` | `POST /api/v1/operations/cranes/{craneName}/pick` | compatibility-first |
| `Putdown(craneName, ..., forkDirection)` | `POST /api/v1/operations/cranes/{craneName}/putdown` | compatibility-first |
| `Lock(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/lock` | align with device lock rules |
| `Unlock(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/unlock` | align with device lock rules |
| `Up(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/up` | compatibility-first |
| `Down(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/down` | compatibility-first |
| `Forward(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/forward` | compatibility-first |
| `Back(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/back` | compatibility-first |
| `ResetWarn(craneName, ...)` | `POST /api/v1/operations/cranes/{craneName}/reset-alarm` | compatibility-first |
| `MoveByForkDirection(craneName, ..., forkDirection)` | `POST /api/v1/operations/cranes/{craneName}/move-by-fork-direction` | compatibility-first |

## WMS service

### `IWcfWcsServiceForWms`

Source file:

- `v1.2/WcfServices/WcsServiceForWms/IWcfWcsServiceForWms.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `SendTask(task)` | `POST /api/v1/integrations/wms/equipment-task-request` | replace directly, keep compatibility window if external WMS still calls WCF |

## Alarm and reporting service

### `IWarningReportService`

Source file:

- `v1.2/WcfServices/WarningReportService/IWarningReportService.cs`

| WCF method | Unified REST target | Handling |
| --- | --- | --- |
| `Find(...)` | `GET /api/v1/monitoring/alarms/history` | add unified equivalent |
| `Add(record)` | `POST /api/v1/monitoring/alarms/history` | compatibility-only unless still needed |
| `Update(record)` | `PUT /api/v1/monitoring/alarms/history/{id}` | compatibility-only unless still needed |
| `Delete(id)` | `DELETE /api/v1/monitoring/alarms/history/{id}` | compatibility-only unless still needed |
| `Get(id)` | `GET /api/v1/monitoring/alarms/history/{id}` | add unified equivalent |
| `Login(userName, password)` | do not migrate as-is | move to unified auth strategy instead |
| `Report_CountByWarningType(...)` | `GET /api/v1/monitoring/reports/warning-type` | add unified equivalent |
| `Report_CountByDeviceName(...)` | `GET /api/v1/monitoring/reports/device-warning-count` | add unified equivalent |
| `Report_CountByRepaird(...)` | `GET /api/v1/monitoring/reports/repair-ratio` | add unified equivalent |
| `Report_CountByFault(...)` | `GET /api/v1/monitoring/reports/fault-ratio` | add unified equivalent |
| `Report_TrendByMonth(...)` | `GET /api/v1/monitoring/reports/trend/monthly` | add unified equivalent |
| `Report_TrendByYear(...)` | `GET /api/v1/monitoring/reports/trend/yearly` | add unified equivalent |
| `Report_FailureRate(...)` | `GET /api/v1/monitoring/reports/failure-rate` | add unified equivalent |

## Decommission priority

Suggested retirement order after REST replacement is available:

1. `ITaskServiceForClient`
2. `IWcsTaskService`
3. `IDeviceServiceForClient`
4. `IConveyorServiceForClient`
5. `ICraneServiceForClient`
6. `IWcfWcsServiceForWms`
7. `ICraneService`
8. `IWarningReportService`

The ordering is based on current overlap with active `ZHQXC/WebAPI` and the expected B/S migration value.

## Recommended next implementation step

The next executable Phase 3 step should be:

1. create shared response DTOs for `/api/v1`
2. implement `operations/tasks` and `operations/devices` first
3. add compatibility wrappers from existing `WCSController` actions
4. only then start retiring WCF consumers one group at a time
