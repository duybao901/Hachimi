import { createFileRoute, Link } from "@tanstack/react-router"
import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { MoreHorizontal } from "lucide-react"
import { useAuthStore } from "@/store/auth.store"
import { useEffect, useState } from "react"
import { GetAuthorUnpublishedPosts } from "@/services/post.service"
import type { PostView } from "@/types/queries/Posts/post"

export const Route = createFileRoute("/(header_layout)/dashboard/")({
    component: RouteComponent,
})

function RouteComponent() {

    const [unpublishedPosts, setUnpublishedPosts] = useState<PostView[]>([]);

    const { currentUser } = useAuthStore.getState();

    useEffect(() => {
        const fetchUnpublishedPosts = async () => {
            if (currentUser) {
                try {
                    const res = await GetAuthorUnpublishedPosts(currentUser.id);
                    if(res.data.isSuccess){
                        setUnpublishedPosts(res.data.value);
                    }
                } catch (error) {

                }
            }
        }

        fetchUnpublishedPosts();
    }, [currentUser])

    return (
        <div className="p-6 space-y-6">

            {/* Header */}
            <div className="flex items-center justify-between">
                <h1 className="text-3xl font-bold tracking-tight">Dashboard</h1>
                <Button>Create Post</Button>
            </div>

            {/* Stats */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <Card className="rounded-2xl shadow-sm">
                    <CardContent className="p-6">
                        <p className="text-muted-foreground text-sm">Total Reactions</p>
                        <h2 className="text-3xl font-bold mt-2">0</h2>
                    </CardContent>
                </Card>

                <Card className="rounded-2xl shadow-sm">
                    <CardContent className="p-6">
                        <p className="text-muted-foreground text-sm">Total Comments</p>
                        <h2 className="text-3xl font-bold mt-2">0</h2>
                    </CardContent>
                </Card>

                <Card className="rounded-2xl shadow-sm">
                    <CardContent className="p-6">
                        <p className="text-muted-foreground text-sm">Total Views</p>
                        <h2 className="text-3xl font-bold mt-2">500</h2>
                    </CardContent>
                </Card>
            </div>

            {/* Main content */}
            <div className="grid grid-cols-1 md:grid-cols-4 gap-6">

                {/* Sidebar */}
                <div className="space-y-2">
                    <SidebarItem label="Posts" count={2} active />
                    <SidebarItem label="Series" count={0} />
                    <SidebarItem label="Followers" count={0} />
                    <SidebarItem label="Following Tags" count={0} />
                    <SidebarItem label="Analytics" />
                </div>

                {/* Posts List */}
                <div className="md:col-span-3 space-y-4">

                    <div className="flex items-center justify-between">
                        <h2 className="text-xl font-semibold">Posts</h2>
                        <Button variant="outline">Recently Created</Button>
                    </div>

                    {
                        unpublishedPosts.length !== 0 ? <>
                            {
                                unpublishedPosts.map((post) => (
                                    <PostRow key={post.id} title={post.title} slug={post.slug} userName={post.postAuthor.userName}/>
                                ))
                            }
                        </> : 
                        <div className="text-center py-8 text-muted-foreground">
                            No unpublished posts yet.
                        </div>
                    }
                </div>
            </div>
        </div>
    )
}

function SidebarItem({
    label,
    count,
    active,
}: {
    label: string
    count?: number
    active?: boolean
}) {
    return (
        <Link
            to="/dashboard"
            className={`flex items-center justify-between px-4 py-2 rounded text-sm transition
        ${active
                    ? "bg-primary text-primary-foreground"
                    : "hover:bg-muted"
                }`}
        >
            <span>{label}</span>
            {count !== undefined && (
                <Badge variant="secondary">{count}</Badge>
            )}
        </Link>
    )
}

function PostRow({ title,slug, userName }: { title: string; slug: string; userName: string }) {
    return (
        <Card className="rounded-2xl shadow-sm p-4">
            <CardContent className="flex items-center justify-between">
                <div className="space-y-1">
                    <Link to={`/${userName}/${slug}`} className="flex items-center gap-2">
                        <h3 className="text-primary text-2xl font-bold cursor-pointer">
                            {title}
                        </h3>
                        <Badge variant="outline">Draft</Badge>
                    </Link>
                </div>

                <div className="flex items-center gap-4">
                    <Button variant="ghost" size="sm" className="text-destructive">
                        Delete
                    </Button>
                    <Button variant="ghost" size="sm">
                        Edit
                    </Button>
                    <MoreHorizontal className="size-4 text-muted-foreground cursor-pointer" />
                </div>
            </CardContent>
        </Card>
    )
}