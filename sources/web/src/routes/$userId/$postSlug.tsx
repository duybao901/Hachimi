import Header from '@/components/Layout/Header/Header'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/$userId/$postSlug')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div>
    <Header></Header>
    <div className="bg-gray-50 py-2 min-h-screen">
      <div className="w-7xl px-4 gap-2 m-auto">
        Hello "/$userId/$postSlug/"!
      </div>
    </div>
  </div>
}
