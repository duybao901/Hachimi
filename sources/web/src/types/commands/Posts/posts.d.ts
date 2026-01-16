import type { PostStatus } from "@/types";

export interface CreatePostCommand {
    title: string;
    content: string;
    authorId: string;
    tagIds: string[];
}

export interface UpdatePostCommand {
    id: string;
    title: string;
    content: string;
    coverImageUrl: string;
    tagIds: string[];
}

export interface GetOrCreateDraftPostResponse {
    id: string
    title: string
    content: string
    tagIds: string[]
    userId: string
    coverImageUrl: string
    postStatus: PostStatus
}