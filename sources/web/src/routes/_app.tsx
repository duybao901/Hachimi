import Header from "@/components/Layout/Header/Header"
import { createFileRoute, Outlet } from "@tanstack/react-router"

export const Route = createFileRoute("/_app")({
  component: RouteComponent,
})

function RouteComponent() {
  return (
    <div>
      <Header></Header>
      <div className="bg-gray-50 py-2 min-h-screen">
        <div className="w-6xl px-4 gap-2 m-auto">
          <Outlet></Outlet>
        </div>
      </div>
    </div>
  )
}
