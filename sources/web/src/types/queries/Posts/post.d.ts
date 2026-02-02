import type { Author } from "../../author";

export interface Tag {
    id: string;
    name: string;
    description: string;
    color: string;
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
    postAuthor: {
        id: string;
        name: string;
        username: string;
        avatarUrl: string;
    };
    postStatus: "Published" | "Draft" | "Archived";
    coverImageUrl?: string;
    viewCount: number;
    readingTimeMinutes?: number;
    postTags: Tag[];
    createdOnUtc: string;
    modifiedOnUtc?: string;
    isFirstPost: boolean;
    comments: PostComment[] | []
}


