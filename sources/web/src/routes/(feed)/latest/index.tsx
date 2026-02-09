import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/latest/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/(feed)/lastest/"!</div>
}
