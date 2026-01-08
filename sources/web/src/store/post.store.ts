import { create } from "zustand"
import { devtools } from "zustand/middleware"

import { Post } from '@/types/post'
import { Author } from "@/types/author"

type PostState = {
    posts: Post[]
    currentEditPost: Post
}

type PostActionss = {
    setCurrentEditPost: (post: Post | null) => void
}