import { queryApi } from '../api/query.api';
import { commandApiV1 } from '../api/command.api';
import type {CreatePostCommand, SaveDraftPostCommand, UpdatePostCommand} from '@/types/commands/Posts/posts'
import type { ApiResponse } from '@/types/api';

export async function CreatePost(post: CreatePostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts`, { post });
} 

export async function GetCurrentEditPost() {
    return await queryApi.get<ApiResponse<CreatePostCommand>>('/posts/current-edit');
}

export async function SaveDraftPost(data: SaveDraftPostCommand) {
    return await commandApiV1.post(`/posts/save-draft`, data);
}

export async function UpdatePost(postId: string, data: UpdatePostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts/${postId}`, data)
}