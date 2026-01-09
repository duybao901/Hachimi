import { create } from "zustand"
import { devtools } from "zustand/middleware"

import type { PostView } from "@/types/queries/Posts/post"
import type { CreatePostCommand } from "@/types/commands/Posts/posts"

type PostState = {
  posts: PostView[]
  currentEditPost: CreatePostCommand | null
}

type PostActions = {
  setCurrentEditPost: (post: CreatePostCommand | null) => void
  setCurrentEditPostContent: (content: string) => void
}

export const usePostStore = create<PostState & PostActions>()(
  devtools((set) => ({
    posts: [],
    currentEditPost: null,

    setCurrentEditPost: (post) =>
      set({ currentEditPost: post }),

    setCurrentEditPostContent: (content) =>
      set((state) => {
        if (!state.currentEditPost) return state

        return {
          ...state,
          currentEditPost: {
            ...state.currentEditPost,
            content,
          },
        }
      }),
  }))
)
