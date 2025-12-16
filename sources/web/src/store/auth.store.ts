import { create } from "zustand"
import { devtools } from "zustand/middleware"

export interface CurrentUser {
  id: string
  email: string
}

type AuthState = {
  currentUser: CurrentUser | null
  accessToken: string | null
  authLoaded: boolean
}

type AuthActions = {
  setAccessToken: (token: string | null) => void
  setCurrentUser: (user: CurrentUser | null) => void
  logout: () => void
  setAuthLoaded: (load: boolean) => void
}

export const useAuthStore = create<AuthState & AuthActions>()(
  devtools((set) => ({
    currentUser: null,
    accessToken: null,
    authLoaded: false,

    setAccessToken: (token) => set({ accessToken: token }),
    setCurrentUser: (currentUser) => set({ currentUser }),
    logout: () => set({ currentUser: null, accessToken: null, authLoaded: false }),
    setAuthLoaded: (load: boolean) => set({ authLoaded: load })
  }))
)
