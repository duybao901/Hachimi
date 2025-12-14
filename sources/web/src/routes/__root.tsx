import { createRootRoute, Outlet } from "@tanstack/react-router"
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools"
import { useEffect } from "react"
import axios from "axios"
import { useAuthStore } from "@/store/auth.store"

export const Route = createRootRoute({
  component: RootLayout,
})

function RootLayout() {
  useEffect(() => {
    const initAuth = async () => {
      try {
        const res = await axios.post(
          "/auth-api/v1/authen/refresh-token",
          null,
          {
            withCredentials: true,
          }
        )

        useAuthStore.getState().setAccessToken(res.data.value.accessToken)

        // const profile = await queryApi.get("/me");
        // useAuthStore.getState().setUser(profile.data);
      } catch {
        useAuthStore.getState().logout()
      }
    }

    initAuth()
  })

  return (
    <>
      <main className="">
        <Outlet />
      </main>
      <TanStackRouterDevtools></TanStackRouterDevtools>
    </>
  )
}
