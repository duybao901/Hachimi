import { GlobalLoading } from "@/components/Common/globaLoading"
import { createRootRoute, Outlet } from "@tanstack/react-router"
import { TanStackRouterDevtools } from "@tanstack/react-router-devtools"

export const Route = createRootRoute({
  component: RootLayout,
})

function RootLayout() {

  return (
    <>
      <main>
        <Outlet />
        <GlobalLoading />
      </main>
      <TanStackRouterDevtools></TanStackRouterDevtools>
    </>
  )
}
