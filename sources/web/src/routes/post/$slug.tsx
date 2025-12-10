import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/post/$slug')({
  component: PostDetail,
})

function PostDetail() {
  const { slug } = Route.useParams()

  return (
    <article>
      <h1 className="text-2xl font-bold">@slug: {slug}</h1>
      {/* content */}
    </article>
  )
}
