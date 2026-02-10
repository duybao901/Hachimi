import Header from '@/components/Layout/Header/Header'
import { GetPostBySlug } from '@/services/post.service';
import type { ValidationErrorResponse } from '@/types/api';
import { extractValidationMessages } from '@/utils/extractValidationMessages';
import { createFileRoute } from '@tanstack/react-router'
import { useParams } from '@tanstack/react-router'
import { useEffect } from 'react';
import { toast } from 'sonner';

export const Route = createFileRoute('/$userId/$postSlug')({
  component: RouteComponent,
})

function RouteComponent() {

  const params = useParams({
    from: '/$userId/$postSlug',
  });

  useEffect(() => {
    const fetchPostBySlug = async () => {
      try {
        const postSlug = params.postSlug;
        const res = await GetPostBySlug(postSlug);
        console.log("Fetched post by slug:", res);
        
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
  })

  return <div>
    <Header></Header>
    <div className="bg-gray-50 py-2 min-h-screen">
      <div className="w-7xl px-4 gap-2 m-auto">
        Hello "/$userId/$postSlug/"!
      </div>
    </div>
  </div>
}
