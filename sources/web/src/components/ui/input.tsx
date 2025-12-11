import * as React from "react"

import { cn } from "@/lib/utils"

function Input({ className, type, ...props }: React.ComponentProps<"input">) {
  return (
    <input
      type={type}
      className={cn(
        // Base dev.to style
        "w-full rounded border border-gray-300 bg-white px-2 py-2 text-sm",
        "placeholder:text-gray-400",
        "focus:outline-none focus:ring-1 focus:ring-primary focus:border-primary",
        "transition-all",

        // Hover like dev.to
        "hover:border-gray-400",

        // Disabled state
        "disabled:opacity-60 disabled:cursor-not-allowed",

        // Dark mode
        "dark:bg-gray-900 dark:border-gray-700 dark:text-gray-100 dark:placeholder:text-gray-500",
        "dark:focus:ring-blue-400 dark:focus:border-blue-400",

        className
      )}
      {...props}
    />
  )
}

export { Input }
