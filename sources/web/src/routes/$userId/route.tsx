import { Outlet, createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/$userId')({
  component: UserLayout,
})

function UserLayout() {
  return <Outlet />
}
