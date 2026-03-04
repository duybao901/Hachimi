import { queryApi } from '../api/query.api';
import type { ApiResponse } from '@/types/api';
import type { AuthorStats } from '@/types/author';

export async function GetAuthorStats(authorId: string) {
    return await queryApi.get<ApiResponse<AuthorStats>>(`/authors/${authorId}/stats`);
}
