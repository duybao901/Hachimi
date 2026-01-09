import type { Tag } from '@/types/tag';
import { queryApi } from '../api/query.api';
import { commandApiV1 } from '../api/command.api';
import type {CreatePostCommand} from '@/types/commands/Posts/posts'

export async function CreatePost(post: CreatePostCommand) {
    return await commandApiV1.post(`/posts`, { post });
} 