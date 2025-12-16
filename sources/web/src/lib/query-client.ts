import { QueryClient } from '@tanstack/react-query'

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 5 * 60 * 1000,        // Cache “tươi” 5 phút
      gcTime: 30 * 60 * 1000,          // Garbage collect sau 30 phút
      refetchOnWindowFocus: false,     // Không tự fetch lại khi focus browser
      refetchOnReconnect: true,
      retry: 0,                        // Retry {} lần nếu lỗi
    },
    mutations: {
      retry: 0, // Mutation KHÔNG retry
    }
  },
})