import { useCallback, useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { NavLink, Navigate, Route, Routes } from 'react-router-dom'
import './App.css'
import {
  createManualTask,
  getGatewayConfig,
  listDevices,
  listTasks,
  mutateTask,
  setDeviceLock,
} from './services/wcsGateway'
import type {
  DashboardSnapshot,
  ManualTaskDraft,
  OperationsDevice,
  TaskStatus,
  WcsTask,
} from './types'

const gatewayConfig = getGatewayConfig()

const taskStatusLabel: Record<TaskStatus, string> = {
  New: '新任务',
  Sent: '已发送',
  Executing: '执行中',
  Suspend: '已暂停',
  Error: '异常',
  Cancelled: '已取消',
  Completed: '已完成',
}

function formatDate(value?: string) {
  if (!value) {
    return '--'
  }

  return new Intl.DateTimeFormat('zh-CN', {
    hour12: false,
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
  }).format(new Date(value))
}

function buildDashboard(tasks: WcsTask[], devices: OperationsDevice[]): DashboardSnapshot {
  return {
    totalTasks: tasks.length,
    executingTasks: tasks.filter((item) => item.status === 'Executing' || item.status === 'Sent').length,
    blockedTasks: tasks.filter((item) => item.status === 'Suspend' || item.status === 'Error').length,
    finishedTasks: tasks.filter((item) => item.status === 'Completed' || item.status === 'Cancelled').length,
    connectedDevices: devices.filter((item) => item.isConnected).length,
    lockedDevices: devices.filter((item) => item.isLocked).length,
    taskableDevices: devices.filter((item) => item.isTaskable).length,
  }
}

function App() {
  const [tasks, setTasks] = useState<WcsTask[]>([])
  const [devices, setDevices] = useState<OperationsDevice[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [notice, setNotice] = useState('')
  const [lastUpdatedAt, setLastUpdatedAt] = useState('')

  const refreshData = useCallback(async () => {
    try {
      const [nextTasks, nextDevices] = await Promise.all([listTasks(), listDevices()])
      setTasks(nextTasks)
      setDevices(nextDevices)
      setError('')
      setLastUpdatedAt(new Date().toISOString())
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : '数据加载失败')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    const kickoff = window.setTimeout(() => {
      void refreshData()
    }, 0)
    const timer = window.setInterval(() => {
      void refreshData()
    }, gatewayConfig.pollIntervalMs)

    return () => {
      window.clearTimeout(kickoff)
      window.clearInterval(timer)
    }
  }, [refreshData])

  const dashboard = useMemo(() => buildDashboard(tasks, devices), [tasks, devices])

  const handleTaskAction = useCallback(
    async (task: WcsTask, action: 'suspend' | 'cancel' | 'resume' | 'archive' | 'complete', currentUserCode?: string) => {
      try {
        await mutateTask(task, action, currentUserCode)
        await refreshData()
        setNotice(`任务 ${task.taskCode} 已完成 ${action} 操作`)
      } catch (requestError) {
        setError(requestError instanceof Error ? requestError.message : '任务操作失败')
      }
    },
    [refreshData],
  )

  const handleDeviceLock = useCallback(
    async (device: OperationsDevice, lockDevice: boolean) => {
      try {
        await setDeviceLock(device, lockDevice)
        await refreshData()
        setNotice(`${device.name} 已${lockDevice ? '锁定' : '解锁'}`)
      } catch (requestError) {
        setError(requestError instanceof Error ? requestError.message : '设备操作失败')
      }
    },
    [refreshData],
  )

  const handleCreateTask = useCallback(
    async (draft: ManualTaskDraft) => {
      try {
        const task = await createManualTask(draft)
        await refreshData()
        setNotice(`手工任务 ${task.taskCode} 已创建`)
      } catch (requestError) {
        setError(requestError instanceof Error ? requestError.message : '创建任务失败')
        throw requestError
      }
    },
    [refreshData],
  )

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="brand-card">
          <p className="brand-mark">WCS</p>
          <h1>Phase 4 Frontend</h1>
          <p className="brand-copy">Simple, structured, and ready for unified API migration.</p>
        </div>
        <nav className="nav">
          <NavLink to="/dashboard" className={({ isActive }) => (isActive ? 'active' : '')}>
            总览
          </NavLink>
          <NavLink to="/tasks" className={({ isActive }) => (isActive ? 'active' : '')}>
            任务列表
          </NavLink>
          <NavLink to="/devices" className={({ isActive }) => (isActive ? 'active' : '')}>
            设备监控
          </NavLink>
          <NavLink to="/manual-task" className={({ isActive }) => (isActive ? 'active' : '')}>
            手工任务
          </NavLink>
        </nav>
      </aside>

      <main className="content">
        <header className="topbar">
          <div>
            <p className="eyebrow">Phase 4 / Web UI</p>
            <h2>统一前端一期骨架</h2>
          </div>
          <div className="topbar-actions">
            <span className={`mode-badge mode-badge--${gatewayConfig.mode}`}>
              {gatewayConfig.mode === 'mock' ? 'Mock data' : 'Live API'}
            </span>
            <button type="button" className="ghost-button" onClick={() => void refreshData()}>
              立即刷新
            </button>
          </div>
        </header>

        <section className="meta-strip">
          <span>数据模式：{gatewayConfig.mode}</span>
          <span>轮询周期：{gatewayConfig.pollIntervalMs / 1000}s</span>
          <span>最后刷新：{formatDate(lastUpdatedAt)}</span>
        </section>

        {notice ? <section className="banner banner--success">{notice}</section> : null}
        {error ? <section className="banner banner--error">{error}</section> : null}

        <Routes>
          <Route
            path="/dashboard"
            element={<DashboardPage dashboard={dashboard} tasks={tasks} devices={devices} loading={loading} />}
          />
          <Route
            path="/tasks"
            element={<TasksPage tasks={tasks} loading={loading} onTaskAction={handleTaskAction} />}
          />
          <Route
            path="/devices"
            element={<DevicesPage devices={devices} loading={loading} onDeviceLock={handleDeviceLock} />}
          />
          <Route
            path="/manual-task"
            element={<ManualTaskPage loading={loading} onSubmit={handleCreateTask} />}
          />
          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </main>
    </div>
  )
}

function DashboardPage({
  dashboard,
  tasks,
  devices,
  loading,
}: {
  dashboard: DashboardSnapshot
  tasks: WcsTask[]
  devices: OperationsDevice[]
  loading: boolean
}) {
  const focusTasks = tasks.slice(0, 3)
  const focusDevices = devices.slice(0, 4)

  return (
    <section className="page">
      <div className="hero-panel">
        <div>
          <p className="eyebrow">Overview</p>
          <h3>统一运维首页</h3>
          <p className="hero-copy">
            一期优先聚焦任务、设备、手工任务三条主链路，保证页面简洁、规整、可扩展。
          </p>
        </div>
        <div className="hero-metrics">
          <MetricCard label="任务总数" value={dashboard.totalTasks} tone="blue" />
          <MetricCard label="执行中" value={dashboard.executingTasks} tone="green" />
          <MetricCard label="受阻任务" value={dashboard.blockedTasks} tone="amber" />
          <MetricCard label="在线设备" value={dashboard.connectedDevices} tone="blue" />
        </div>
      </div>

      <div className="panel-grid panel-grid--dashboard">
        <section className="panel">
          <div className="panel-head">
            <h4>关键指标</h4>
            <span>{loading ? '刷新中...' : '实时轮询'}</span>
          </div>
          <div className="stat-grid">
            <MetricCard label="已完成 / 已取消" value={dashboard.finishedTasks} tone="slate" />
            <MetricCard label="设备锁定数" value={dashboard.lockedDevices} tone="amber" />
            <MetricCard label="可调度设备" value={dashboard.taskableDevices} tone="green" />
            <MetricCard label="设备总数" value={devices.length} tone="slate" />
          </div>
        </section>

        <section className="panel">
          <div className="panel-head">
            <h4>重点任务</h4>
            <span>优先展示最新任务</span>
          </div>
          <div className="stack-list">
            {focusTasks.map((task) => (
              <article key={task.taskCode} className="stack-item">
                <div>
                  <strong>{task.taskCode}</strong>
                  <p>
                    {task.startLocation.userCode} → {task.endLocation.userCode}
                  </p>
                </div>
                <StatusPill kind={task.status} text={taskStatusLabel[task.status]} />
              </article>
            ))}
          </div>
        </section>

        <section className="panel">
          <div className="panel-head">
            <h4>重点设备</h4>
            <span>列表版监控</span>
          </div>
          <div className="stack-list">
            {focusDevices.map((device) => (
              <article key={device.name} className="stack-item">
                <div>
                  <strong>{device.name}</strong>
                  <p>{device.deviceType}</p>
                </div>
                <div className="stack-status">
                  <StatusPill kind={device.isConnected ? 'success' : 'danger'} text={device.isConnected ? '在线' : '离线'} />
                  {device.isLocked ? <StatusPill kind="warning" text="已锁定" /> : null}
                </div>
              </article>
            ))}
          </div>
        </section>
      </div>
    </section>
  )
}

function TasksPage({
  tasks,
  loading,
  onTaskAction,
}: {
  tasks: WcsTask[]
  loading: boolean
  onTaskAction: (task: WcsTask, action: 'suspend' | 'cancel' | 'resume' | 'archive' | 'complete', currentUserCode?: string) => Promise<void>
}) {
  const [keyword, setKeyword] = useState('')
  const [statusFilter, setStatusFilter] = useState<'all' | TaskStatus>('all')
  const [selectedTaskCode, setSelectedTaskCode] = useState('')
  const [resumeLocation, setResumeLocation] = useState('')

  const filteredTasks = useMemo(() => {
    return tasks.filter((task) => {
      const matchKeyword =
        keyword.trim() === '' ||
        task.taskCode.toLowerCase().includes(keyword.toLowerCase()) ||
        task.startLocation.userCode.toLowerCase().includes(keyword.toLowerCase()) ||
        task.endLocation.userCode.toLowerCase().includes(keyword.toLowerCase())

      const matchStatus = statusFilter === 'all' || task.status === statusFilter
      return matchKeyword && matchStatus
    })
  }, [keyword, statusFilter, tasks])

  const effectiveSelectedTaskCode =
    filteredTasks.length > 0 && filteredTasks.some((item) => item.taskCode === selectedTaskCode)
      ? selectedTaskCode
      : filteredTasks[0]?.taskCode ?? ''

  const selectedTask = filteredTasks.find((item) => item.taskCode === effectiveSelectedTaskCode)

  const handleAction = async (
    task: WcsTask,
    action: 'suspend' | 'cancel' | 'resume' | 'archive' | 'complete',
    currentUserCode?: string,
  ) => {
    await onTaskAction(task, action, currentUserCode)
    if (action === 'resume') {
      setResumeLocation('')
    }
  }

  return (
    <section className="page">
      <section className="page-header">
        <div>
          <p className="eyebrow">Operations</p>
          <h3>任务列表</h3>
        </div>
        <div className="toolbar">
          <input
            className="search-input"
            value={keyword}
            onChange={(event) => setKeyword(event.target.value)}
            placeholder="按任务号、起点、终点搜索"
          />
          <select className="filter-select" value={statusFilter} onChange={(event) => setStatusFilter(event.target.value as 'all' | TaskStatus)}>
            <option value="all">全部状态</option>
            {Object.entries(taskStatusLabel).map(([value, label]) => (
              <option key={value} value={value}>
                {label}
              </option>
            ))}
          </select>
        </div>
      </section>

      <div className="panel-grid panel-grid--tasks">
        <section className="panel">
          <div className="panel-head">
            <h4>任务总览</h4>
            <span>{loading ? '刷新中...' : `${filteredTasks.length} 条`}</span>
          </div>
          <div className="table-wrap">
            <table className="data-table">
              <thead>
                <tr>
                  <th>任务号</th>
                  <th>来源</th>
                  <th>起点</th>
                  <th>终点</th>
                  <th>状态</th>
                  <th>当前位</th>
                  <th>优先级</th>
                </tr>
              </thead>
              <tbody>
                {filteredTasks.map((task) => (
                  <tr
                    key={task.taskCode}
                    className={task.taskCode === effectiveSelectedTaskCode ? 'is-selected' : ''}
                    onClick={() => setSelectedTaskCode(task.taskCode)}
                  >
                    <td>{task.taskCode}</td>
                    <td>{task.source}</td>
                    <td>{task.startLocation.userCode}</td>
                    <td>{task.endLocation.userCode}</td>
                    <td>
                      <StatusPill kind={task.status} text={taskStatusLabel[task.status]} />
                    </td>
                    <td>{task.currentLocation.userCode}</td>
                    <td>{task.priority}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <section className="panel">
          <div className="panel-head">
            <h4>任务详情</h4>
            <span>{selectedTask ? selectedTask.taskCode : '未选择任务'}</span>
          </div>
          {selectedTask ? (
            <div className="detail-stack">
              <div className="detail-grid">
                <DetailItem label="任务来源" value={selectedTask.source} />
                <DetailItem label="任务类型" value={selectedTask.taskType || '--'} />
                <DetailItem label="创建时间" value={formatDate(selectedTask.createdAt)} />
                <DetailItem label="结束时间" value={formatDate(selectedTask.finishedAt)} />
                <DetailItem label="任务状态" value={taskStatusLabel[selectedTask.status]} />
                <DetailItem label="起点" value={selectedTask.startLocation.userCode} />
                <DetailItem label="终点" value={selectedTask.endLocation.userCode} />
                <DetailItem label="当前位置" value={selectedTask.currentLocation.userCode} />
                <DetailItem label="载具编码" value={selectedTask.containerCodes.join(', ') || '--'} />
              </div>

              <div className="task-actions">
                {selectedTask.status === 'New' || selectedTask.status === 'Executing' || selectedTask.status === 'Sent' ? (
                  <button type="button" onClick={() => void handleAction(selectedTask, 'suspend')}>
                    暂停
                  </button>
                ) : null}
                {selectedTask.status === 'Suspend' || selectedTask.status === 'Error' ? (
                  <>
                    <button type="button" onClick={() => void handleAction(selectedTask, 'cancel')}>
                      取消
                    </button>
                    <button type="button" onClick={() => void handleAction(selectedTask, 'complete')}>
                      强制完成
                    </button>
                  </>
                ) : null}
                {selectedTask.status === 'Completed' || selectedTask.status === 'Cancelled' ? (
                  <button type="button" onClick={() => void handleAction(selectedTask, 'archive')}>
                    归档
                  </button>
                ) : null}
              </div>

              {selectedTask.status === 'Suspend' || selectedTask.status === 'Error' ? (
                <div className="resume-box">
                  <input
                    className="search-input"
                    placeholder="继续执行时可选填当前位置"
                    value={resumeLocation}
                    onChange={(event) => setResumeLocation(event.target.value)}
                  />
                  <button
                    type="button"
                    className="primary-button"
                    onClick={() => void handleAction(selectedTask, 'resume', resumeLocation)}
                  >
                    继续执行
                  </button>
                </div>
              ) : null}

              <section className="timeline">
                <h5>Movements & Actions</h5>
                {selectedTask.movements.length === 0 ? (
                  <p className="empty-text">当前任务还没有 movement / action 明细。</p>
                ) : (
                  selectedTask.movements.map((movement) => (
                    <article key={movement.id} className="timeline-item">
                      <div className="timeline-title">
                        <strong>Movement #{movement.id}</strong>
                        <span>
                          {movement.startLocation.userCode} → {movement.endLocation.userCode}
                        </span>
                      </div>
                      <p className="timeline-meta">
                        设备：{movement.deviceName} / Route：{movement.routeId ?? '--'} / 状态：{movement.status}
                      </p>
                      <div className="timeline-actions">
                        {movement.equipmentActions.map((action) => (
                          <div key={action.id} className="timeline-chip">
                            <span>Action #{action.id}</span>
                            <small>{action.description}</small>
                          </div>
                        ))}
                      </div>
                    </article>
                  ))
                )}
              </section>
            </div>
          ) : (
            <p className="empty-text">请选择左侧任务查看详情。</p>
          )}
        </section>
      </div>
    </section>
  )
}

function DevicesPage({
  devices,
  loading,
  onDeviceLock,
}: {
  devices: OperationsDevice[]
  loading: boolean
  onDeviceLock: (device: OperationsDevice, lockDevice: boolean) => Promise<void>
}) {
  const [keyword, setKeyword] = useState('')

  const filteredDevices = useMemo(() => {
    return devices.filter((device) => {
      return (
        keyword.trim() === '' ||
        device.name.toLowerCase().includes(keyword.toLowerCase()) ||
        device.deviceType.toLowerCase().includes(keyword.toLowerCase())
      )
    })
  }, [devices, keyword])

  return (
    <section className="page">
      <section className="page-header">
        <div>
          <p className="eyebrow">Operations</p>
          <h3>设备监控</h3>
        </div>
        <div className="toolbar">
          <input
            className="search-input"
            value={keyword}
            onChange={(event) => setKeyword(event.target.value)}
            placeholder="按设备名或类型搜索"
          />
          <span className="toolbar-note">{loading ? '刷新中...' : `${filteredDevices.length} 台设备`}</span>
        </div>
      </section>

      <section className="device-grid">
        {filteredDevices.map((device) => (
          <article key={device.name} className="device-card">
            <div className="device-card__head">
              <div>
                <h4>{device.name}</h4>
                <p>{device.deviceType}</p>
              </div>
              <StatusPill kind={device.isConnected ? 'success' : 'danger'} text={device.isConnected ? '在线' : '离线'} />
            </div>

            <div className="device-card__meta">
              <DetailItem label="可调度" value={device.isTaskable ? '是' : '否'} />
              <DetailItem label="锁定状态" value={device.isLocked ? '已锁定' : '未锁定'} />
              <DetailItem label="锁定人" value={device.lockerUser || '--'} />
              <DetailItem label="锁定 IP" value={device.lockerIp || '--'} />
            </div>

            <div className="device-card__actions">
              <button
                type="button"
                className={device.isLocked ? 'ghost-button' : 'primary-button'}
                onClick={() => void onDeviceLock(device, !device.isLocked)}
              >
                {device.isLocked ? '解锁设备' : '锁定设备'}
              </button>
            </div>
          </article>
        ))}
      </section>
    </section>
  )
}

function ManualTaskPage({
  loading,
  onSubmit,
}: {
  loading: boolean
  onSubmit: (draft: ManualTaskDraft) => Promise<void>
}) {
  const [form, setForm] = useState<ManualTaskDraft>({
    taskCode: '',
    source: 'Manual',
    taskType: 'Inbound',
    description: '',
    startLocationCode: '',
    endLocationCode: '',
    containerCodesText: '',
    additionalInfoText: '',
  })
  const [submitting, setSubmitting] = useState(false)

  const updateField = <Key extends keyof ManualTaskDraft>(key: Key, value: ManualTaskDraft[Key]) => {
    setForm((current) => ({
      ...current,
      [key]: value,
    }))
  }

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setSubmitting(true)

    try {
      await onSubmit(form)
      setForm({
        taskCode: '',
        source: 'Manual',
        taskType: 'Inbound',
        description: '',
        startLocationCode: '',
        endLocationCode: '',
        containerCodesText: '',
        additionalInfoText: '',
      })
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <section className="page">
      <section className="page-header">
        <div>
          <p className="eyebrow">Operations</p>
          <h3>手工任务</h3>
        </div>
        <span className="toolbar-note">{loading ? '数据初始化中...' : '支持 mock / live 双模式'}</span>
      </section>

      <section className="panel form-panel">
        <div className="panel-head">
          <h4>创建手工任务</h4>
          <span>先落当前 unified API 已支持字段</span>
        </div>

        <form className="task-form" onSubmit={handleSubmit}>
          <label>
            <span>任务号（可选）</span>
            <input value={form.taskCode} onChange={(event) => updateField('taskCode', event.target.value)} placeholder="留空则自动生成" />
          </label>

          <label>
            <span>任务来源</span>
            <select value={form.source} onChange={(event) => updateField('source', event.target.value)}>
              <option value="Manual">Manual</option>
              <option value="WMS">WMS</option>
              <option value="AGV">AGV</option>
            </select>
          </label>

          <label>
            <span>任务类型</span>
            <input value={form.taskType} onChange={(event) => updateField('taskType', event.target.value)} placeholder="Inbound / Outbound / Transfer" />
          </label>

          <label>
            <span>起点货位</span>
            <input required value={form.startLocationCode} onChange={(event) => updateField('startLocationCode', event.target.value)} placeholder="00-001-0101" />
          </label>

          <label>
            <span>终点货位</span>
            <input required value={form.endLocationCode} onChange={(event) => updateField('endLocationCode', event.target.value)} placeholder="00-001-1208" />
          </label>

          <label className="form-span-2">
            <span>任务说明</span>
            <textarea rows={3} value={form.description} onChange={(event) => updateField('description', event.target.value)} placeholder="简要描述任务背景与处理目标" />
          </label>

          <label className="form-span-2">
            <span>托盘 / 容器编码</span>
            <textarea rows={4} value={form.containerCodesText} onChange={(event) => updateField('containerCodesText', event.target.value)} placeholder={'每行一个，例如：\nP240501\nBX-9128'} />
          </label>

          <label className="form-span-2">
            <span>Additional Info</span>
            <textarea rows={4} value={form.additionalInfoText} onChange={(event) => updateField('additionalInfoText', event.target.value)} placeholder={'每行一个 key=value，例如：\nowner=operator-a\npriorityTag=urgent'} />
          </label>

          <div className="form-footer form-span-2">
            <button type="submit" className="primary-button" disabled={submitting}>
              {submitting ? '提交中...' : '创建任务'}
            </button>
          </div>
        </form>
      </section>
    </section>
  )
}

function MetricCard({ label, value, tone }: { label: string; value: number; tone: 'blue' | 'green' | 'amber' | 'slate' }) {
  return (
    <article className={`metric-card metric-card--${tone}`}>
      <span>{label}</span>
      <strong>{value}</strong>
    </article>
  )
}

function StatusPill({
  kind,
  text,
}: {
  kind: TaskStatus | 'success' | 'danger' | 'warning'
  text: string
}) {
  const tone =
    kind === 'Executing' || kind === 'Sent' || kind === 'success'
      ? 'success'
      : kind === 'Suspend' || kind === 'warning'
        ? 'warning'
        : kind === 'Error' || kind === 'danger'
          ? 'danger'
          : kind === 'Completed'
            ? 'blue'
            : 'slate'

  return <span className={`status-pill status-pill--${tone}`}>{text}</span>
}

function DetailItem({ label, value }: { label: string; value: string }) {
  return (
    <div className="detail-item">
      <span>{label}</span>
      <strong>{value}</strong>
    </div>
  )
}

export default App
