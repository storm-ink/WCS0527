import type {
  GatewayConfig,
  ManualTaskDraft,
  OperationsDevice,
  RuntimeMode,
  TaskStatus,
  WcsLocation,
  WcsTask,
} from '../types'

interface UnifiedApiResponse<T> {
  success: boolean
  code: string
  message: string
  data: T
  errors?: Array<{ field?: string; message: string }>
}

const runtimeMode = (import.meta.env.VITE_WCS_MODE as RuntimeMode | undefined) ?? 'mock'
const apiBaseUrl = (import.meta.env.VITE_WCS_API_BASE_URL as string | undefined) ?? 'http://127.0.0.1:9898'
const pollIntervalMs = Number(import.meta.env.VITE_WCS_POLL_INTERVAL_MS ?? 5000)

export function getGatewayConfig(): GatewayConfig {
  return {
    mode: runtimeMode,
    apiBaseUrl,
    pollIntervalMs,
  }
}

function iso(hoursAgo: number) {
  return new Date(Date.now() - hoursAgo * 60 * 60 * 1000).toISOString()
}

function location(userCode: string, deviceName: string, deviceCode = userCode): WcsLocation {
  return {
    userCode,
    deviceCode,
    unifiedCode: `${deviceName}:${userCode}`,
    deviceName,
  }
}

let nextTaskId = 1200

let mockTasks: WcsTask[] = [
  {
    id: 1001,
    taskCode: 'MT-20260528-001',
    source: 'Manual',
    startLocation: location('00-001-0101', 'Infeed-01'),
    endLocation: location('00-001-1208', 'Crane-01'),
    currentLocation: location('00-001-0603', 'Conveyor-01'),
    status: 'Executing',
    taskType: 'Inbound',
    createdAt: iso(1.5),
    description: 'Manual inbound replenishment',
    priority: 90,
    containerCodes: ['P240501'],
    additionalInfo: { owner: 'operator-a' },
    movements: [
      {
        id: 2001,
        deviceName: 'Conveyor-01',
        createdAt: iso(1.4),
        sendedAt: iso(1.35),
        routeId: 32,
        startLocation: location('00-001-0101', 'Infeed-01'),
        endLocation: location('00-001-0603', 'Conveyor-01'),
        status: 1,
        equipmentActions: [
          {
            id: 3001,
            equipmentTaskId: 8801,
            startLocation: location('00-001-0101', 'Infeed-01'),
            endLocation: location('00-001-0603', 'Conveyor-01'),
            deviceName: 'Conveyor-01',
            status: 1,
            createdAt: iso(1.35),
            sendedAt: iso(1.3),
            alarms: '',
            description: 'Move pallet from infeed to buffer',
          },
        ],
      },
    ],
  },
  {
    id: 1002,
    taskCode: 'WMS-20260528-115',
    source: 'WMS',
    startLocation: location('00-001-0802', 'Crane-02'),
    endLocation: location('00-001-0201', 'Outfeed-01'),
    currentLocation: location('00-001-0802', 'Crane-02'),
    status: 'Suspend',
    taskType: 'Outbound',
    createdAt: iso(4.2),
    description: 'Paused by operator for lane confirmation',
    priority: 80,
    containerCodes: ['BX-9128'],
    additionalInfo: { orderNo: 'SO-20260528-12' },
    movements: [],
  },
  {
    id: 1003,
    taskCode: 'AGV-20260528-009',
    source: 'AGV',
    startLocation: location('00-001-0210', 'Outfeed-02'),
    endLocation: location('00-001-0301', 'AGV-Dock'),
    currentLocation: location('00-001-0301', 'AGV-Dock'),
    status: 'Completed',
    taskType: 'Transfer',
    createdAt: iso(8.8),
    finishedAt: iso(6.2),
    description: 'Completed transfer to AGV dock',
    priority: 40,
    containerCodes: ['BX-9030'],
    additionalInfo: {},
    movements: [],
  },
  {
    id: 1004,
    taskCode: 'MT-20260528-004',
    source: 'Manual',
    startLocation: location('00-001-0411', 'Conveyor-02'),
    endLocation: location('00-001-0708', 'Crane-03'),
    currentLocation: location('00-001-0411', 'Conveyor-02'),
    status: 'Error',
    taskType: 'Relocation',
    createdAt: iso(2.6),
    description: 'Waiting for manual resume after exception',
    priority: 70,
    containerCodes: ['TB-1002'],
    additionalInfo: { exception: 'Barcode mismatch' },
    movements: [],
  },
]

let mockDevices: OperationsDevice[] = [
  { name: 'Crane-01', deviceType: 'Crane', isConnected: true, isLocked: false, lockerUser: '', lockerIp: '', isTaskable: true },
  { name: 'Crane-02', deviceType: 'Crane', isConnected: true, isLocked: true, lockerUser: 'operator-b', lockerIp: '10.0.0.32', isTaskable: true },
  { name: 'Conveyor-01', deviceType: 'Conveyor', isConnected: true, isLocked: false, lockerUser: '', lockerIp: '', isTaskable: true },
  { name: 'Conveyor-02', deviceType: 'Conveyor', isConnected: false, isLocked: false, lockerUser: '', lockerIp: '', isTaskable: true },
  { name: 'AGV-Dock', deviceType: 'Dock', isConnected: true, isLocked: false, lockerUser: '', lockerIp: '', isTaskable: false },
  { name: 'Robot-01', deviceType: 'Robot', isConnected: true, isLocked: true, lockerUser: 'maintenance', lockerIp: '10.0.0.88', isTaskable: true },
]

function delay(ms = 220) {
  return new Promise((resolve) => window.setTimeout(resolve, ms))
}

function cloneTasks() {
  return mockTasks
    .slice()
    .sort((left, right) => Date.parse(right.createdAt) - Date.parse(left.createdAt))
    .map((item) => structuredClone(item))
}

function cloneDevices() {
  return mockDevices.slice().map((item) => ({ ...item }))
}

function ensureOk<T>(response: UnifiedApiResponse<T>) {
  if (!response.success) {
    const message = response.errors?.map((item) => item.message).join(', ') || response.message || response.code
    throw new Error(message)
  }

  return response.data
}

function coerceTaskStatus(value: unknown): TaskStatus {
  const raw = String(value ?? 'New')
  if (raw === '0') return 'New'
  if (raw === '1') return 'Sent'
  if (raw === '2') return 'Executing'
  if (raw === '3') return 'Suspend'
  if (raw === '4') return 'Error'
  if (raw === '5') return 'Cancelled'
  if (raw === '6') return 'Completed'
  if (raw === 'New' || raw === 'Sent' || raw === 'Executing' || raw === 'Suspend' || raw === 'Error' || raw === 'Cancelled' || raw === 'Completed') {
    return raw
  }

  return 'New'
}

function fromApiLocation(raw: Record<string, unknown>): WcsLocation {
  return {
    userCode: String(raw.userCode ?? raw.UserCode ?? ''),
    deviceCode: String(raw.deviceCode ?? raw.DeviceCode ?? ''),
    unifiedCode: String(raw.unifiedCode ?? raw.UnifiedCode ?? ''),
    deviceName: String(raw.deviceName ?? raw.DeviceName ?? ''),
  }
}

function fromApiTask(raw: Record<string, unknown>): WcsTask {
  const movements = Array.isArray(raw.movements ?? raw.Movements) ? (raw.movements ?? raw.Movements) as Array<Record<string, unknown>> : []

  return {
    id: Number(raw.id ?? raw.Id ?? 0),
    taskCode: String(raw.taskCode ?? raw.TaskCode ?? ''),
    source: String(raw.source ?? raw.Source ?? ''),
    startLocation: fromApiLocation((raw.startLocation ?? raw.StartLocation ?? {}) as Record<string, unknown>),
    endLocation: fromApiLocation((raw.endLocation ?? raw.EndLocation ?? {}) as Record<string, unknown>),
    currentLocation: fromApiLocation((raw.currentLocation ?? raw.CurrentLocation ?? {}) as Record<string, unknown>),
    status: coerceTaskStatus(raw.status ?? raw.Status),
    taskType: String(raw.taskType ?? raw.TaskType ?? ''),
    createdAt: String(raw.createdAt ?? raw.CreatedAt ?? new Date().toISOString()),
    finishedAt: raw.finishedAt ?? raw.FinishedAt ? String(raw.finishedAt ?? raw.FinishedAt) : undefined,
    description: String(raw.description ?? raw.Description ?? ''),
    priority: Number(raw.priority ?? raw.Priority ?? 0),
    containerCodes: Array.isArray(raw.containerCodes ?? raw.ContainerCodes) ? ((raw.containerCodes ?? raw.ContainerCodes) as string[]) : [],
    additionalInfo: ((raw.additionalInfo ?? raw.AdditionalInfo) as Record<string, string> | undefined) ?? {},
    movements: movements.map((movement) => ({
      id: Number(movement.id ?? movement.Id ?? 0),
      deviceName: String(movement.deviceName ?? movement.DeviceName ?? ''),
      createdAt: String(movement.createdAt ?? movement.CreatedAt ?? new Date().toISOString()),
      finishedAt: movement.finishedAt ?? movement.FinishedAt ? String(movement.finishedAt ?? movement.FinishedAt) : undefined,
      sendedAt: movement.sendedAt ?? movement.SendedAt ? String(movement.sendedAt ?? movement.SendedAt) : undefined,
      routeId: movement.routeId ?? movement.RouteId ? Number(movement.routeId ?? movement.RouteId) : undefined,
      startLocation: fromApiLocation((movement.startLocation ?? movement.StartLocation ?? {}) as Record<string, unknown>),
      endLocation: fromApiLocation((movement.endLocation ?? movement.EndLocation ?? {}) as Record<string, unknown>),
      status: Number(movement.status ?? movement.Status ?? 0),
      equipmentActions: Array.isArray(movement.equipmentActions ?? movement.EquipmentActions)
        ? ((movement.equipmentActions ?? movement.EquipmentActions) as Array<Record<string, unknown>>).map((action) => ({
            id: Number(action.id ?? action.Id ?? 0),
            equipmentTaskId: Number(action.equipmentTaskId ?? action.EquipmentTaskId ?? 0),
            startLocation: fromApiLocation((action.startLocation ?? action.StartLocation ?? {}) as Record<string, unknown>),
            endLocation: fromApiLocation((action.endLocation ?? action.EndLocation ?? {}) as Record<string, unknown>),
            deviceName: String(action.deviceName ?? action.DeviceName ?? ''),
            status: Number(action.status ?? action.Status ?? 0),
            createdAt: String(action.createdAt ?? action.CreatedAt ?? new Date().toISOString()),
            sendedAt: action.sendedAt ?? action.SendedAt ? String(action.sendedAt ?? action.SendedAt) : undefined,
            finishedAt: action.finishedAt ?? action.FinishedAt ? String(action.finishedAt ?? action.FinishedAt) : undefined,
            alarms: String(action.alarms ?? action.Alarms ?? ''),
            description: String(action.description ?? action.Description ?? ''),
          }))
        : [],
    })),
  }
}

function fromApiDevice(raw: Record<string, unknown>): OperationsDevice {
  return {
    name: String(raw.name ?? raw.Name ?? ''),
    deviceType: String(raw.deviceType ?? raw.DeviceType ?? ''),
    isConnected: Boolean(raw.isConnected ?? raw.IsConnected),
    isLocked: Boolean(raw.isLocked ?? raw.IsLocked),
    lockerUser: String(raw.lockerUser ?? raw.LockerUser ?? ''),
    lockerIp: String(raw.lockerIp ?? raw.LockerIp ?? ''),
    isTaskable: Boolean(raw.isTaskable ?? raw.IsTaskable),
  }
}

async function request<T>(path: string, init?: RequestInit) {
  const response = await fetch(`${apiBaseUrl}${path}`, {
    headers: {
      'Content-Type': 'application/json',
    },
    ...init,
  })

  const json = (await response.json()) as UnifiedApiResponse<T>
  return ensureOk(json)
}

function updateTaskStatus(taskCode: string, nextStatus: TaskStatus, currentUserCode?: string) {
  mockTasks = mockTasks.map((task) => {
    if (task.taskCode !== taskCode) {
      return task
    }

    const currentLocation = currentUserCode
      ? {
          ...task.currentLocation,
          userCode: currentUserCode,
          unifiedCode: `${task.currentLocation.deviceName}:${currentUserCode}`,
        }
      : task.currentLocation

    return {
      ...task,
      currentLocation,
      status: nextStatus,
      finishedAt: nextStatus === 'Completed' || nextStatus === 'Cancelled' ? new Date().toISOString() : task.finishedAt,
    }
  })
}

function parseAdditionalInfo(text: string) {
  return text
    .split('\n')
    .map((line) => line.trim())
    .filter(Boolean)
    .reduce<Record<string, string>>((result, line) => {
      const [key, ...rest] = line.split('=')
      if (!key || rest.length === 0) {
        return result
      }

      result[key.trim()] = rest.join('=').trim()
      return result
    }, {})
}

function parseContainerCodes(text: string) {
  return text
    .split('\n')
    .map((line) => line.trim())
    .filter(Boolean)
}

export async function listTasks() {
  if (runtimeMode === 'live') {
    const data = await request<Array<Record<string, unknown>>>('/api/v1/operations/tasks')
    return data.map(fromApiTask)
  }

  await delay()
  return cloneTasks()
}

export async function listDevices() {
  if (runtimeMode === 'live') {
    const data = await request<Array<Record<string, unknown>>>('/api/v1/operations/devices')
    return data.map(fromApiDevice)
  }

  await delay()
  return cloneDevices()
}

export async function createManualTask(draft: ManualTaskDraft) {
  const payload = {
    taskCode: draft.taskCode?.trim() || undefined,
    source: draft.source,
    taskType: draft.taskType,
    description: draft.description.trim(),
    startLocation: {
      userCode: draft.startLocationCode.trim(),
    },
    endLocation: {
      userCode: draft.endLocationCode.trim(),
    },
    containerCodes: parseContainerCodes(draft.containerCodesText),
    additionalInfo: parseAdditionalInfo(draft.additionalInfoText),
  }

  if (runtimeMode === 'live') {
    const data = await request<Record<string, unknown>>('/api/v1/operations/tasks', {
      method: 'POST',
      body: JSON.stringify(payload),
    })
    return fromApiTask(data)
  }

  await delay(320)
  const newTask: WcsTask = {
    id: nextTaskId++,
    taskCode: payload.taskCode || `MT-${new Date().toISOString().slice(0, 10).replace(/-/g, '')}-${String(nextTaskId).padStart(3, '0')}`,
    source: payload.source,
    startLocation: location(payload.startLocation.userCode, 'Manual-In'),
    endLocation: location(payload.endLocation.userCode, 'Manual-Out'),
    currentLocation: location(payload.startLocation.userCode, 'Manual-In'),
    status: 'New',
    taskType: payload.taskType,
    createdAt: new Date().toISOString(),
    description: payload.description,
    priority: 50,
    movements: [],
    containerCodes: payload.containerCodes,
    additionalInfo: payload.additionalInfo,
  }

  mockTasks = [newTask, ...mockTasks]
  return structuredClone(newTask)
}

export async function mutateTask(task: WcsTask, action: 'suspend' | 'cancel' | 'resume' | 'archive' | 'complete', currentUserCode?: string) {
  if (runtimeMode === 'live') {
    if (action === 'complete') {
      const data = await request<Record<string, unknown>>(`/api/v1/operations/tasks/by-id/${task.id}/complete`, {
        method: 'POST',
      })
      return fromApiTask(data)
    }

    if (action === 'archive') {
      const data = await request<Record<string, unknown>>(`/api/v1/operations/tasks/by-id/${task.id}/archive`, {
        method: 'POST',
      })
      return fromApiTask(data)
    }

    const body = action === 'resume' ? { currentUserCode } : undefined
    const data = await request<Record<string, unknown>>(`/api/v1/operations/tasks/${task.taskCode}/${action === 'resume' ? 'resume' : action}`, {
      method: 'POST',
      body: body ? JSON.stringify(body) : undefined,
    })
    return fromApiTask(data)
  }

  await delay(220)
  if (action === 'suspend') {
    updateTaskStatus(task.taskCode, 'Suspend')
  } else if (action === 'cancel') {
    updateTaskStatus(task.taskCode, 'Cancelled')
  } else if (action === 'resume') {
    updateTaskStatus(task.taskCode, 'Executing', currentUserCode)
  } else if (action === 'archive') {
    mockTasks = mockTasks.filter((item) => item.taskCode !== task.taskCode)
  } else if (action === 'complete') {
    updateTaskStatus(task.taskCode, 'Completed')
  }

  return cloneTasks().find((item) => item.taskCode === task.taskCode) ?? task
}

export async function setDeviceLock(device: OperationsDevice, lockDevice: boolean) {
  if (runtimeMode === 'live') {
    const suffix = lockDevice ? 'lock' : 'unlock'
    const data = await request<Record<string, unknown>>(`/api/v1/operations/devices/${encodeURIComponent(device.name)}/${suffix}`, {
      method: 'POST',
    })
    return fromApiDevice(data)
  }

  await delay(180)
  mockDevices = mockDevices.map((item) => {
    if (item.name !== device.name) {
      return item
    }

    return {
      ...item,
      isLocked: lockDevice,
      lockerUser: lockDevice ? 'phase4-web' : '',
      lockerIp: lockDevice ? '127.0.0.1' : '',
    }
  })

  return cloneDevices().find((item) => item.name === device.name) ?? device
}
