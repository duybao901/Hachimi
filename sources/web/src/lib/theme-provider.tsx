import React, { useEffect, useState } from "react"
import { type Theme, ThemeContext } from "./theme-context"

const THEME_KEY = "theme"

interface ThemeProviderProps {
  children: React.ReactNode
}

export const ThemeProvider = ({ children }: ThemeProviderProps) => {
    
  const [theme, setTheme] = useState<Theme>(() => {
    try {
      return (localStorage.getItem(THEME_KEY) as Theme) ?? "system"
    } catch {
      return "system"
    }
  })

  const resolvedTheme: "light" | "dark" =
    theme === "system"
      ? window.matchMedia("(prefers-color-scheme: dark)").matches
        ? "dark"
        : "light"
      : theme

  // Update document class and persist theme changes
  useEffect(() => {
    const classList = document.documentElement.classList

    if (resolvedTheme === "dark") classList.add("dark")
    else classList.remove("dark")

    try {
      localStorage.setItem(THEME_KEY, theme)
    } catch {
      // ignore
    }

    if (theme === "system") {
      const media = window.matchMedia("(prefers-color-scheme: dark)")

      const handler = () => {
        if (media.matches) classList.add("dark")
        else classList.remove("dark")
      }

      media.addEventListener("change", handler)
      return () => media.removeEventListener("change", handler)
    }
  }, [theme, resolvedTheme])

  const toggle = () => {
    setTheme((prev) => (prev === "dark" ? "light" : "dark"))
  }

  return (
    <ThemeContext.Provider value={{ theme, resolvedTheme, setTheme, toggle }}>
      {children}
    </ThemeContext.Provider>
  )
}
