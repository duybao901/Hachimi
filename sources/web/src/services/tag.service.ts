import type { Tag } from '@/types/tag';
import { queryApi } from '../api/query.api';

export async function fetchSearchTags(searchTerm: string = '') {
    return await queryApi.get(`/tags`, {
        params: { searchTerm }
    });
} 