# WCS Phase 3 Unified API Specification

## Goal

This document turns the Phase 3 inventory into an implementation baseline.

It defines:

1. the approved API module split
2. route and response conventions
3. compatibility rules for replacing WCF and legacy HTTP surfaces
4. what belongs inside the internal device adapter layer

## Confirmed decisions

The current design baseline is:

- keep the four-module split
- use `ZHQXC/WebAPI` as the active API anchor
- include all currently active WMS and AGV contracts in the unified API scope
- keep PLC and device protocol details inside the internal adapter layer
- handle WCF replacement with a compatibility-first migration rather than a big-bang cutover

## Unified API modules

The future unified API is split into four modules.

| Module | Scope | Current source |
| --- | --- | --- |
| `/api/v1/operations/*` | operator actions, task management, device control, manual intervention | `WCSController` |
| `/api/v1/integrations/wms/*` | WMS inbound requests and WMS-facing contract stabilization | `WCSForWMSWebApiController` + `RequestWMSHelper` DTO set |
| `/api/v1/integrations/agv/*` | AGV inbound callback and AGV dispatch contract stabilization | `AgvApiController` + `AgvHttpApiClient` DTO set |
| `/api/v1/monitoring/*` | alarms, statistics, dashboard-oriented read APIs | `MessageController` + `StatisticsController` |

## Route conventions

### General rules

- use lowercase URL paths
- use plural resource names for collections
- prefer nouns over verbs in the path
- keep action-style verbs only where the operation is truly imperative and not a normal CRUD transition
- keep versioning at the top-level prefix with `/api/v1`

### Examples

| Current route | Unified route |
| --- | --- |
| `API/WCSWebApi/Tasks` | `/api/v1/operations/tasks/query` |
| `API/WCSWebApi/Task` | `/api/v1/operations/tasks/detail` |
| `API/WCSWebApi/AddTask` | `/api/v1/operations/tasks` |
| `API/WCSWebApi/SuspendTask` | `/api/v1/operations/tasks/{taskCode}/suspend` |
| `API/WCSWebApi/LockDevice` | `/api/v1/operations/devices/{deviceName}/lock` |
| `API/WCSForWMSWebApi/EquipmentTaskRequest` | `/api/v1/integrations/wms/equipment-task-request` |
| `API/Agv/ReportTaskStatus` | `/api/v1/integrations/agv/task-status-report` |
| `api/Message/Page` | `/api/v1/monitoring/alarms/active` |
| `api/Statistics/All` | `/api/v1/monitoring/dashboard/summary` |

## HTTP method conventions

Use these rules by default:

- `GET` for read-only list and detail queries
- `POST` for creation and integration ingress payloads
- `PUT` or `PATCH` for state transitions where idempotence matters
- `DELETE` only for true deletion semantics

Because current controllers are mostly `POST`-heavy and payload-driven, the first migration step may still keep some query endpoints on `POST`. That is acceptable during compatibility transition, but the target contract should prefer normal REST semantics where practical.

## Response envelope

The unified API should normalize all responses to one envelope.

Suggested envelope:

- `success`: boolean
- `code`: stable machine-readable code
- `message`: readable summary
- `data`: response payload
- `traceId`: optional request correlation id
- `errors`: optional structured validation or domain error list

Example semantics:

- success query: `success=true`, `code="ok"`
- validation error: `success=false`, `code="validation_error"`
- business rejection: `success=false`, `code="task_state_invalid"`
- integration error: `success=false`, `code="integration_upstream_error"`

## Error-code strategy

Error codes should be grouped by module:

- `ops_*` for operations
- `wms_*` for WMS integration
- `agv_*` for AGV integration
- `monitoring_*` for monitoring endpoints
- `common_*` for shared errors

Examples:

- `common_validation_error`
- `ops_task_not_found`
- `ops_device_locked`
- `wms_request_rejected`
- `agv_task_owner_invalid`

## Query and paging rules

For list endpoints:

- default to explicit paging support
- use `pageNo` and `pageSize`
- return `total` and `items`
- keep filter fields aligned with current business DTO vocabulary where possible

Suggested list response shape:

- `data.items`
- `data.total`
- `data.pageNo`
- `data.pageSize`

## Compatibility policy

### WCF migration

WCF should be replaced in two stages:

1. define the unified REST equivalent
2. keep the old WCF contract alive only as a compatibility facade while callers are migrated

This means WCF is not the long-term target, but it also does not need to be removed before the REST equivalent is available and consumers are switched.

### Legacy HTTP migration

Current `ZHQXC/WebAPI` routes should be treated as the first compatibility layer.

Recommended sequence:

1. introduce unified routes and normalized response envelopes
2. adapt old controller routes to call the new application services
3. mark old routes as compatibility endpoints
4. remove them after consumers are migrated

## Integration contract policy

### WMS

All active WMS contracts stay in scope.

That includes:

- inbound WMS request routes
- outbound WMS callback payloads
- DTOs already used by `RequestWMSHelper`

The unified API should not break current WMS semantics during the first migration pass.

### AGV

All active AGV contracts stay in scope.

That includes:

- inbound task-status callback
- outbound dispatch request contract

The current AGV implementation uses both HTTP and middle-table persistence. That dual implementation stays internal. External callers should only see the unified AGV contract.

## Internal adapter boundary

The following concerns stay behind the internal adapter layer and should not become first-class public API resources:

- PLC command blocks
- conveyor protocol objects
- raw device read and write packets
- AGV middle-table details
- internal NHibernate persistence shapes
- WCS event-bus wiring

The public API should expose business semantics, not transport- or device-level implementation details.

## Recommended next implementation step

After this document, the next concrete step should be:

1. define shared response DTOs and error codes
2. create the first unified routes for the `operations` module
3. wire old `WCSController` endpoints through those new application-layer contracts
4. then move WMS and AGV contracts onto the same conventions
