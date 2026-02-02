import { queryApi, queryPublicApi } from '../api/query.api';
import { commandApiV1 } from '../api/command.api';
import type {CreatePostCommand, PublishPostCommand, SaveDraftPostCommand, UpdatePostCommand} from '@/types/commands/Posts/posts'
import type { ApiResponse, PagedResult } from '@/types/api';
import type { PostView } from '@/types/queries/Posts/post';

export async function CreatePost(post: CreatePostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts`, { post });
} 

export async function GetCurrentEditPost() {
    return await queryApi.get<ApiResponse<CreatePostCommand>>('/posts/current-edit');
}

export async function SaveDraftPost(data: SaveDraftPostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts/save-draft`, data);
}

export async function UpdatePost(postId: string, data: UpdatePostCommand) {
    return await commandApiV1.post<ApiResponse<string>>(`/posts/${postId}`, data)
}

export async function PublishPost(data: PublishPostCommand){
    return await commandApiV1.post<ApiResponse<string>>(`/posts/publish`, data);
}

export async function GetPosts(pageIndex: number, pageSize: number, feed: string = "relevant") {
    return await queryPublicApi.get<PagedResult<PostView>>(`/posts/public?pageIndex=${pageIndex}&pageSize=${pageSize}&feed=${feed}`);
}