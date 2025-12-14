import axios from "axios"
import { useAuthStore } from "../store/auth.store"

// const refreshApi = axios.create({
//   baseURL: import.meta.env.VITE_API_BASE_URL, // vd: http://localhost:5173 (proxy)
//   withCredentials: true,
// })

export function createApi(baseURL: string) {
  const api = axios.create({
    baseURL,
    withCredentials: true,
  })

  // Send access token header
  api.interceptors.request.use((config) => {
    const token = useAuthStore.getState().accessToken
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  })

  api.interceptors.response.use(
    (res) => res,
    async (err) => {
      const status = err.response?.status
      const headers = err.response?.headers || {}

      if (status === 401) {
        if (headers["is-token-revoked"] === "true") {
          useAuthStore.getState().logout()
          return Promise.reject(err)
        }

        // if (headers["is-token-expired"] === "true") {
        //   return handleRefreshToken(api, err)
        // }
      }

      return Promise.reject(err)
    }
  )

  return api
}

/*
  Req A → 401 → refresh-token
  Req B → 401 → queue
  Req C → 401 → queue

  refresh-token OK →
    - provide new token mới
    - retry A
    - retry B
    - retry C
  */
const handleRefreshToken = async (api: any, err: any) => {
  // make refresh token when access token expired
  let isRefreshing = false

  // list of pending requests while refreshing
  let queue: Array<{
    resolve: (token: string) => void
    reject: (err: any) => void
  }> = []

  const original = err.config

  if (!original || original._retry) {
    return Promise.reject(err)
  }

  original._retry = true

  if (isRefreshing) {
    return new Promise((resolve, reject) => {
      queue.push({
        resolve: (token: string) => {
          original.headers = original.headers ?? {}
          original.headers.Authorization = `Bearer ${token}`
          resolve(api(original))
        },
        reject,
      })
    })
  }

  isRefreshing = true

  try {
    const res = await axios.post(
      "/auth-api/v1/authen/refresh-token",
      null,
      {
        withCredentials: true,
      }
    )

    const newToken = res.data.value.accessToken

    useAuthStore.getState().setAccessToken(newToken)

    queue.forEach((p) => p.resolve(newToken))
    queue = []

    original.headers = original.headers ?? {}
    original.headers.Authorization = `Bearer ${newToken}`

    return api(original)
  } catch (refreshErr) {
    queue.forEach((p) => p.reject(refreshErr))
    queue = []

    useAuthStore.getState().logout()
    return Promise.reject(refreshErr)
  } finally {
    isRefreshing = false
  }
}
