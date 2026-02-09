import { createFileRoute } from '@tanstack/react-router'

import PostFeeds from '@/components/Posts/PostFeeds'
import { useAuthStore } from '@/store/auth.store';

export const Route = createFileRoute('/(feed)/')({
  component: RouteComponent,
})

function RouteComponent() {

  const { currentUser } = useAuthStore.getState();

  return <PostFeeds typeOf={currentUser ? "discover" : "relevant"} >

  </PostFeeds>;
}
