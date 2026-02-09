import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/following/latest')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/(feed)/following/lastest"!</div>
}
