import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/top/week/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/(feed)/top/week/"!</div>
}
