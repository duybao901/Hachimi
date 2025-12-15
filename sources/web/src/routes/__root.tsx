import { createRootRoute, Outlet } from "@tanstack/react-router"
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools"
import { useEffect } from "react"
import { toast } from "sonner"
import { loadSessionOnInit } from "@/services/auth.service"

export const Route = createRootRoute({
  component: RootLayout,
})

function RootLayout() {
  useEffect(() => {
    const initAuth = async () => {
      try {
        const isFirstLogin = localStorage.getItem("isFirstLogin")
        if (isFirstLogin !== "true") {
          return
        }

        await loadSessionOnInit();
      } catch (error: any) {
        if(error.response.data){
          toast.error(error.response?.data?.Detail);
        }else{
          toast.error(error.message || "Server error");
        }
      }
    }
    initAuth()
  }, [])

  return (
    <>
      <main className="">
        <Outlet />
      </main>
      <TanStackRouterDevtools></TanStackRouterDevtools>
    </>
  )
}
