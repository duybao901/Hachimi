import type { Post } from "@/types/post"
import PostCard from "./PostCard"

function PostLists() {
  const posts: Post[] = [
    {
      id: "1",
      title: "5 Terminal Commands That Saved Me Hours of Clicking",
      author: {
        id: "a1",
        name: "John Doe",
        username: "johndoe",
        avatarUrl:
          "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/capybara_vwpmqp.png",
      },
      content: "This is the content of the first post.",
      IsPublished: true,
      tags: [
        { id: "t1", name: "discuss", color: "#2396F3" },
        { id: "t2", name: "webdev", color: "#3396A3" },
      ],
      CoverImageUrl:
        "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/facebook_1678938669867_7041979178780517777_tjvdnr.jpg",
      isFirstPost: true,
    },
    {
      id: "2",
      title: "How to Become an AWS Community Builder",
      author: {
        id: "a1",
        name: "Sarvar Nadaf",
        username: "SarvarNadaf",
        avatarUrl:
          "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/capybara_vwpmqp.png",
      },
      content: "This is the content of the second post.",
      IsPublished: true,
      tags: [
        { id: "t1", name: "discuss", color: "#2396F3" },
        { id: "t2", name: "webdev", color: "#3396A3" },
      ],
    },
  ]

  return (
    <div>
      {posts.map((post) => {
        return <PostCard post={post}></PostCard>
      })}
    </div>
  )
}

export default PostLists
