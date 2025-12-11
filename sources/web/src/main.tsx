import { createRoot } from 'react-dom/client'
import '@/styles/globals.css'
import { ThemeProvider } from "@/components/theme-provider"
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { Toaster } from "@/components/ui/sonner"

// Import the generated route tree
import { routeTree } from './routeTree.gen'

// Create a new router instance
const router = createRouter({ routeTree })

createRoot(document.getElementById('root')!).render(
  <ThemeProvider defaultTheme="light" storageKey="hachimi-ui-theme">
    <RouterProvider router={router} />
     <Toaster position="top-right" richColors  />
  </ThemeProvider>
)
