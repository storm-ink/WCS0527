# WCS Phase 5 下线矩阵 v1

## 1. 目的与判断口径

本矩阵用于回答 Phase 5 的三个核心问题：

1. 先剔除谁
2. 保留谁
3. 谁绝对不能动

本版矩阵基于当前站点运行证据判断，核心依据包括：

- `艾芬达6号库WCS/WCS.MinimalRuntime.sln`
- `艾芬达6号库WCS/ZHQXC/系统配置/基本配置/startups.xml`
- `艾芬达6号库WCS/ZHQXC/Wcs.App.exe.config`
- `艾芬达6号库WCS/ZHQXC/系统配置/基本配置/settings.xml`
- `艾芬达6号库WCS/v1.2/WCS.DEVAPP/frmStarting.cs`
- `艾芬达6号库WCS/v1.2/Plugins/ServiceHosting/ServiceHostingPlugin.cs`
- `docs/wcs_bs_migration_plan.md`
- `docs/wcs_minimal_runtime.md`
- `docs/wcs_phase3_api_inventory.md`

### 状态定义

| 状态 | 含义 |
| --- | --- |
| 绝对不能动 | 当前站点主运行链路或最小运行集成员，现阶段不得剔除 |
| 过渡保留 | 属于旧链路，但在新宿主 / 新接口 / 新前端完全接管前不建议下线 |
| 高风险后移 | 表面像 UI/工具，实际与权限、配置、后台线程、WCF 宿主或业务逻辑耦合，必须先抽逻辑再动 |
| 先从 full solution 剔除 | 可先从 `v1.2/Wcs框架.sln` 或部署清单中收口，源码暂不物理删除 |
| 待现场确认 | 当前仓库看不到足够证据判断是否仍被现场依赖，需要补部署/现场核对 |

### 使用原则

- “先剔除”默认指 **先从 full solution / 部署清单 / 默认构建链路剔除**，不是立刻删除源码。
- 任何项目在未核对 `startups.xml`、`Wcs.App.exe.config`、`settings.xml`、插件扫描和现场部署目录前，不做物理删除。
- 当前默认运行收口目标应继续以 `WCS.MinimalRuntime.sln` 为准。

## 2. 绝对不能动

这些工程已经进入当前最小运行集，或直接被站点配置和启动链引用，现阶段不得剔除。

| 工程 | 当前判断 | 建议动作 | 主要依据 |
| --- | --- | --- | --- |
| `ZHQXC/ZHQXC.csproj` | 绝对不能动 | 保留 | 站点业务主工程；`webApiAssemblies` 加载 `ZHQXC.dll`；`startups.xml` 中多个启动项指向 `ZHQXC` |
| `v1.2/Wcs/Wcs.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；核心基础层 |
| `v1.2/Wcs.Framework/Wcs.Framework.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；`DefaultApplicationStartup`、配置与调度核心 |
| `v1.2/Wcs.FrameworkExtend/Wcs.FrameworkExtend.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；`PreTaskHandStartup` 在 `startups.xml` 启用 |
| `v1.2/WcsConsoleApplication/WcsConsoleApplication.csproj` | 绝对不能动 | 保留 | 当前非 UI 主宿主候选；`docs/wcs_minimal_runtime.md` 指定为最小运行入口 |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.AGV/Wcs.DefaultImplementCollection.AGV.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；站点配置存在 AGV 子系统与 NHibernate mapping |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Business/Wcs.DefaultImplementCollection.Business.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；`BusinessAboutStartUp` 在 `startups.xml` 启用 |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Conveyor/Wcs.DefaultImplementCollection.Conveyor.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；站点配置启用输送线设备、位置与 handler |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Crane/Wcs.DefaultImplementCollection.Crane.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；站点配置启用堆垛机设备、位置与恢复启动项 |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Scanner/Wcs.DefaultImplementCollection.Scanner.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；站点配置启用扫码器 |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.Robot/Wcs.DefaultImplementCollection.Robot.csproj` | 绝对不能动 | 保留 | `WCS.MinimalRuntime.sln` 成员；站点配置启用机械手与位置文件 |

## 3. 过渡保留

这些工程属于旧宿主 / 旧接口链路，但在“新宿主 + 统一 API + Web 前端”没有完全接管前，不建议立即下线。

| 工程 | 当前判断 | 建议动作 | 主要依据 |
| --- | --- | --- | --- |
| `v1.2/WcfServices/WcsServiceForWms/WcsServiceForWms.csproj` | 过渡保留 | 先保留，等 WMS 全量迁到统一 HTTP 后再下线 | `docs/wcs_phase3_api_inventory.md` 将其标为被 `WCSForWMSWebApiController` 替代的兼容面 |
| `v1.2/WcfServices/WcsServiceForClient/WcsServiceForClient.csproj` | 过渡保留 | 先保留，待 `operations/*` 全量替代 | 与 `WCSController`/统一 `operations` 面高度重叠 |
| `v1.2/WcfServices/WcfForSubSystemConveyorOccupySingleServer/WcfForSubSystemConveyorOccupySingleServer.csproj` | 过渡保留 | 先保留 | 属于旧 WCF 服务族，暂无现场切换完成证据 |
| `v1.2/WcfServices/RemoteSupportService/RemoteSupportService.csproj` | 过渡保留 | 先保留 | 旧远程支持服务；仓库中仍存在完整工程 |
| `v1.2/WcfServices/WarningReportService/WarningReportService.csproj` | 过渡保留 | 先保留，等 `/monitoring/alarms/*` 完整替代后再下线 | `docs/wcs_phase3_api_inventory.md` 明确其未来落点应是 monitoring 模块 |
| `v1.2/WcfServices/CraneService/CraneService.csproj` | 过渡保留 | 先保留 | 与未来 `/operations/cranes/*` 能力相关，不能先于替代接口下线 |
| `v1.2/TaskService/WcsTaskService.csproj` | 过渡保留 | 先保留 | `IWcsTaskService` 属于明确待替代的任务服务面 |
| `v1.2/WCS.DEVAPP/WCS.APP.csproj` | 过渡保留 | 最后期下线 | `frmStarting.cs` 会扫描并加载插件，是当前桌面宿主总入口 |

## 4. 高风险后移

这些工程不能简单视为“纯 UI”。它们与权限、运行时设置、WCF 宿主、后台线程或现场操作逻辑耦合，必须先抽逻辑/做替代再动。

| 工程 | 当前判断 | 建议动作 | 主要依据 |
| --- | --- | --- | --- |
| `v1.2/HomePage/HomePage.csproj` | 高风险后移 | 先保留，待首页与运行态引用完全替代后再下线 | `ZHQXC.csproj` 直接引用 `HomePage.dll`；仓库中仍有 `HomePageHelper` 相关引用 |
| `v1.2/OnlineLogDiagnosis/OnlineLogDiagnosis.csproj` | 高风险后移 | 先保留 | `settings.xml` 的 `WcsPermissionDlls` 仍显式列出 `OnlineLogDiagnosis.dll` |
| `v1.2/Plugins/ArchivedPreTaskManager/ArchivedPreTaskManager.csproj` | 高风险后移 | 先保留 | `settings.xml` 的 `WcsPermissionDlls` 仍显式列出该 DLL；前端尚无归档预任务页 |
| `v1.2/Plugins/ArchivedTaskManager/ArchivedTaskManager.csproj` | 高风险后移 | 先保留 | `settings.xml` 的 `WcsPermissionDlls` 仍显式列出该 DLL；前端尚无归档任务页 |
| `v1.2/Plugins/AuthorityManager/AuthorityManager.csproj` | 高风险后移 | 先保留 | `settings.xml` 的 `WcsPermissionDlls` 明确列出该插件；桌面权限链仍依赖 |
| `v1.2/Plugins/CranePriorToLoadLocation/CranePriorToLoadLocation.csproj` | 高风险后移 | 先保留 | 该插件不仅是界面，仍带后台线程/运行态行为 |
| `v1.2/Plugins/DeviceManager/DeviceManager.csproj` | 高风险后移 | 先保留，待 Web 设备能力完全覆盖再下线 | 当前前端只完成一期设备列表与锁定/解锁 |
| `v1.2/Plugins/EquipmentActionSchedulerManager/EquipmentActionSchedulerManager.csproj` | 高风险后移 | 先保留 | 属于设备调度运维能力，尚无 Web 替代证据 |
| `v1.2/Plugins/EventsManager/EventsManager.csproj` | 高风险后移 | 先保留 | 事件查看/干预能力尚未迁到浏览器 |
| `v1.2/Plugins/ManualTask/ManualTask.csproj` | 高风险后移 | 先保留，待 Web 手工任务功能完全覆盖再下线 | Web 前端仅完成一期最小字段集 |
| `v1.2/Plugins/MessageBoard/MessageBoard.csproj` | 高风险后移 | 先保留 | `settings.xml` 仍包含 `MessageBoard` 配置段；前端报警/消息页尚未完成 |
| `v1.2/Plugins/OutTaskFirst/OutTaskFirst.csproj` | 高风险后移 | 先保留 | 该插件影响运行态设置“出库优先”，不是纯展示 |
| `v1.2/Plugins/PreTaskManager/PreTaskManager.csproj` | 高风险后移 | 先保留 | 预任务运维链路尚未迁到浏览器 |
| `v1.2/Plugins/ProxyManager/ProxyManager.csproj` | 高风险后移 | 先保留 | 与代理/设备显示控制相关，缺少 Web 替代 |
| `v1.2/Plugins/RequestManager/RequestManager.csproj` | 高风险后移 | 先保留 | 请求处理类运维页面，尚无浏览器替代 |
| `v1.2/Plugins/ServiceHosting/ServiceHosting.csproj` | 高风险后移 | 先保留，等 WCF 全量下线后一并移除 | `ServiceHostingPlugin.cs` 会读取 `WcfServiceCollection` 并主动启动 WCF host |
| `v1.2/Plugins/TaskManager/TaskManager.csproj` | 高风险后移 | 先保留，待 Web 任务运维能力完全覆盖再下线 | 当前 Web 任务页尚未覆盖所有桌面能力 |
| `v1.2/Plugins/TaskManager_2/TaskManager.csproj` | 高风险后移 | 先保留 | 属于任务管理变体页面，仍需功能核对 |
| `v1.2/Plugins/Tools/Tools.csproj` | 高风险后移 | 先保留 | `settings.xml` 的 `frmRouteTestExtendButtons` 仍指向 `Plugins.Tools.frmRoutesSetting` |
| `v1.2/PreTaskManager/PreTaskManager.csproj` | 高风险后移 | 先保留 | 独立预任务工具，功能仍需映射 |
| `v1.2/TaskManager/TaskManager.csproj` | 高风险后移 | 先保留 | 独立任务工具，功能仍需映射 |

## 5. 先从 full solution 剔除

这些工程不在当前最小运行集内，也没有进入站点配置主链路。建议优先从 `v1.2/Wcs框架.sln`、默认构建和部署候选中剔除，但暂不物理删除源码。

| 工程 | 当前判断 | 建议动作 | 主要依据 |
| --- | --- | --- | --- |
| `v1.2/Client/Wcs.Client/Wcs.Client.csproj` | 先从 full solution 剔除 | 剔除 | 旧客户端，不在最小运行集 |
| `v1.2/Client/Wcs.Client.App/Wcs.Client.App.csproj` | 先从 full solution 剔除 | 剔除 | 旧客户端外壳，不在最小运行集 |
| `v1.2/WebUI/WebUI.csproj` | 先从 full solution 剔除 | 剔除 | 旧 MVC Web 工程，已被新前端方案取代 |
| `v1.2/Wcs.WebClient/Wcs.WebClient.csproj` | 先从 full solution 剔除 | 剔除 | 旧 Web 监控端，已被新前端方案取代 |
| `v1.2/WEBAPI/WEBAPI.csproj` | 先从 full solution 剔除 | 剔除 | 非当前站点 active API anchor；当前应以 `ZHQXC/WebAPI` 为准 |
| `v1.2/Wcs.WebApi/Wcs.WebApi.csproj` | 先从 full solution 剔除 | 剔除 | 非当前站点 active API anchor；属于重复 HTTP 基础设施 |
| `v1.2/Wcs.DefaultImplementCollection/Wcs.DefaultImplementCollection.RailGuidedVehicle/Wcs.DefaultImplementCollection.RailGuidedVehicle.csproj` | 先从 full solution 剔除 | 对当前站点剔除 | 当前站点配置与最小运行集未见启用该设备实现 |
| `v1.2/Plugins/ConveyorLocation/ConveyorLocation.csproj` | 先从 full solution 剔除 | 剔除 | 低频维护页面，不在最小运行主链 |
| `v1.2/Plugins/CraneManualClient/CraneManualClient.csproj` | 先从 full solution 剔除 | 剔除 | 低频人工调试页面，不在最小运行主链 |
| `v1.2/Plugins/CraneTest/CraneTest.csproj` | 先从 full solution 剔除 | 剔除 | 明确测试用途；`settings.xml` 虽列权限 DLL，但不应进入未来主链部署 |
| `v1.2/Plugins/Debugger/Debugger.csproj` | 先从 full solution 剔除 | 剔除 | 明确调试用途 |
| `v1.2/Plugins/FileLogViewer/FileLogViewer.csproj` | 先从 full solution 剔除 | 剔除 | 日志查看工具，不属于生产最小链路 |
| `v1.2/Plugins/LogTrace/LogTrace.csproj` | 先从 full solution 剔除 | 剔除 | 调试/日志工具 |
| `v1.2/Plugins/LogsViewer/LogsViewer.csproj` | 先从 full solution 剔除 | 剔除 | 调试/日志工具 |
| `v1.2/Plugins/RackLocation/RackLocation.csproj` | 先从 full solution 剔除 | 剔除 | 低频维护页面，不在最小运行主链 |
| `v1.2/Plugins/SystemInfo/SystemInfo.csproj` | 先从 full solution 剔除 | 剔除 | 系统信息工具，不在最小运行主链 |
| `v1.2/Plugins/TwoForksCraneManualClient/TwoForksCraneManualClient.csproj` | 先从 full solution 剔除 | 剔除 | 专项手动调试页面 |
| `v1.2/Plugins/TwoForksRackLocations/TwoForksRackLocations.csproj` | 先从 full solution 剔除 | 剔除 | 专项货位页面 |
| `v1.2/Plugins/TwoForksTaskViewer/TwoForksTaskViewer.csproj` | 先从 full solution 剔除 | 剔除 | 专项任务查看页面 |
| `v1.2/RemoteDiagnose/Client/Client/Client.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Client/Client.App/Client.App.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Client/Client.WebUI/Client.WebUI.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Common/Matedata/Matedata.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Proxy/Proxy.App/Proxy.App.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/DeviceServer.Services/ConveyorDeviceService/ConveyorDeviceService/ConveyorDeviceService.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/DeviceServer.Services/RailGuidedVehicleDeviceService/RailGuidedVehicleDeviceService.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/DeviceServer.Services/SingleForkCraneDeviceService/SingleForkCraneDeviceService.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/MatedataServer/MatedataServer.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/MatedataServer.App/MatedataServer.App.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/MatedataServer.Services/ConveyorService/ConveyorService.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/MatedataServer.Services/RailGuidedVehicleService/RailGuidedVehicleService.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/MatedataServer.Services/SingleForkCraneService/SingleForkCraneService.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |
| `v1.2/RemoteDiagnose/Server/MatedataServer.Services/WcsLog/WcsLog.csproj` | 先从 full solution 剔除 | 剔除 | 远程诊断独立链路，不在最小运行集 |

## 6. 待现场确认

这些工程从仓库层面无法直接证明“已经完全无用”或“必须保留”，需要补现场部署、硬件清单或许可证核对。

| 工程 | 当前判断 | 建议动作 | 主要依据 |
| --- | --- | --- | --- |
| `v1.2/Aga.Controls/Aga.Controls.csproj` | 待现场确认 | 随依赖桌面项目一并评估 | 属于 UI 组件库，是否还能删取决于哪些桌面项目保留 |
| `v1.2/BaseCommunication/S7/S7Net40.csproj` | 待现场确认 | 补硬件/协议依赖核对 | 迁移计划中明确为“按需保留” |
| `v1.2/DeviceEventQueueSettings/DeviceEventQueueSettings.csproj` | 待现场确认 | 补现场使用核对 | 迁移计划中明确为“视现场决定” |
| `v1.2/LEDServices/LEDServices.csproj` | 待现场确认 | 补现场硬件核对 | 迁移计划中明确为“视现场决定” |
| `v1.2/Plugins/DataAnalysis/DataAnalysis.csproj` | 待现场确认 | 核对是否仍有业务价值 | 迁移计划中明确为“视现场决定” |
| `v1.2/Plugins/Reports/Reports.csproj` | 待现场确认 | 核对是否仍有业务价值 | 迁移计划中明确为“视现场决定” |
| `v1.2/Plugins/WarningRecord/WarningRecord.csproj` | 待现场确认 | 等报警/历史报表替代方案明确后再判断 | 现阶段 monitoring 历史能力尚未补齐 |
| `v1.2/Plugins/故障统计/CraneTaskWarnings.csproj` | 待现场确认 | 等统计/报表替代方案明确后再判断 | 现阶段统计/报表页尚未完成 |
| `v1.2/WCSValidity/WCSValidity.csproj` | 待现场确认 | 先核对授权流程 | 与授权/有效期链路相关 |
| `v1.2/WCSValidityHelper/WCSValidityHelper.csproj` | 待现场确认 | 先核对授权流程，不要先删 | `ZHQXC.csproj` 仍打包 `WCSValidityHelper.dll` |
| `v1.2/WMSServices/WMSServices.csproj` | 待现场确认 | 核对是否仍有部署/调用方 | 迁移计划将其列为过渡保留，不能无证据下线 |

## 7. 推荐下线波次

建议按下面顺序推进，而不是一次性大删：

### Wave 0：先冻结主链路

先明确以下项目为“当前默认保留集”：

- `ZHQXC`
- `Wcs`
- `Wcs.Framework`
- `Wcs.FrameworkExtend`
- `WcsConsoleApplication`
- `Wcs.DefaultImplementCollection.{AGV,Business,Conveyor,Crane,Scanner,Robot}`

### Wave 1：先从 full solution 剔除明显重复/旁路项目

优先剔除：

- `Client/*`
- `WebUI`
- `Wcs.WebClient`
- `WEBAPI`
- `Wcs.WebApi`
- `RemoteDiagnose/*`
- 明确调试/测试/专项手动页面

### Wave 2：做旧桌面插件的替代关系表

重点核对并建立替代关系：

- `ManualTask`
- `TaskManager`
- `PreTaskManager`
- `DeviceManager`
- `MessageBoard`
- `AuthorityManager`
- `Tools`

### Wave 3：新链路稳定后再动 WCF 与桌面宿主

最后再处理：

- `WcfServices/*`
- `TaskService`
- `WCS.DEVAPP`

## 8. 本版结论

### 可以先剔除的

- `Client/*`
- `WebUI`
- `Wcs.WebClient`
- `WEBAPI`
- `Wcs.WebApi`
- `RemoteDiagnose/*`
- 调试/测试/专项手动插件

### 需要保留的

- 最小运行集 11 个核心工程
- 旧 WCF 服务与 `WCS.DEVAPP`（过渡期）
- 一批仍带业务逻辑或权限/设置耦合的插件

### 绝对不能动的

- `ZHQXC`
- `Wcs`
- `Wcs.Framework`
- `Wcs.FrameworkExtend`
- `WcsConsoleApplication`
- 当前站点已启用的 6 个设备实现工程

## 9. 下一步建议

基于本矩阵，下一步最合适的动作是：

1. 先出一版 `full solution` 收口清单，只做“剔除 solution 项”，不删源码。
2. 同步产出一版“桌面插件 -> Web/API 替代关系表”。
3. 对 `待现场确认` 项补一轮部署目录 / 现场硬件 / 实际使用核对。
4. 在 Phase 4 和 Phase 3 收尾后，再进入真正的物理下线阶段。
