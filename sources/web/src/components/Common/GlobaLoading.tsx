import { Loader2 } from "lucide-react"
import { useGlobalLoading } from "@/store/globalLoading.store"

export function GlobalLoading() {
  const loading = useGlobalLoading((s) => s.loading)

  if (!loading) return null

  return (
    <div className="fixed inset-0 z-[9999] bg-background/80 backdrop-blur-sm flex items-center justify-center">
      <Loader2 className="h-8 w-8 animate-spin text-primary" />
    </div>
  )
}