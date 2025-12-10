import { createRootRoute, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools'

export const Route = createRootRoute({
  component: RootLayout,
})

function RootLayout() {
  return (
    <>
      <div className="w-full">
        <main className="max-w-3xl mx-auto mt-6 px-4">
          <Outlet />
        </main>
      </div>
      <TanStackRouterDevtools></TanStackRouterDevtools>
    </>
  )
}
