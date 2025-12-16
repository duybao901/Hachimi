import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/_app/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div className='w-6xl flex items-center justify-between px-4'>
    
  </div>
}
