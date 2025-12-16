import { createFileRoute, useNavigate } from "@tanstack/react-router"
import { Button } from "@/components/ui/button"
import { logout } from "@/services/auth.service"
import { toast } from "sonner"
import { useAuthStore } from "@/store/auth.store"
import { useState } from "react"
import { Spinner } from "@/components/ui/spinner"

export const Route = createFileRoute("/_app/confirmSignout")({
  component: RouteComponent,
})

function RouteComponent() {
  const { currentUser } = useAuthStore.getState()
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const navigate = useNavigate()

  const logoutUser = async () => {
    try {
      if (currentUser) {
        setIsLoading(true)
        await logout()
        navigate({ to: "/auth/login" })
      }
    } catch (error: any) {
      if (error.response) {
        toast.error(
          error.response?.data?.Detail || error.message || "Server error"
        )
      }
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className="w-screen h-80 flex flex-col items-center justify-center">
      <h1 className="text-2xl font-semibold mb-4">
        Are you sure you want to sign out?
      </h1>
      <Button disabled={!currentUser || isLoading} onClick={logoutUser}>
        {isLoading && <Spinner></Spinner>} Yes, Sign out
      </Button>
    </div>
  )
}
