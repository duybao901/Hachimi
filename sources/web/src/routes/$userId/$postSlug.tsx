import Header from '@/components/Layout/Header/Header'
import { GetPostBySlug } from '@/services/post.service';
import type { ValidationErrorResponse } from '@/types/api';
import type { PostView } from '@/types/queries/Posts/post';
import { extractValidationMessages } from '@/utils/extractValidationMessages';
import { createFileRoute } from '@tanstack/react-router'
import { useParams } from '@tanstack/react-router'
import { useEffect, useState } from 'react';
import { toast } from 'sonner';

export const Route = createFileRoute('/$userId/$postSlug')({
  component: RouteComponent,
})

function RouteComponent() {

  const [post, setPost] = useState<PostView>();

  const params = useParams({
    from: '/$userId/$postSlug',
  });

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

  return <div>
    <Header></Header>
    <div className="bg-gray-50 py-2 min-h-screen">
      <div className="w-7xl px-4 gap-2 m-auto">
        {
          post ?
            <div className="bg-white rounded-sm shadow-sm p-6">
              <h1 className="text-3xl font-extrabold mb-4">{post.title}</h1>
              <div className="prose prose-lg max-w-none">
                {/* <div dangerouslySetInnerHTML={{ __html: post.contentHtml }}></div> */}
              </div>
              <div />
            </div>
            :
            <div>Loading post...</div>
        }
      </div>
    </div>
  </div >
}
