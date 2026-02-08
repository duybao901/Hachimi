import type { PostView } from "@/types/queries/Posts/post"
import PostCard from "./PostCard"
import { useEffect, useState } from "react"
import type { ValidationErrorResponse } from "@/types/api"
import { extractValidationMessages } from "@/utils/extractValidationMessages"
import { toast } from "sonner"
import { GetPosts } from "@/services/post.service"
import { useAuthStore } from "@/store/auth.store"

type PostFeedsProps = {
  posts: PostView[]
}

function PostFeeds({ posts }: PostFeedsProps) {
  const [postsData, setPostsData] = useState<PostView[]>(posts);

  const { currentUser } = useAuthStore.getState();

  useEffect(() => {
    const fetchPosts = async () => {
      try {

        const pageIndex = 1;
        const pageSize = 10;
        const feed = "relevant";

        const res = await GetPosts(pageIndex, pageSize, feed);

        console.log("Fetched posts:", res.data);

        if (res.data && res.data.value && res.data.value.items) {
          setPostsData([...posts, ...res.data.value.items]);
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

    if(!currentUser) {
      fetchPosts()
    }else {
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
