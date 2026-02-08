import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/goat/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/(feed)/goat/"!</div>
}
