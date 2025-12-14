import { create } from "zustand"
import { devtools } from "zustand/middleware"

export interface CurrentUser {
  id: string
  email: string
  userName: string
}

type AuthState = {
  user: CurrentUser | null
  accessToken: string | null
}

type AuthActions = {
  setAccessToken: (token: string | null) => void
  setUser: (user: CurrentUser | null) => void
  logout: () => void
}

export const useAuthStore = create<AuthState & AuthActions>()(
  devtools((set) => ({
    user: null,
    accessToken: null,

    setAccessToken: (token) => set({ accessToken: token }),
    setUser: (user) => set({ user }),
    logout: () => set({ user: null, accessToken: null }),
  }))
)
