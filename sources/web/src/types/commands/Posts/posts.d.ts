import type { PostStatus } from "@/types";

export interface DraftPost {
    title: string 
    content: string 
    tagList: string[]
    coverImageUrl: string
}

export interface CreatePostCommand {
    title: string;
    content: string;
    tagIds: string[];
}

export interface SaveDraftPostCommand {
    title: string;
    content: string;
    tagIds: string[];
    coverImageUrl: string;
}

export interface PublishPostCommand {
    title: string;
    content: string;
    tagIds: string[];
    coverImageUrl: string;
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