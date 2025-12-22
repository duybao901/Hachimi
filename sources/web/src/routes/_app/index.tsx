import SideBarLeft from '@/components/Layout/SideBarLeft/SideBarLeft'
import SideBarRight from '@/components/Layout/SideBarRight/SideBarRight'
import { createFileRoute, useLocation } from '@tanstack/react-router'
import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Ellipsis as EllipsisIcon } from 'lucide-react';
import QuickPostBox from '@/components/Posts/QuickPostBox'
import PostLists from '@/components/Posts/PostLists'

export const Route = createFileRoute('/_app/')({
  component: RouteComponent,
})

function RouteComponent() {

  const location = useLocation();
  const pathname = location.pathname;

  const isDiscover = pathname === "/" || pathname === "/discover";
  const isFollowing = pathname === "/following";

  return <div className='flex gap-4'>
    <div className='w-60 shrink-0'>
      <SideBarLeft></SideBarLeft>
    </div>

    <div className='grow'>
      <QuickPostBox></QuickPostBox>

      {/* Dropdown menu for sorting options */}
      <div className='mt-2 mb-2 flex justify-between items-center'>
        <div className="flex justify-between items-center gap-2">
          <Button
            variant="ghost"
            className={`text-base font-light hover:text-primary ${isDiscover ? "text-black font-bold rounded bg-white" : ""
              }`}
          >
            Discover
          </Button>

          <Button
            variant="ghost"
            className={`text-base font-light hover:text-primary ${isFollowing ? "text-primary font-medium" : ""
              }`}
          >
            Following
          </Button>
        </div>
        <div>
          <DropdownMenu>
            <DropdownMenuTrigger className="bg-transparent !focus-visible:shadow-none border-none outline-none">
              <Button variant="secondary" className="bg-transparent !focus-visible:shadow-none">
                <EllipsisIcon className="w-5 h-5"></EllipsisIcon>
              </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent className='hover:bg-none w-60'>
              <DropdownMenuLabel>Relevant</DropdownMenuLabel>
              <DropdownMenuSeparator />
              <DropdownMenuItem
                className='bg-transparent hover:bg-transparent focus:bg-transparent cursor-pointer text-(--base-90) data-highlighted:text-primary data-highlighted:bg-transparent'>
                Top this week
              </DropdownMenuItem>
              <DropdownMenuItem
                className='bg-transparent hover:bg-transparent focus:bg-transparent cursor-pointer text-(--base-90) data-highlighted:text-primary data-highlighted:bg-transparent'>
                Top this month
              </DropdownMenuItem>
              <DropdownMenuItem
                className='bg-transparent hover:bg-transparent focus:bg-transparent cursor-pointer text-(--base-90) data-highlighted:text-primary data-highlighted:bg-transparent'>
                Top this year
              </DropdownMenuItem>
              <DropdownMenuItem
                className='bg-transparent hover:bg-transparent focus:bg-transparent cursor-pointer text-(--base-90) data-highlighted:text-primary data-highlighted:bg-transparent'>
                Top of all time
              </DropdownMenuItem>
              <DropdownMenuSeparator />
              <DropdownMenuItem
                className='bg-transparent hover:bg-transparent focus:bg-transparent cursor-pointer text-(--base-90) data-highlighted:text-primary data-highlighted:bg-transparent'>
                Lastest
              </DropdownMenuItem>
            </DropdownMenuContent>
          </DropdownMenu>
        </div>
      </div>

      {/* Posts list */}
      <PostLists></PostLists>

    </div>

    <div className='w-60 shrink-0'>
      <SideBarRight></SideBarRight>
    </div>
  </div>
}
