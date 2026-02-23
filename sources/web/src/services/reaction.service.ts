import { queryPublicApi } from '../api/query.api';
import type { ApiResponse } from '@/types/api';
import type { Reaction } from '@/types/queries/Reactions/reaction';

export async function GetPublicReactions() {
    return await queryPublicApi.get<ApiResponse<Reaction[]>>('/reactions/public');
}
