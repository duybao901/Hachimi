import SideBarLeft from '@/components/Layout/SideBarLeft/SideBarLeft'
import SideBarRight from '@/components/Layout/SideBarRight/SideBarRight'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/_app/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <div className='flex gap-4'>
    <div className='w-60'>
      <SideBarLeft></SideBarLeft>
    </div>

    <div className='grow'>
      List Post
    </div>

    <div className='w-60'>
      <SideBarRight></SideBarRight>
    </div>
  </div>
}
