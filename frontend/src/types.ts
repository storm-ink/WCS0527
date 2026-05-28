export type RuntimeMode = 'mock' | 'live'

export type TaskStatus =
  | 'New'
  | 'Sent'
  | 'Executing'
  | 'Suspend'
  | 'Error'
  | 'Cancelled'
  | 'Completed'

export interface WcsLocation {
  userCode: string
  deviceCode: string
  unifiedCode: string
  deviceName: string
}

export interface WcsEquipmentAction {
  id: number
  equipmentTaskId: number
  startLocation: WcsLocation
  endLocation: WcsLocation
  deviceName: string
  status: number
  createdAt: string
  sendedAt?: string
  finishedAt?: string
  alarms: string
  description: string
}

export interface WcsLogicMovement {
  id: number
  deviceName: string
  createdAt: string
  finishedAt?: string
  sendedAt?: string
  routeId?: number
  startLocation: WcsLocation
  endLocation: WcsLocation
  status: number
  equipmentActions: WcsEquipmentAction[]
}

export interface WcsTask {
  id: number
  taskCode: string
  source: string
  startLocation: WcsLocation
  endLocation: WcsLocation
  currentLocation: WcsLocation
  status: TaskStatus
  taskType: string
  createdAt: string
  finishedAt?: string
  description: string
  priority: number
  movements: WcsLogicMovement[]
  containerCodes: string[]
  additionalInfo: Record<string, string>
}

export interface OperationsDevice {
  name: string
  deviceType: string
  isConnected: boolean
  isLocked: boolean
  lockerUser: string
  lockerIp: string
  isTaskable: boolean
}

export interface ManualTaskDraft {
  taskCode?: string
  source: string
  taskType: string
  description: string
  startLocationCode: string
  endLocationCode: string
  containerCodesText: string
  additionalInfoText: string
}

export interface DashboardSnapshot {
  totalTasks: number
  executingTasks: number
  blockedTasks: number
  finishedTasks: number
  connectedDevices: number
  lockedDevices: number
  taskableDevices: number
}

export interface GatewayConfig {
  mode: RuntimeMode
  apiBaseUrl: string
  pollIntervalMs: number
}
