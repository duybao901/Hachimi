import { queryApi } from '../api/query.api';
import { commandApiV1 } from '../api/command.api';
import type {CreatePostCommand, GetOrCreateDraftPostResponse, UpdatePostCommand} from '@/types/commands/Posts/posts'
import type { ApiResponse } from '@/types/api';

export async function CreatePost(post: CreatePostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts`, { post });
} 

export async function GetCurrentEditPost() {
    return await queryApi.get<ApiResponse<CreatePostCommand>>('/posts/current-edit');
}

export async function GetOrCreateDraftPost() {
    return await commandApiV1.get<ApiResponse<GetOrCreateDraftPostResponse>>('/posts/draft');
}

export async function UpdatePost(postId: string ,data: UpdatePostCommand) {
    return await commandApiV1.put<ApiResponse<string>>(`/posts/${postId}`, data);
}

export async function PushPost(postId: string) {

}