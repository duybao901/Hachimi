import Header from '@/components/Layout/Header/Header'
import SideBarRight from '@/components/Layout/SideBarRight/SideBarRight'
import { createFileRoute, Outlet, useLocation } from '@tanstack/react-router'
import SideBarLeft from '@/components/Layout/SideBarLeft/SideBarLeft'
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
import { useAuthStore } from '@/store/auth.store'

export const Route = createFileRoute('/(feed)')({
  component: RouteComponent,
})

function RouteComponent() {

  const { currentUser } = useAuthStore.getState();

  const location = useLocation();
  const pathname = location.pathname;

  const isDiscover = pathname === "/" || pathname === "/discover";
  const isFollowing = pathname === "/following";

  return (
    <div>
      <Header></Header>
      <div className="bg-gray-50 py-2 min-h-screen">
        <div className="w-7xl px-4 gap-2 m-auto">
          <div className='flex gap-4'>
            <div className='w-60 shrink-0'>
              <SideBarLeft></SideBarLeft>
            </div>

            <div className='grow'>
              <QuickPostBox></QuickPostBox>

              {/* Dropdown menu for sorting options */}
              <div className='mt-2 mb-2 flex justify-between items-center'>
                <div className="flex justify-between items-center gap-2">
                  {
                    currentUser ? <><Button
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
                    </> : <>
                      <Button
                        variant="ghost"
                        className={`text-base font-light hover:text-primary ${isDiscover ? "text-black font-bold rounded bg-white" : ""
                          }`}
                      >
                        Relevant
                      </Button>

                      <Button
                        variant="ghost"
                        className={`text-base font-light hover:text-primary ${isFollowing ? "text-primary font-medium" : ""
                          }`}
                      >
                        Lastest
                      </Button>

                      <Button
                        variant="ghost"
                        className={`text-base font-light hover:text-primary ${isFollowing ? "text-primary font-medium" : ""
                          }`}
                      >
                        Top
                      </Button>
                    </>
                  }
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
              {/* <PostFeeds></PostFeeds> */}
              <Outlet></Outlet>

            </div>

            <div className='w-60 shrink-0'>
              <SideBarRight></  SideBarRight>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
