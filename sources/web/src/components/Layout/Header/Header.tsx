import { Button } from "@/components/ui/button"
import { logout } from "@/services/auth.service";
import { useAuthStore } from "@/store/auth.store"
import { useNavigate } from "@tanstack/react-router";
import { toast } from "sonner";

function Header() {
  const { currentUser } = useAuthStore();

  const navigate = useNavigate();
  const logoutUser = async () => {
    if (currentUser) {
      try {
        await logout();
        navigate({to: '/'})
      } catch (error: any) {
        if (error.response) {
          toast.error(
            error.response?.data?.Detail || error.message || "Server error"
          )
        }
      }
    }
  }

  return (
    <header className="h-14 border-b flex items-center px-4 gap-3">
      Header
      {currentUser && <Button variant={"outline"} onClick={logoutUser}>Logout</Button>}
    </header>
  )
}

export default Header
