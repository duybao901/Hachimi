import type { en } from "zod/v4/locales";
import type { Author } from "../../author";

export type PostStatus = "Draft" | "Published" | "Archived";

export interface Tag {
    id: string;
    name: string;
    color?: string;
}

export interface PostComment {
    id: string;
    content: string;
    author: Author;
    CreatedOnUtc?: Date;
}

export interface PostView {
    id: string;
    title: string;
    slug: string;
    content: string;
    author: {
        id: string;
        name: string;
        username: string;
        avatarUrl: string;
    };
    postStatus: PostStatus;
    coverImageUrl?: string;
    viewCount: number;
    readingTimeMinutes?: number;
    tags: Tag[];
    createdOnUtc: string;
    modifiedOnUtc?: string;
    isFirstPost: boolean;
    comments: PostComment[] | []
}



