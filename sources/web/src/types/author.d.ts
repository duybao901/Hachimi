export interface Author {
    id: string;
    name: string;
    username: string;
    avatarUrl?: string; 
    Bio?: string;    
}

export interface AuthorStats {
    totalReactions: number;
    totalComments: number;
    totalViews: number;
    totalPosts: number;
    totalDrafts: number;
}