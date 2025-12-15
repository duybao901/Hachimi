import { createRouter } from "@tanstack/react-router"
import { routeTree } from "./routeTree.gen"
import { loadSessionOnInit } from "@/services/auth.service"
import { useAuthStore } from "@/store/auth.store"

await loadSessionOnInit()  

export const router = createRouter({
  routeTree,
  context: {
    auth: useAuthStore.getState(),
  },
})
