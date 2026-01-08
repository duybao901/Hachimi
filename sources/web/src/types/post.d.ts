import type { en } from "zod/v4/locales";
import type { Author } from "./author";

export enum PostStatus {
    Draft = "Draft",
    Published = "Published",
    Archived = "Archived"
}

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

export interface Post {
    id: string;
    title?: string;
    author: Author;
    content: string;
    createdOnUtc?: Date;
    modifiedOnUtc?: Date;
    slug: string;
    readingTimeMinutes?: number;
    viewCount: number | 0; 
    coverImageUrl?: string;
    PostStatus: PostStatus | PostStatus.Draft;    
    tags: Tag[]
    isFirstPost: boolean | false;
    comments: PostComment[];
}