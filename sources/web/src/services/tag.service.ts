import type { Tag } from '@/types/tag';
import { queryApi } from '../api/query.api';
import type { ApiResponse } from '@/types/api';

export async function fetchSearchTags(searchTerm: string = '') {
  return queryApi.get<ApiResponse<Tag[]>>(`/tags`, {
    params: { searchTerm },
  })
}
