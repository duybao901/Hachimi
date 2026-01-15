import type { PostStatus } from "@/types";

export interface CreatePostCommand {
    title: string;
    content: string;
    authorId: string;
    tagIds: string[];

}

export interface UpdatePostContentCommand {
    id: string;
    title: string;
    content: string;
}

export interface UpdatePostTagsCommand {
    id: string;
    tagIds: string[];
}

export interface PostEditorState {
    title: string;
    content: string;
    tags: Tag[];
    isDirty: boolean;
}

export interface GetOrCreateDraftPostResponse {
    id: string
    title: string
    content: string
    tagIds: string[]
    userId: string
    postStatus: PostStatus
}