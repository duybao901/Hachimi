import { useAuthStore } from "@/store/auth.store"
import { redirect } from "@tanstack/react-router"

export const guestGuard = () => {
    const { currentUser, authLoaded } = useAuthStore.getState();
    
    if (!authLoaded) return { preload: true }

    if (currentUser) {
        throw redirect({
            to: "/", // hoặc trang home của bạn
        })
    }
}