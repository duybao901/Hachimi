import PostFeeds from '@/components/Posts/PostFeeds';
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/(feed)/discover/')({
  component: RouteComponent,
})

function RouteComponent() {
  return <PostFeeds typeOf="discover" />
}
