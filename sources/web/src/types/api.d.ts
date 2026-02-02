export interface ApiResponse<T> {
  value: T
  isSuccess: boolean
  isFailure: boolean
  error: {
    code: string
    message: string
  }
}

export interface PagedResult<T> {
  value: {
    items: T[]
    pageSize: number
    pageIndex: number
    TotalCount: number
    HasNextPage: boolean
    HasPreviousPage: boolean
  }
  isSuccess: boolean
  isFailure: boolean
  error: {
    code: string
    message: string
  }

}

type ValidationErrorItem = {
  code: string
  message: string
}

export interface ValidationErrorResponse {
  type: string
  title: string
  status: number
  errors: Record<string, ValidationErrorItem>
  traceId: string
}
