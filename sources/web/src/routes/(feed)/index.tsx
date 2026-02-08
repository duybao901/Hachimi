import { createFileRoute } from '@tanstack/react-router'

import PostFeeds from '@/components/Posts/PostFeeds'
import type { PostView } from '@/types/queries/Posts/post';

export const Route = createFileRoute('/(feed)/')({
  component: RouteComponent,
})

function RouteComponent() {

  const postsMock: PostView[] = [
    {
      id: "1",
      title: "5 Terminal Commands That Saved Me Hours of Clicking",
      slug: "5-terminal-commands-that-saved-me-hours-of-clicking",
      postAuthor: {
        id: "a1",
        name: "John Doe",
        username: "johndoe",
        avatarUrl:
          "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509116/Hachimi/GFaSsnub0AAXCNS_syuddp.jpg",
      },
      content: "This is the content of the first post.",
      postStatus: "Published",
      postTags: [
        { id: "t1", description: "", name: "discuss", color: "#2396F3" },
        { id: "t2", description: "", name: "webdev", color: "#3396A3" },
      ],
      coverImageUrl:
        "https://res.cloudinary.com/dxnfxl89q/image/upload/v1765901290/Hachimi/facebook_1678938669867_7041979178780517777_tjvdnr.jpg",
      isFirstPost: true,
      comments: [
        {
          id: "c1",
          content: "Great post! Really helped me out.",
          author: {
            id: "a1",
            name: "ffyom_7",
            username: "SarvarNadaf",
            avatarUrl:
              "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509108/Hachimi/rz_gytbsc.png",
          },
        },
      ],
      viewCount: 0,
      createdOnUtc: ""
    },
    {
      id: "2",
      title: "How to Become an AWS Community Builder",
      slug: "how-to-become-an-aws-community-builder",
      postAuthor: {
        id: "a1",
        name: "Sarvar Nadaf",
        username: "SarvarNadaf",
        avatarUrl:
          "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509112/Hachimi/Screenshot_2025-05-16_210725_xghzr8.png",
      },
      content: "This is the content of the second post.",
      postStatus: "Published",
      postTags: [
        { id: "t1", description: "", name: "discuss", color: "#2396F3" },
        { id: "t2", description: "", name: "webdev", color: "#3396A3" },
      ],
      isFirstPost: false,
      comments: [],
      viewCount: 0,
      createdOnUtc: ""
    },
    {
      id: "3",
      title:
        "Introducing ProXPL: A Modern Programming Language Built from Scratch",
      slug: "introducing-proxpl-a-modern-programming-language-built-from-scratch",
      postAuthor: {
        id: "a1",
        name: "Prog. Kanishk Raj",
        username: "Prog. Kanishk Raj",
        avatarUrl:
          "https://res.cloudinary.com/dxnfxl89q/image/upload/v1766509107/Hachimi/Screenshot_2025-10-25_015739_et58i0.png",
      },
      content: "This is the content of the second post.",
      postStatus: "Published",
      postTags: [
        { id: "t1", description: "", name: "discuss", color: "#2396F3" },
        { id: "t2", description: "", name: "webdev", color: "#3396A3" },
      ],
      isFirstPost: false,
      comments: [],
      viewCount: 0,
      createdOnUtc: ""
    },
  ]
  
  return <PostFeeds posts={postsMock}>

  </PostFeeds>;
}
