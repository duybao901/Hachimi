import Header from '@/components/Layout/Header/Header'
import { GetPostBySlug, ReactToPost } from '@/services/post.service';
import { MarkdownRenderer } from '@/components/Common/MarkdownRenderer';
import type { ValidationErrorResponse } from '@/types/api';
import type { PostView } from '@/types/queries/Posts/post';
import { extractValidationMessages } from '@/utils/extractValidationMessages';
import { createFileRoute, Link, useNavigate } from '@tanstack/react-router'
import { useParams } from '@tanstack/react-router'
import { useEffect, useState } from 'react';
import { toast } from 'sonner';
import { BookmarkIcon, MessageCircleIcon, ShareIcon } from 'lucide-react'
import { ReactionSelector } from '@/components/Posts/ReactionSelector';
import { useAuthStore } from '@/store/auth.store';
import { Button } from '@/components/ui/button';
import { formatPostDate } from '@/utils/dateUtils';
import { Dialog, DialogContent, DialogDescription, DialogTitle, DialogHeader } from '@/components/ui/dialog';

export const Route = createFileRoute('/$userId/$postSlug')({
  component: RouteComponent,
})

function RouteComponent() {

  const [post, setPost] = useState<PostView>();
  const { currentUser } = useAuthStore();

  const params = useParams({
    from: '/$userId/$postSlug',
  });

  const navigate = useNavigate();

  const [loginDialogOpen, setLoginDialogOpen] = useState(false);

  useEffect(() => {
    const fetchPostBySlug = async () => {
      try {
        const postSlug = params.postSlug;
        const res = await GetPostBySlug(postSlug);
        console.log("Fetched post by slug:", res);

        if (res.data && res.data.value) {
          setPost(res.data.value);
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

    fetchPostBySlug();
  }, [params.postSlug]);

  useEffect(() => {
    if (post && post.isPublished === false) {
      if (!currentUser || currentUser.id !== post.postAuthor.id) {
        toast.error("Bạn không có quyền truy cập vào bản nháp này.");
        navigate({ to: "/" });
      }
    }

  }, [post, currentUser, navigate]);



  const handleReact = async (
    postId: string,
    userId: string,
    reactionTypeId: string
  ) => {

    if (currentUser == null) {
      setLoginDialogOpen(true);
      return;
    }

    if (!post) return;

    try {
      const updatedReactions = post.reactions.map(r => {
        if (r.id === reactionTypeId) {
          const wasAlreadyCurrent = r.isReactionByCurrentUser;

          return {
            ...r,
            count: wasAlreadyCurrent
              ? Math.max(0, r.count - 1)
              : r.count + 1,
            isReactionByCurrentUser: !wasAlreadyCurrent
          };
        }

        return r;
      });


      setPost(prev =>
        prev ? { ...prev, reactions: updatedReactions } : prev
      );

      await ReactToPost(postId, userId, reactionTypeId);

    } catch (error) {
      toast.error("Failed to add reaction");

      const res = await GetPostBySlug(params.postSlug);
      if (res.data?.value) setPost(res.data.value);
    }
  };


  return <div>
    <Header></Header>
    <div className="bg-gray-50 py-2 min-h-screen">
      <div className="w-7xl px-4 gap-2 m-auto">
        {
          post ? (
            <div className="max-w-7xl mx-auto px-4 py-6">
              <div className="flex gap-6">

                {/* LEFT ACTION BAR */}
                <div className="hidden lg:block w-16">
                  <div className="sticky top-24 flex flex-col items-center gap-6 text-gray-500">

                    <ReactionSelector
                      postId={post.id}
                      side="right"
                      showCount={false}
                      reactions={post.reactions}
                      onReact={handleReact}
                    />
                    <span className="text-sm -mt-4">
                      {post.reactions.reduce((acc, curr) => acc + curr.count, 0)}
                    </span>

                    <button className="flex flex-col items-center hover:text-blue-500 transition cursor-pointer">
                      <MessageCircleIcon className="w-6 h-6" />
                      <span className="text-sm mt-1">4</span>
                    </button>

                    <button className="flex flex-col items-center hover:text-yellow-500 transition cursor-pointer">
                      <BookmarkIcon className="w-6 h-6" />
                      <span className="text-sm mt-1">3</span>
                    </button>

                    <button className="flex flex-col items-center hover:text-gray-700 transition cursor-pointer">
                      <ShareIcon className="w-6 h-6" />
                    </button>

                  </div>
                </div>

                {/* MAIN CONTENT */}
                <div className="flex-1 max-w-3xl">
                  {
                    post.isPublished === false && <div className="bg-yellow-100 border-l-4 border-yellow-500 p-4 mb-6">
                      <p className="text-sm text-yellow-700 ">Unpublished Post. This URL is public but secret, so share at your own discretion.
                        <Link to={`/new`} className="text-primary font-semibold ml-2">Click to edit</Link>
                      </p>
                    </div>
                  }
                  <article className="bg-white rounded-lg shadow-sm">
                    {/* Cover Image */}
                    {post.coverImageUrl && (
                      <img
                        src={post.coverImageUrl}
                        alt={post.title}
                        className="w-full object-cover max-h-[420px] rounded-t-lg"
                      />
                    )}

                    <div className="p-8">
                      {/* Author */}
                      <div className="flex items-center gap-3 mb-6">
                        <img
                          src={post.postAuthor.avatarUrl}
                          alt=""
                          className="w-10 h-10 rounded-full"
                        />
                        <div>
                          <div className="font-semibold">
                            {post.postAuthor.name}
                          </div>
                          <div className="text-xs text-gray-500">
                            {post.publishedAt ? formatPostDate(post.publishedAt) : "Draft"}
                          </div>
                        </div>
                      </div>


                      {/* Title */}
                      <h1 className="text-4xl font-extrabold mb-6">
                        {post.title}
                      </h1>

                      {/* Reactions */}
                      {
                        post.reactions.length > 0 && <div className="flex items-center gap-4 mb-6">
                          {post.reactions.map(r => (
                            <Button variant={"ghost"} key={r.id} className="flex items-center gap-2" onClick={() => handleReact(post.id, currentUser?.id ?? "", r.id)}>
                              <img src={r.url} alt={r.name} className="w-8 h-8" />
                              <span className="text-sm text-gray-500">{r.count}</span>
                            </Button>
                          ))}
                        </div>
                      }


                      {/* Tags */}
                      <div className="flex flex-wrap gap-2 mb-6">
                        {post.postTags.map(tag => (
                          <span
                            key={tag.id}
                            className="text-hover:underline cursor-pointer"
                          >
                            <span style={{ color: tag.color }}>#</span>{tag.name}
                          </span>
                        ))}
                      </div>

                      {/* Content */}
                      <MarkdownRenderer content={post.content} />

                      {/* COMMENTS SECTION */}
                      <div className="mt-16">

                        {/* Comment Header */}
                        <h2 className="text-2xl font-bold mb-6">
                          {post.comments?.length || 0} Comments
                        </h2>

                        {/* Write Comment */}
                        <div className="bg-gray-50 border rounded-lg p-4 mb-8">
                          <textarea
                            placeholder="Add to the discussion..."
                            className="w-full bg-white border rounded-md p-3 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            rows={4}
                          />
                          <div className="flex justify-end mt-3">
                            <button className="bg-primary text-white px-4 py-2 rounded-md hover:bg-primary/90 transition cursor-pointer">
                              Submit
                            </button>
                          </div>
                        </div>

                        {/* Comment List */}
                        <div className="space-y-8">
                          {post.comments?.map((comment) => (
                            <div key={comment.id} className="flex gap-4">

                              {/* Avatar */}
                              <img
                                src={comment.author.avatarUrl}
                                alt=""
                                className="w-9 h-9 rounded-full mt-1"
                              />

                              {/* Comment Body */}
                              <div className="flex-1">

                                <div className="bg-white border rounded-lg p-4 shadow-sm">

                                  {/* Author + Date */}
                                  <div className="flex items-center justify-between mb-2">
                                    <div className="font-semibold">
                                      {comment.author.name}
                                    </div>
                                    <div className="text-xs text-gray-500">
                                      {/* {new Date(comment.createdAt).toLocaleDateString()} */}
                                    </div>
                                  </div>

                                  {/* Content */}
                                  <div className="text-gray-800 text-sm leading-relaxed">
                                    {comment.content}
                                  </div>

                                </div>

                                {/* Comment Actions */}
                                <div className="flex items-center gap-6 text-sm text-gray-500 mt-2">

                                  <button className="hover:text-red-500 transition flex items-center gap-1">
                                    {/* ❤️ <span>{comment.likes || 0}</span> */}
                                  </button>

                                  <button className="hover:text-blue-500 transition">
                                    Reply
                                  </button>

                                </div>

                                {/* Replies */}
                                {/* {comment.replies?.length > 0 && (
                                <div className="mt-6 ml-8 space-y-6 border-l pl-6">
                                  {comment.replies.map((reply) => (
                                    <div key={reply.id} className="flex gap-4">

                                      <img
                                        src={reply.author.avatarUrl}
                                        alt=""
                                        className="w-8 h-8 rounded-full mt-1"
                                      />

                                      <div className="flex-1">
                                        <div className="bg-gray-50 border rounded-lg p-4">

                                          <div className="flex items-center justify-between mb-2">
                                            <div className="font-semibold text-sm">
                                              {reply.author.name}
                                            </div>
                                            <div className="text-xs text-gray-500">
                                              {new Date(reply.createdAt).toLocaleDateString()}
                                            </div>
                                          </div>

                                          <div className="text-sm text-gray-800">
                                            {reply.content}
                                          </div>

                                        </div>

                                        <div className="text-xs text-gray-500 mt-2">
                                          ❤️ {reply.likes || 0}
                                        </div>

                                      </div>
                                    </div>
                                  ))}
                                </div>
                              )} */}

                              </div>
                            </div>
                          ))}
                        </div>

                      </div>
                    </div>
                  </article>
                </div>

                {/* RIGHT SIDEBAR */}
                <aside className="hidden xl:block w-80">
                  <div className="sticky top-0 space-y-4">

                    <div className="bg-white rounded-lg shadow-sm p-6">
                      <h3 className="font-bold mb-2">About the Author</h3>
                      <p className="text-sm text-gray-600">
                        {/* {post.author.bio} */}
                      </p>
                    </div>

                    <div className="bg-white rounded-lg shadow-sm p-6">
                      <h3 className="font-bold mb-2">More from {post.postAuthor.name}</h3>
                      <ul className="text-sm space-y-2">
                        <li className="hover:underline cursor-pointer">
                          Related Post 1
                        </li>
                        <li className="hover:underline cursor-pointer">
                          Related Post 2
                        </li>
                      </ul>
                    </div>

                  </div>
                </aside>

              </div>
            </div>
          ) : (
            <div className="text-center py-20 text-gray-500">
              Loading post...
            </div>
          )
        }

      </div>
    </div>
    <Dialog open={loginDialogOpen} onOpenChange={setLoginDialogOpen}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>You are not logged in</DialogTitle>
          <DialogDescription>
            You need to login to react to posts.
          </DialogDescription>
        </DialogHeader>

        <div className="flex justify-end gap-2 mt-4">
          <Button variant="outline" onClick={() => setLoginDialogOpen(false)}>
            Cancel
          </Button>
          <Button onClick={() => navigate({ to: '/auth/login' })}>
            Go to Login
          </Button>
        </div>
      </DialogContent>
    </Dialog>
  </div >
}
