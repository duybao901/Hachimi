import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/year/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>Hello "/(feed)/year/"!</div>
}
