import type { PostView } from "@/types/queries/Posts/post"
import PostCard from "./PostCard"
import { useEffect, useState } from "react"
import type { ValidationErrorResponse } from "@/types/api"
import { extractValidationMessages } from "@/utils/extractValidationMessages"
import { toast } from "sonner"
import { GetPosts } from "@/services/post.service"
import { useAuthStore } from "@/store/auth.store"

type PostFeedsProps = {
  typeOf?: "relevant" | "lastest" | "top:week" | "top:month" | "top:all" | "discover" | "following"
}

const postsMock: PostView[] = [
  {
    id: "1",
    title: "5 Terminal Commands That Saved Me Hours of Clicking",
    slug: "5-terminal-commands-that-saved-me-hours-of-clicking",
    postAuthor: {
      id: "a1",
      name: "John Doe",
      username: "johndoe",
      avatarUrl:
        "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509116/Hachimi/GFaSsnub0AAXCNS_syuddp.jpg",
    },
    content: "This is the content of the first post.",
    postStatus: "Published",
    postTags: [
      { id: "t1", description: "", name: "discuss", color: "#2396F3" },
      { id: "t2", description: "", name: "webdev", color: "#3396A3" },
    ],
    coverImageUrl:
      "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/facebook_1678938669867_7041979178780517777_tjvdnr.jpg",
    isFirstPost: true,
    comments: [
      {
        id: "c1",
        content: "Great post! Really helped me out.",
        author: {
          id: "a1",
          name: "ffyom_7",
          username: "SarvarNadaf",
          avatarUrl:
            "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509108/Hachimi/rz_gytbsc.png",
        },
      },
    ],
    viewCount: 0,
    createdOnUtc: ""
  },
  {
    id: "2",
    title: "How to Become an AWS Community Builder",
    slug: "how-to-become-an-aws-community-builder",
    postAuthor: {
      id: "a1",
      name: "Sarvar Nadaf",
      username: "SarvarNadaf",
      avatarUrl:
        "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509112/Hachimi/Screenshot_2025-05-16_210725_xghzr8.png",
    },
    content: "This is the content of the second post.",
    postStatus: "Published",
    postTags: [
      { id: "t1", description: "", name: "discuss", color: "#2396F3" },
      { id: "t2", description: "", name: "webdev", color: "#3396A3" },
    ],
    isFirstPost: false,
    comments: [],
    viewCount: 0,
    createdOnUtc: ""
  },
  {
    id: "3",
    title:
      "Introducing ProXPL: A Modern Programming Language Built from Scratch",
    slug: "introducing-proxpl-a-modern-programming-language-built-from-scratch",
    postAuthor: {
      id: "a1",
      name: "Prog. Kanishk Raj",
      username: "Prog. Kanishk Raj",
      avatarUrl:
        "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509107/Hachimi/Screenshot_2025-10-25_015739_et58i0.png",
    },
    content: "This is the content of the second post.",
    postStatus: "Published",
    postTags: [
      { id: "t1", description: "", name: "discuss", color: "#2396F3" },
      { id: "t2", description: "", name: "webdev", color: "#3396A3" },
    ],
    isFirstPost: false,
    comments: [],
    viewCount: 0,
    createdOnUtc: ""
  },
]

function PostFeeds({ typeOf }: PostFeedsProps) {
  const [postsData, setPostsData] = useState<PostView[]>(postsMock);

  const { currentUser } = useAuthStore.getState();

  useEffect(() => {
    const fetchPosts = async () => {
      try {

        const pageIndex = 1;
        const pageSize = 10;
        const feed = typeOf || "relevant";

        const res = await GetPosts(pageIndex, pageSize, feed);

        console.log("Fetched posts:", res.data);

        if (res.data && res.data.value && res.data.value.items) {
          setPostsData([...postsMock,...res.data.value.items]);
        }

      } catch (error: any) {
        const data = error?.response?.data as ValidationErrorResponse | undefined

        if (data?.errors) {
          const messages = extractValidationMessages(data.errors)

          toast.error("Validation error", {
            description: (
              <ul className="list-disc pl-4">
                {messages.map((msg) => (
                  <li key={msg}>{msg}</li>
                ))}
              </ul>
            ),
          })
        } else {
          if (error.response) {
            toast.error(
              error.response?.data?.Detail || error.message || "Server error"
            )
          }
        }
      }
    }

    if (!currentUser) {
      fetchPosts()
    } else {
      // If user is logged in, you can implement fetching personalized feed here
      fetchPosts()
    }

  }, [currentUser]);

  return (
    <div>
      {postsData && postsData.map((post) => {
        return <PostCard post={post}></PostCard>
      })}
    </div>
  )
}

export default PostFeeds
