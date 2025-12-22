import type { Author } from "./author";

export interface Tag {
    id: string;
    name: string;
    color?: string;
}

export interface Post {
    id: string;
    title?: string;
    author: Author;
    content: string;
    CreatedOnUtc?: Date;
    ModifiedOnUtc?: Date;
    Slug?: string;
    ReadingTimeMinutes?: number;
    ViewCount?: number;
    CoverImageUrl?: string;
    IsPublished: boolean;
    tags?: Tag[]
    isFirstPost?: boolean;
}

// public string Title { get; set; }
// public string Content { get; set; }
// public string? Slug { get; set; }
// public string? CoverImageUrl { get; set; }
// public bool IsPublished { get; set; }
// public int? ViewCount { get; set; }
// public int? ReadingTimeMinutes { get; set; }
// public Guid AuthorId { get; set; }
// public DateTimeOffset CreatedOnUtc { get; set; }
// public DateTimeOffset? ModifiedOnUtc { get; set; }