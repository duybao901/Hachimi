import { createRoot } from 'react-dom/client'
import '@/styles/globals.css'
import { ThemeProvider } from "@/components/theme-provider"
import { RouterProvider } from '@tanstack/react-router'
import { Toaster } from "@/components/ui/sonner"
import { QueryClientProvider } from '@tanstack/react-query'
import { queryClient } from './lib/query-client'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { router } from "./router"

createRoot(document.getElementById('root')!).render(
  <QueryClientProvider client={queryClient}>
    <ThemeProvider defaultTheme="light" storageKey="hachimi-ui-theme">
      <RouterProvider router={router} />
      <Toaster position="bottom-right" richColors />
    </ThemeProvider>
    <ReactQueryDevtools initialIsOpen={false} />
  </QueryClientProvider>
)
