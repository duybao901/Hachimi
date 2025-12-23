import type { Post } from "@/types/post"
import { hexToRgb } from "@/utils/hexToRgb"
import { Link } from "@tanstack/react-router"
import { Button } from "../ui/button"
import {
  Bookmark as BookmarkIcon,
  MessageCircle as MessageCircleIcon,
} from "lucide-react"

type PostCardProps = {
  post: Post
}

function PostCard({ post }: PostCardProps) {
  return (
    <div className="bg-white rounded-sm border border-gray-200 mb-2 overflow-hidden">
      <div className="w-full">
        {post.isFirstPost && (
          <Link to={"/"}>
            <div
              className="w-full h-[250px] bg-cover bg-center"
              style={{ backgroundImage: `url(${post.CoverImageUrl})` }}
            ></div>
          </Link>
        )}
        <div className="flex gap-2 p-4">
          <img
            className="h-8 w-8 rounded-full"
            src={post.author.avatarUrl}
            alt={post.author.name}
          ></img>
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
              <Link to={"/"} className="text-2xl font-extrabold mb mt-2 block">
                {post.title}
              </Link>
              <div className="mt-2">
                {post.tags?.map((tag) => {
                  return (
                    <div
                      className="text-(--link-color) text-[13px] px-2 py-1 rounded-sm mr-2 inline-block"
                      style={{
                        background: `rgba(${hexToRgb(tag.color!)}, 0.1)`,
                      }}
                      key={tag.id}
                    >
                      <span style={{ color: `${tag.color}` }}>#</span>
                      {tag.name}
                    </div>
                  )
                })}
              </div>
              <div className="mt-2 w-full flex justify-between items-center -ml-2">
                <div className="flex justify-between items-center">
                  <Link to={"/"} className="mr-2 flex items-center px-2 py-1 rounded-sm hover:bg-gray-100">
                    <div className="flex">
                      <span className="-mr-2 rounded-full border-2 border-white bg-gray-100 p-1"><img src="https://assets.dev.to/assets/fire-f60e7a582391810302117f987b22a8ef04a2fe0df7e3258a5f49332df1cec71e.svg" width="18" height="18" alt="Fire" /></span>
                      <span className="-mr-2 rounded-full border-2 border-white bg-gray-100 p-1"><img src="https://assets.dev.to/assets/fire-f60e7a582391810302117f987b22a8ef04a2fe0df7e3258a5f49332df1cec71e.svg" width="18" height="18" alt="Fire" /></span>
                      <span className="-mr-2 rounded-full border-2 border-white bg-gray-100 p-1"><img src="https://assets.dev.to/assets/fire-f60e7a582391810302117f987b22a8ef04a2fe0df7e3258a5f49332df1cec71e.svg" width="18" height="18" alt="Fire" /></span>
                    </div>
                    <span className="ml-4 text-[13px] text-(--link-color) mt-1 ">25 Reactions</span>
                  </Link>
                  <Link to={'/'} className="flex justify-between items-center px-2 py-1 rounded-sm hover:bg-gray-100">
                    <MessageCircleIcon className="w-4 mr-1" />
                    <span className="text-[13px] text-(--link-color) mt-1 ">7 Comments</span>
                  </Link>
                </div>
                <div className="flex justify-between items-center">
                  <p className="text-xs text-(--link-color-secondary) mr-2">2 min read</p>
                  <Button variant="ghost" className="font-light">
                    <BookmarkIcon className="w-5" />
                  </Button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default PostCard
