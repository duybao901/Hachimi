import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/top/month/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/(feed)/top/month/"!</div>
}
