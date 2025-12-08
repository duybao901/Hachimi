import { createRoot } from 'react-dom/client'
import App from './App.tsx'
import { ThemeProvider } from "./lib/theme-provider";

createRoot(document.getElementById('root')!).render(
  <ThemeProvider>
    <App />
  </ThemeProvider>
)
