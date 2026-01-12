import { queryApi } from '../api/query.api';
import { commandApiV1 } from '../api/command.api';
import type {CreatePostCommand} from '@/types/commands/Posts/posts'
import type { ApiResponse } from '@/types/api';
import type { PostCurrentEdit } from '@/types/queries/Posts/post';

export async function CreatePost(post: CreatePostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts`, { post });
} 

export async function GetCurrentEditPost() {
    return await queryApi.get<ApiResponse<CreatePostCommand>>('/posts/current-edit');
}