import { Button } from "@/components/ui/button"
import { useAuthStore } from "@/store/auth.store"
import { Link } from "@tanstack/react-router"
import { Bell as BellIcon, Search as SearchIcon } from "lucide-react"
import Logo from "@/assets/horse_logo.png"
import { Input } from "@/components/ui/input"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { useState } from "react"

function Header() {
  const [open, setOpen] = useState<boolean>(false)
  const { currentUser } = useAuthStore()

  return (
    <header className="h-14 border-b flex items-center justify-center sticky z-50">
      <div className="w-6xl flex items-center justify-between px-4">
        <div className="basis-2/3">
          <div className="flex items-center">
            <Link to="/">
              <img src={Logo} className="w-8 h-8 mr-4"></img>
            </Link>
            <div className="w-full">
              <div className="relative h-10">
                <Link to="/">
                  <SearchIcon className="absolute z-1 left-3 top-1/2 -translate-y-1/2 text-muted-foreground"></SearchIcon>
                </Link>
                <Input className="absolute h-full pl-10 pr-32"></Input>
                <span className="absolute right-3 top-1/2 -translate-y-1/2 text-xs text-muted-foreground whitespace-nowrap">
                  Powered by Hachimi
                </span>
              </div>
            </div>
          </div>
        </div>
        <nav className="basis-1/3 ml-4">
          {currentUser ? (
            <div className="flex items-center justify-end gap-4">
              <Button variant="outline">Create Post</Button>
              <Link to="/notifications">
                <Button variant="secondary" className="bg-white">
                  <BellIcon className="w-5 h-5"></BellIcon>
                </Button>
              </Link>
              <div>
                <DropdownMenu open={open} onOpenChange={setOpen}>
                  <DropdownMenuTrigger>
                    <div
                      className={`w-10 h-10 rounded-full overflow-hidden cursor-pointer p-1 flex items-center justify-center transition-colors 
                      ${open ? "bg-primary/10" : "hover:bg-primary/10"}`}
                    >
                      <img
                        className="w-full h-full rounded-full"
                        src={currentUser.avatarUrl}
                      ></img>
                    </div>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent className="p-2">
                    <DropdownMenuLabel className="p-0">
                      <Link
                        to="/"
                        className="group px-4 py-1 h-auto w-auto flex flex-col items-start justify-start gap-0 hover:bg-primary/10 rounded-md"
                      >
                        <div className="text-base text-gray-700 group-hover:text-primary">
                          {currentUser.name}
                        </div>
                        <span className="text-sm text-gray-500 font-light group-hover:text-primary">
                          @{currentUser.userName}
                        </span>
                      </Link>
                    </DropdownMenuLabel>
                    <DropdownMenuSeparator />
                    <Link
                      onClick={() => setOpen(false)}
                      to="/"
                      className="text-[15px] font-light px-4 py-2 h-auto w-full flex flex-col items-start justify-start gap-0 hover:bg-primary/10 rounded-md hover:text-primary hover:underline"
                    >
                      Dashboard
                    </Link>
                    <Link
                      onClick={() => setOpen(false)}
                      to="/"
                      className="text-[15px] font-light px-4 py-2 h-auto w-full flex flex-col items-start justify-start gap-0 hover:bg-primary/10 rounded-md hover:text-primary hover:underline"
                    >
                      Create Post
                    </Link>
                    <Link
                      onClick={() => setOpen(false)}
                      to="/"
                      className="text-[15px] font-light px-4 py-2 h-auto w-full flex flex-col items-start justify-start gap-0 hover:bg-primary/10 rounded-md hover:text-primary hover:underline"
                    >
                      Reading list
                    </Link>
                    <Link
                      onClick={() => setOpen(false)}
                      to="/"
                      className="text-[15px] font-light px-4 py-2 h-auto w-full flex flex-col items-start justify-start gap-0 hover:bg-primary/10 rounded-md hover:text-primary hover:underline"
                    >
                      Settings
                    </Link>
                    <DropdownMenuSeparator />
                    <Link
                      onClick={() => setOpen(false)}
                      to="/confirmSignout"
                      className="text-[15px] font-light px-4 py-2 h-auto w-full flex flex-col items-start justify-start gap-0 hover:bg-primary/10 rounded-md hover:text-primary hover:underline"
                    >
                      Sign out
                    </Link>
                  </DropdownMenuContent>
                </DropdownMenu>
              </div>
            </div>
          ) : (
            <div className="flex items-center justify-end">
              <Link to="/auth/login">
                <Button variant="link">Log in</Button>
              </Link>
              <Link to="auth/register">
                <Button variant="outline">Create Account</Button>
              </Link>
            </div>
          )}
        </nav>
      </div>
    </header>
  )
}

export default Header
