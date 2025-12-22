import type { Post } from "@/types/post"
import { Link } from "@tanstack/react-router"

type PostCardProps = {
  post: Post
}

function PostCard({ post }: PostCardProps) {
  return (
    <div className="bg-white rounded-sm border border-gray-200 mb-2 overflow-hidden">
      <div className="">
        {post.isFirstPost && (
          <Link to={'/'}>
            <div
              className="w-full h-[250px] bg-cover bg-center"
              style={{ backgroundImage: `url(${post.CoverImageUrl})` }}
            ></div>
          </Link>
        )}
        <div className="flex gap-2 p-4 ">
          <img
            className="h-8 w-8 rounded-full"
            src={post.author.avatarUrl}
            alt={post.author.name}
          ></img>
          <div>
            <div className="h-8 flex flex-col justify-between">
              <Link
                to={"/"}
                className="text-sm font-medium text-(--link-color)"
              >
                {post.author.name}
              </Link>
              <div className="text-xs text-(--link-color-secondary)">
                Dec 19 (3 days ago)
              </div>
            </div>

            <div>
              <Link to={"/"} className="text-2xl font-extrabold mb mt-2 block">
                {post.title}
              </Link>
              <div>
                {post.tags?.map((tag) => {
                  return (
                    <div className="bg-gray-100 text-(--link-color) text-xs px-2 py-1 rounded-sm mr-2 inline-block">
                      #{tag.name}
                    </div>
                  )
                })}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default PostCard
