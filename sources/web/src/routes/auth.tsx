import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/auth')({
  component: About,
})

function About() {
  return <div className="p-2">Login/Register</div>
}