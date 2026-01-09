export interface ApiResponse<T> {
  value: T
  isSuccess: boolean
  isFailure: boolean
  error: {
    code: string
    message: string
  }
}

export interface PagedResult {
    // TODO
}

export interface ValidationErrorResponse {
  type: string
  title: string
  status: number
  errors: Record<string, string[]>
  traceId: string
}
