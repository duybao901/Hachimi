import { Button } from "@/components/ui/button"
import { logout } from "@/services/auth.service";
import { useAuthStore } from "@/store/auth.store"
import { Link, useNavigate } from "@tanstack/react-router";
import { toast } from "sonner";
import { Search as SearchIcon } from "lucide-react";
import Logo from '@/assets/horse_logo.png'
import { Input } from "@/components/ui/input";

function Header() {
  const { currentUser } = useAuthStore();

  const navigate = useNavigate();

  return (
    <header className="h-14 border-b flex items-center px-6 gap-3 sticky z-50">
      <div className="w-full flex items-center justify-between">
        <div className="basis-2/3">
          <div className="flex items-center">
            <Link to='/'>
              <img src={Logo} className="w-8 h-8 mr-4"></img>
            </Link>
            <div className="w-full">
              <div className="relative h-10">
                <Link to='/'><SearchIcon className="absolute z-1 left-3 top-1/2 -translate-y-1/2 text-muted-foreground">
                </SearchIcon>
                </Link>
                <Input className="absolute h-full pl-10 pr-32">
                </Input>
                <span className="absolute right-3 top-1/2 -translate-y-1/2 text-xs text-muted-foreground whitespace-nowrap">
                  Powered by Hachimi
                </span>
              </div>
            </div>
          </div>
        </div>
        <nav className="basis-1/3 ml-4">
          {
            currentUser ?
              <div className="flex items-center justify-end">
                <Link to='/confirmSignout'><Button variant="outline">Sign out</Button></Link>
              </div> :
              <div className="flex items-center justify-end">
                <Link to='/auth/login'><Button variant="link">Log in</Button></Link>
                <Link to='auth/register'><Button variant="outline">Create Account</Button></Link>
              </div>
          }
        </nav>
      </div>
    </header>
  )
}

export default Header
