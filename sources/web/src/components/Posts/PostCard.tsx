import type { PostView } from "@/types/queries/Posts/post"
import { hexToRgb } from "@/utils/hexToRgb"
import { Link } from "@tanstack/react-router"
import { Button } from "../ui/button"
import {
  Bookmark as BookmarkIcon,
  MessageCircle as MessageCircleIcon,
} from "lucide-react"
import type { Tag } from "@/types/tag"

type PostCardProps = {
  post: PostView
}

function PostCard({ post }: PostCardProps) {
  return (
    <div className="bg-white rounded-sm border border-gray-200 mb-2 overflow-hidden">
      <div className="w-full">
        {post.isFirstPost && (
          <Link to={"/"}>
            <div
              className="w-full h-[250px] bg-cover bg-center"
              style={{ backgroundImage: `url(${post.coverImageUrl})` }}
            ></div>
          </Link>
        )}
        <div className="flex gap-2 p-4">
          <Link
            to={"/"}
            className="min-w-8 h-8 rounded-full overflow-hidden"
            style={{
              backgroundImage: `url(${post.author.avatarUrl})`,
              backgroundSize: "cover",
            }}
          ></Link>
          <div className="grow">
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
              <Link
                to={`/post/${post.slug}`}
                className="text-2xl font-extrabold mb-3 mt-3 block hover:text-primary"
              >
                {post.title}
              </Link>
              <div className="mt-2">
                {post.tags?.map((tag: any) => {
                  return (
                    <Link
                      to={`/tag/${tag.name}`}
                      className="text-(--link-color) text-[13px] px-2 py-1 rounded-sm mr-2 inline-block"
                      style={{
                        background: `rgba(${hexToRgb(tag.color!)}, 0.1)`,
                      }}
                      key={tag.id}
                    >
                      <span style={{ color: `${tag.color}` }}>#</span>
                      {tag.name}
                    </Link>
                  )
                })}
              </div>
              <div className="mt-2 w-full flex justify-between items-center -ml-2">
                <div className="flex justify-between items-center">
                  <Link
                    to={"/"}
                    className="mr-2 flex items-center px-2 py-1 rounded-sm hover:bg-gray-100"
                  >
                    <div className="flex">
                      <div className="-mr-2.5 rounded-full border-2 border-white bg-gray-100 p-1">
                        <img
                          src="https://assets.dev.to/assets/fire-f60e7a582391810302117f987b22a8ef04a2fe0df7e3258a5f49332df1cec71e.svg"
                          width="18"
                          height="18"
                          alt="Fire"
                        />
                      </div>
                      <div className="-mr-2.5 rounded-full border-2 border-white bg-gray-100 p-1">
                        <img
                          src="https://assets.dev.to/assets/exploding-head-daceb38d627e6ae9b730f36a1e390fca556a4289d5a41abb2c35068ad3e2c4b5.svg"
                          width="18"
                          height="18"
                          alt="Exploding Head"
                        />
                      </div>
                      <div className="-mr-2.5 rounded-full border-2 border-white bg-gray-100 p-1">
                        <img
                          src="https://assets.dev.to/assets/raised-hands-74b2099fd66a39f2d7eed9305ee0f4553df0eb7b4f11b01b6b1b499973048fe5.svg"
                          width="18"
                          height="18"
                          alt="Raised Hands"
                        />
                      </div>
                    </div>
                    <div className="ml-4 text-[13px] text-(--link-color)">
                      25 Reactions
                    </div>
                  </Link>
                  <Link
                    to={"/"}
                    className="flex justify-between items-center px-2 py-1 rounded-sm hover:bg-gray-100"
                  >
                    <MessageCircleIcon className="w-4 mr-1" />
                    {post.comments?.length > 0 ? <div className="text-[13px] text-(--link-color)">
                      7 Comments
                    </div> : <div className="text-[13px] text-(--link-color)">
                      Add comment
                    </div>}
                    
                  </Link>
                </div>
                <div className="flex justify-between items-center">
                  <p className="text-xs text-(--link-color-secondary) mr-2">
                    2 min read
                  </p>
                  <Button
                    variant="ghost"
                    className="font-light hover:text-primary hover:bg-primary/5"
                  >
                    <BookmarkIcon className="w-5" />
                  </Button>
                </div>
              </div>
            </div>
          </div>
        </div>
        {post.isFirstPost && post.comments && post.comments.length > 0 && (
          <div className="px-4">
            {post.comments!.map((comment) => (
              <div key={comment.id} className="flex items-start gap-2">
                <Link
                  to={"/"}
                  className="min-w-6 h-6 rounded-full overflow-hidden"
                  style={{
                    backgroundImage: `url(${comment.author.avatarUrl})`,
                    backgroundSize: "cover",
                  }}
                ></Link>
                <div className=" w-full">
                  <div className="bg-gray-50 p-3 rounded-sm mb-2">
                    <p className="text-sm font-medium text-(--base-80)">
                      {comment.author.name}
                    </p>
                    <p className="mt-2 text-xs text-gray-900">
                      {comment.content}
                    </p>
                  </div>
                  <Link
                    to={"/"}
                    className="text-xs font-medium text-(--base-80) flex items-center px-2 py-1 rounded-sm hover:bg-gray-100 cursor-pointer w-fit mb-2"
                  >
                    See all 4 comments
                  </Link>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}

export default PostCard
