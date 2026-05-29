# WCS Phase 5 桌面插件到 Web/API 替代关系表 v1

## 1. 目的

本表用于回答 Phase 5 Wave 2 的核心问题：

1. 哪些桌面插件已经有 Web/API 替代
2. 哪些只替代了一部分
3. 哪些当前还没有替代，不能动
4. 哪些不适合直接迁成普通运维页面，需要单独策略

本表是 `docs/wcs_phase5_retirement_matrix.md` 的补充文档，用于指导下一批高风险插件的收口顺序。

## 2. 当前替代基线

当前已经落地的 Web 前端范围只有：

- `/dashboard`
- `/tasks`
- `/devices`
- `/manual-task`

当前已落地的 unified API 重点范围只有：

- `/api/v1/operations/tasks/*`
- `/api/v1/operations/devices/*`
- `/api/v1/operations/movements/*`
- `/api/v1/operations/actions/*`

因此，当前 Phase 5 Wave 2 的判断必须基于“前端一期 + operations API 一期”的真实现状，而不能按目标蓝图假定已经替代完成。

## 3. 状态定义

| 状态 | 含义 |
| --- | --- |
| 已替代 | 当前 Web 前端和统一 API 已可覆盖原插件主功能 |
| 部分替代 | 主链路已存在，但仍缺关键字段、关键动作或关键视图 |
| 未替代 | 当前没有对应 Web 页面或统一 API 能力 |
| 需单独策略 | 不适合简单迁移成普通运维页面，往往涉及权限中心、工程工具或后台管理模块 |

## 4. 插件替代关系表

| 插件 | 原桌面功能 | 当前 Web/API 替代 | 当前状态 | 仍缺什么 | Phase 5 建议 |
| --- | --- | --- | --- | --- | --- |
| `v1.2/Plugins/ManualTask` | 手工创建运行任务与计划任务，带更多业务字段与可达性检查 | Web `/manual-task` + `POST /api/v1/operations/tasks` | 部分替代 | 计划任务模式、更多字段、位置选择与到达校验 | 先保留 |
| `v1.2/Plugins/TaskManager` | 当前任务查询、干预、详情、优先级调整、粒度更细的完成操作 | Web `/tasks` + `operations/tasks` + `operations/movements/actions` | 部分替代 | 优先级修改、movement/action 级别按钮、恢复路线提示、事件驱动刷新 | 先保留 |
| `v1.2/Plugins/PreTaskManager` | 计划任务列表与干预 | 仅 legacy `API/WCSWebApi/*`，无新 Web 页面 | 未替代 | 整个 pre-task operator UI 与统一 API | 不能动 |
| `v1.2/Plugins/DeviceManager` | 设备树、连接/断开、锁定/解锁、设备状态面板 | Web `/devices` + `operations/devices` | 部分替代 | connect/disconnect、设备类型树、设备专用状态页 | 先保留 |
| `v1.2/Plugins/MessageBoard` | 实时消息/报警看板 | 仅有未来 `monitoring/*` 方向，当前前端无对应页 | 未替代 | 报警/消息统一语义、前端 monitoring 页 | 不能动 |
| `v1.2/Plugins/AuthorityManager` | 登录、用户、角色、密码、桌面权限体系 | 无 | 需单独策略 | Web 权限中心、登录态、RBAC | 单独立项，不纳入普通页面替代 |
| `v1.2/Plugins/Tools` | 路径工具、批量归档、扩展配置入口 | 仅 per-task archive 已有，其余无 | 需单独策略 | 路径测试工具、批量归档策略页、工程设置入口 | 拆成工程工具/后台模块 |
| `v1.2/Plugins/ArchivedTaskManager` | 历史任务查询、导出、详情 | 无 | 未替代 | 历史任务 API、历史页、导出 | 不能动 |
| `v1.2/Plugins/ArchivedPreTaskManager` | 历史计划任务查询 | 无 | 未替代 | 历史 pre-task API 与页面 | 不能动 |
| `v1.2/Plugins/RequestManager` | Request 队列查询与人工归档/删除 | 无 | 未替代 | request list/archive API 与页面 | 不能动 |
| `v1.2/Plugins/EquipmentActionSchedulerManager` | 设备动作调度器、状态机与方法跟踪 | 仅任务详情时间线只读展示 | 未替代 | 调度器面板、状态机可视化、方法跟踪 | 不能动 |
| `v1.2/Plugins/EventsManager` | 延迟事件队列查看、弹出、归档 | 无 | 未替代 | event queue API 与页面 | 不能动 |

## 5. 逐项说明

### 5.1 `ManualTask`

当前前端已经有 `手工任务` 页面，可以提交运行任务，且网关已对接 `POST /api/v1/operations/tasks`。  
但桌面插件同时支持运行任务与计划任务两条路径，还带业务字段、到达性判断和更多配置项，因此当前只能算“部分替代”。

### 5.2 `TaskManager`

当前前端的 `任务列表` 已经覆盖了：

- 列表查询
- 基础筛选
- 暂停 / 继续 / 取消 / 强制完成 / 归档
- movement/action 时间线只读展示

但桌面 `TaskManager` 仍有这些能力尚未替代：

- 优先级修改
- movement/action 级别的人工操作入口
- 更完整的任务详情页
- 更强的实时推送体验

所以当前仍是“部分替代”，不能直接下线。

### 5.3 `PreTaskManager`

计划任务仍停留在 legacy 控制器路径，当前既没有新前端页面，也没有新的 `/api/v1/operations/pre-tasks/*` 统一 API。  
因此它是“未替代”，目前不能动。

### 5.4 `DeviceManager`

当前前端 `设备监控` 已经覆盖了：

- 设备列表
- 搜索
- 锁定 / 解锁

但桌面插件还包含：

- 连接 / 断开
- 按类型树状组织
- 设备专用状态面板

因此只算“部分替代”。

### 5.5 `MessageBoard`

当前前端总览页还没有实时消息/报警板块。  
虽然统一 API 规划里已经预留 `monitoring/*` 模块，但实际前后端都还没完成，因此 `MessageBoard` 仍是“未替代”。

### 5.6 `AuthorityManager`

它不是普通运维页面，而是权限中心。  
后续应单独做 Web 登录、角色、用户、权限模型，而不是简单照搬桌面插件页面，所以归类为“需单独策略”。

### 5.7 `Tools`

`Tools` 里混合了：

- 路径测试/工程工具
- 批量归档工具
- 站口/路径设置扩展入口

这类功能不适合直接归进普通运维首页，建议拆为：

- 工程工具页
- 后台管理页
- 配置中心

因此归类为“需单独策略”。

### 5.8 `ArchivedTaskManager` / `ArchivedPreTaskManager`

这两个插件都属于“历史查询面”。  
当前前端虽然可以执行“归档”动作，但并没有“历史任务 / 历史计划任务”的浏览页面，因此都仍是“未替代”。

### 5.9 `RequestManager`

当前前后端都没有 request 队列管理能力，仍是空白，所以是“未替代”。

### 5.10 `EquipmentActionSchedulerManager`

当前前端虽然能看到 movement / action 时间线，但还看不到：

- 设备动作调度队列
- 状态机
- 方法跟踪

因此只能判断为“未替代”。

### 5.11 `EventsManager`

当前前后端都没有 event queue 查看与人工处理能力，所以仍是“未替代”。

## 6. 当前结论

### 6.1 当前最接近可下线的插件

最接近但仍未完全可下线的是：

- `ManualTask`
- `TaskManager`
- `DeviceManager`

它们的主链路已经有了 Web/API 承接，只是还不完整。

### 6.2 当前绝对不能动的 Wave 2 插件

以下插件当前还没有可接受替代，不应继续下线：

- `PreTaskManager`
- `MessageBoard`
- `ArchivedTaskManager`
- `ArchivedPreTaskManager`
- `RequestManager`
- `EquipmentActionSchedulerManager`
- `EventsManager`

### 6.3 当前需要单独开题的插件

这些插件不适合直接做成一期普通运维页面：

- `AuthorityManager`
- `Tools`

## 7. 基于本表的下一步建议

建议按下面顺序推进：

1. 先补 `ManualTask / TaskManager / DeviceManager` 的缺口，争取把这 3 个插件推进到“可下线”状态。
2. 单独设计 `PreTask` 与 `History` 两条能力线，不要混进当前一期运维页。
3. 单独补 `monitoring/*` 与对应前端页面，用来接住 `MessageBoard`。
4. 将 `AuthorityManager` 和 `Tools` 拆成独立后台/工程工具方案。
5. 在这之后，再评估第二批高风险插件是否可从 full solution 或部署清单继续收口。

## 8. 与 Phase 5 矩阵的关系

本表与 `docs/wcs_phase5_retirement_matrix.md` 的关系如下：

- 下线矩阵负责判断“项目能不能动”
- 替代关系表负责判断“为什么现在还不能动，缺口在哪”

后续若继续推进 Phase 5，高风险插件是否可继续剔除，应以本表为直接依据。
