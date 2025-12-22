import type { Post } from "@/types/post";
import PostCard from "./PostCard";

function PostLists() {

    const posts: Post[] = [
        {
            id: "1",
            title: "First Post",
            author: {
                id: "a1",
                name: "John Doe",
                username: "johndoe",
                avatarUrl: "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/capybara_vwpmqp.png"
            },
            content: "This is the content of the first post.",
            IsPublished: true,
            tags: [
                { id: "t1", name: "discuss" },
                { id: "t2", name: "webdev" }
            ]
        },
        {
            id: "2",
            title: "Second Post",
            author: {
                id: "a1",
                name: "John Doe",
                username: "johndoe",
                avatarUrl: "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/capybara_vwpmqp.png"
            },
            content: "This is the content of the second post.",
            IsPublished: true,
            tags: [
                { id: "t1", name: "discuss" },
                { id: "t2", name: "webdev" }
            ]
        },
    ];

    return <div>
        {posts.map((post) => {
            return <PostCard post={post}></PostCard>
        })}
    </div>;
}

export default PostLists;