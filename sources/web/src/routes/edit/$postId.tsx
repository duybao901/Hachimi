import { createFileRoute, Link, useNavigate, useParams } from "@tanstack/react-router"
import Logo from "@/assets/horse_logo.png"
import {
    X as XIcon,
    ClipboardClock as ClipboardClockIcon,
    ImageIcon,
    ChevronLeft,
    Save,
    Send,
    Eye,
    Edit3,
    Sparkles,
    Video
} from "lucide-react"
import { Button } from "@/components/ui/button"
import { useEffect, useRef, useState } from "react"
import { cn } from "@/lib/utils"
import { Textarea } from "@/components/ui/textarea"
import type { Tag } from "@/types/tag"
import { hexToRgb } from "@/utils/hexToRgb"
import { fetchSearchTags } from "@/services/tag.service"
import { UploadImage } from "@/services/file.service"
import PostEditor from "@/components/Posts/PostEditor"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { DialogClose } from "@radix-ui/react-dialog"
import { toast } from "sonner"
import { UpdatePost, PublishDraftPost, GetPostById } from "@/services/post.service"
import type { UpdatePostCommand } from "@/types/commands/Posts/posts"
import { extractValidationMessages } from "@/utils/extractValidationMessages"
import type { ValidationErrorResponse } from "@/types/api"
import { useGlobalLoading } from "@/store/globalLoading.store"
import { useAuthStore } from "@/store/auth.store"
import { Badge } from "@/components/ui/badge"
import { Separator } from "@/components/ui/separator"

export const Route = createFileRoute("/edit/$postId")({
    component: RouteComponent,
})

const MAX_LINES = 6

function RouteComponent() {
    const { postId } = useParams({ strict: false });
    const textareaRef = useRef<HTMLTextAreaElement | null>(null)

    const [editMode, setEditMode] = useState<boolean>(true)
    const [tagInput, setTagInput] = useState("")
    const [selectedTags, setSelectedTags] = useState<Tag[] | []>([])
    const [suggestions, setSuggestions] = useState<Tag[] | []>([])
    const [isFocused, setIsFocused] = useState(false)
    const [isUploading, setIsUploading] = useState(false)
    const fileInputRef = useRef<HTMLInputElement>(null)
    const navigate = useNavigate()

    const [draftPost, setDraftPost] = useState({
        title: "",
        content: "",
        tagList: [] as string[],
        coverImageUrl: "",
    })

    const loadingStore = useGlobalLoading.getState()
    const { currentUser } = useAuthStore()

    useEffect(() => {
        if (!postId) return;

        const fetchPostData = async () => {
            try {
                loadingStore.show();
                const res = await GetPostById(postId);
                if (res.data.isSuccess && res.data.value) {
                    const post = res.data.value;

                    if (currentUser && currentUser.id !== post.postAuthor.userId) {
                        console.log("Current user:", currentUser, "Post author:", post.postAuthor);
                        toast.error("You don't have permission to edit this draft.");
                        navigate({ to: "/" });
                        return;
                    }


                    setDraftPost({
                        title: post.title,
                        content: post.content,
                        tagList: post.postTags.map(t => t.id),
                        coverImageUrl: post.coverImageUrl || "",
                    });
                    setSelectedTags(post.postTags);
                } else {
                    toast.error("Could not load post");
                    navigate({ to: "/dashboard" });
                }
            } catch (error: any) {
                toast.error("Error loading post");
                navigate({ to: "/dashboard" });
            } finally {
                loadingStore.hide();
            }
        };

        if (currentUser?.id) {
            fetchPostData();
        }
    }, [postId, currentUser?.id]);


    useEffect(() => {
        if (!isFocused && tagInput === "") {
            setSuggestions([]);
            return;
        }

        const fetchData = async () => {
            try {
                const res = await fetchSearchTags(tagInput)
                if (res.data) {
                    setSuggestions(res.data.value)
                }
            } catch (error: any) {
                toast.error(error.message)
            }
        }

        const timer = setTimeout(fetchData, 300)
        return () => clearTimeout(timer)
    }, [tagInput, isFocused])

    const handleInput = () => {
        const el = textareaRef.current
        if (!el) return

        el.style.height = "auto"
        const lineHeight = parseFloat(getComputedStyle(el).lineHeight)
        const maxHeight = lineHeight * MAX_LINES

        if (el.scrollHeight > maxHeight) {
            el.style.height = `${maxHeight}px`
            el.style.overflowY = "auto"
        } else {
            el.style.height = `${el.scrollHeight}px`
            el.style.overflowY = "hidden"
        }
    }

    useEffect(() => {
        handleInput()
    }, [draftPost?.title])

    function selectTag(tag: Tag) {
        setSelectedTags((prev) => {
            if (prev.some((t) => t.id === tag.id)) return prev
            const next = [...prev, tag]
            setDraftPost((draft) => ({
                ...draft,
                tagList: next.map((t) => t.id),
            }))
            return next
        })
        setTagInput("")
        setSuggestions([])
    }

    function removeTag(tagId: string) {
        const tags = selectedTags.filter((t) => t.id !== tagId)
        setSelectedTags(tags)
        setDraftPost((draft) => ({
            ...draft,
            tagList: tags.map((t) => t.id),
        }))
    }

    const handleUploadCoverImage = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (!file) return;

        if (!file.type.startsWith('image/')) {
            toast.error("Please upload a valid image file.");
            return;
        }

        setIsUploading(true);
        loadingStore.show();

        try {
            const response = await UploadImage(file);
            if (response.data && response.data.secure_url) {
                setDraftPost(prev => ({ ...prev, coverImageUrl: response.data.secure_url }));
                toast.success("Cover image uploaded successfully!");
            } else {
                throw new Error("Could not get image URL from server.");
            }
        } catch (error: any) {
            toast.error(error.message || "An error occurred during upload.");
        } finally {
            setIsUploading(false);
            loadingStore.hide();
            if (fileInputRef.current) {
                fileInputRef.current.value = "";
            }
        }
    };

    const saveDraft = async () => {
        if (!postId) return;
        try {
            const draftPostData: UpdatePostCommand = {
                id: postId as string,
                title: draftPost?.title || "",
                content: draftPost?.content || "",
                tagIds: selectedTags.map((tag) => tag.id),
                coverImageUrl: draftPost.coverImageUrl,
            }

            loadingStore.show();
            const res = await UpdatePost(postId as string, draftPostData)
            if (res.data.isSuccess) {
                toast.success("Draft updated successfully")
            } else {
                toast.error("Draft update failed")
            }
        } catch (error: any) {
            handleError(error);
        } finally {
            loadingStore.hide();
        }
    }

    const publishPost = async () => {
        if (!postId) return;
        try {
            const draftPostData: UpdatePostCommand = {
                id: postId as string,
                title: draftPost?.title || "",
                content: draftPost?.content || "",
                tagIds: selectedTags.map((tag) => tag.id),
                coverImageUrl: draftPost.coverImageUrl,
            }

            loadingStore.show();
            const updateRes = await UpdatePost(postId as string, draftPostData)
            if (updateRes.data.isFailure) {
                toast.error("Could not update post before publishing")
                return
            }

            const publishRes = await PublishDraftPost(postId as string)
            if (publishRes.data.isSuccess) {
                toast.success("Post published successfully!")
                navigate({ to: "/" })
            } else {
                toast.error("Post publishing failed")
            }
        } catch (error: any) {
            handleError(error);
        } finally {
            loadingStore.hide();
        }
    }

    const handleError = (error: any) => {
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
            toast.error(error.response?.data?.Detail || error.message || "Server error")
        }
    }

    return (
        <div className="min-h-screen bg-neutral-50/50 selection:bg-primary/20">
            {/* Premium Header */}
            <header className="sticky top-0 z-50 w-full border-b bg-white/80 backdrop-blur-xl transition-all">
                <div className="mx-auto flex h-16 max-w-7xl items-center justify-between px-4 sm:px-6 lg:px-8">
                    <div className="flex items-center gap-4">
                        <Link to="/dashboard" className="group flex items-center gap-1 text-sm font-medium text-neutral-500 hover:text-primary transition-colors">
                            <ChevronLeft className="h-4 w-4 transition-transform group-hover:-translate-x-0.5" />
                            Dashboard
                        </Link>
                        <Separator orientation="vertical" className="h-4" />
                        <div className="flex items-center gap-2">
                            <img src={Logo} className="h-7 w-7" alt="Logo" />
                            <span className="text-sm font-semibold text-neutral-900 hidden sm:inline-block">Edit Draft</span>
                        </div>
                    </div>

                    <div className="flex items-center gap-3">
                        <div className="hidden sm:flex items-center bg-neutral-100 p-1 rounded-full border border-neutral-200">
                            <button
                                onClick={() => setEditMode(true)}
                                className={cn(
                                    "flex items-center gap-1.5 px-4 py-1.5 text-xs font-semibold rounded-full transition-all",
                                    editMode ? "bg-white text-primary shadow-sm ring-1 ring-neutral-200" : "text-neutral-500 hover:text-neutral-900"
                                )}
                            >
                                <Edit3 className="h-3.5 w-3.5" />
                                Write
                            </button>
                            <button
                                onClick={() => setEditMode(false)}
                                className={cn(
                                    "flex items-center gap-1.5 px-4 py-1.5 text-xs font-semibold rounded-full transition-all",
                                    !editMode ? "bg-white text-primary shadow-sm ring-1 ring-neutral-200" : "text-neutral-500 hover:text-neutral-900"
                                )}
                            >
                                <Eye className="h-3.5 w-3.5" />
                                Preview
                            </button>
                        </div>

                        <Dialog>
                            <DialogTrigger asChild>
                                <Button variant="ghost" size="icon" className="h-9 w-9 rounded-full">
                                    <XIcon className="h-4 w-4" />
                                </Button>
                            </DialogTrigger>
                            <DialogContent className="sm:max-w-md">
                                <DialogHeader>
                                    <DialogTitle>Exit editor?</DialogTitle>
                                    <DialogDescription>
                                        You have unsaved changes. Are you sure you want to leave?
                                    </DialogDescription>
                                </DialogHeader>
                                <DialogFooter className="sm:justify-start gap-2">
                                    <Button type="button" variant="destructive" onClick={() => navigate({ to: "/dashboard" })}>
                                        Leave
                                    </Button>
                                    <DialogClose asChild>
                                        <Button type="button" variant="secondary">Keep editing</Button>
                                    </DialogClose>
                                </DialogFooter>
                            </DialogContent>
                        </Dialog>
                    </div>
                </div>
            </header>

            <main className="mx-auto max-w-5xl px-4 py-8 sm:px-6 lg:px-8">
                <div className="grid grid-cols-1 lg:grid-cols-12 gap-8">
                    {/* Main Editor Section */}
                    <div className="lg:col-span-12">
                        <div className="bg-white rounded-2xl border border-neutral-200 shadow-[0_8px_30px_rgb(0,0,0,0.04)] overflow-hidden">
                            {editMode ? (
                                <div className="flex flex-col">
                                    {/* Cover Section */}
                                    <div className="p-8 border-b border-neutral-100 flex flex-col gap-6">
                                        <div className="flex flex-wrap items-center gap-3">
                                            <input
                                                type="file"
                                                ref={fileInputRef}
                                                onChange={handleUploadCoverImage}
                                                accept="image/*"
                                                className="hidden"
                                            />
                                            <Button
                                                variant="border"
                                                onClick={() => fileInputRef.current?.click()}
                                                disabled={isUploading}
                                            >
                                                <ImageIcon className="h-4 w-4" />
                                                {isUploading ? "Uploading..." : "Upload Cover Image"}
                                            </Button>
                                            <Button variant="border">🍌 Generate Image</Button>
                                            <Button variant="border">Cover Video Link</Button>
                                        </div>

                                        {draftPost.coverImageUrl && (
                                            <div className="relative group rounded-2xl overflow-hidden border border-neutral-200 aspect-21/9 w-full max-w-3xl animate-in fade-in zoom-in duration-300">
                                                <img
                                                    src={draftPost.coverImageUrl}
                                                    alt="Cover"
                                                    className="h-full w-full object-cover transition-transform duration-500 group-hover:scale-105"
                                                />
                                                <div className="absolute inset-0 bg-black/20 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center backdrop-blur-[2px]">
                                                    <Button
                                                        variant="destructive"
                                                        size="sm"
                                                        onClick={() => setDraftPost(prev => ({ ...prev, coverImageUrl: "" }))}
                                                        className="h-9 gap-2 shadow-lg"
                                                    >
                                                        <XIcon className="h-4 w-4" /> remove cover image
                                                    </Button>
                                                </div>
                                            </div>
                                        )}
                                    </div>

                                    {/* Content Section */}
                                    <div className="p-8 space-y-6">
                                        <Textarea
                                            value={draftPost?.title}
                                            onChange={(e) => setDraftPost(prev => ({ ...prev, title: e.target.value }))}
                                            ref={textareaRef}
                                            rows={1}
                                            placeholder="Post title..."
                                            onInput={handleInput}
                                            className="p-0 w-full text-4xl sm:text-5xl font-extrabold resize-none border-0 focus:ring-0 outline-none placeholder:text-neutral-300 min-h-[60px] overflow-hidden leading-tight"
                                        />

                                        <div className="relative">
                                            <div className="flex flex-wrap gap-2 items-center min-h-[32px]">
                                                {selectedTags.map((tag) => (
                                                    <Badge
                                                        key={tag.id}
                                                        variant="secondary"
                                                        className="pl-3 pr-1 py-1.5 flex items-center gap-1.5 group font-medium transition-all hover:pr-2"
                                                        style={{
                                                            backgroundColor: `rgba(${hexToRgb(tag.color)}, 0.08)`,
                                                            color: tag.color,
                                                            borderColor: `rgba(${hexToRgb(tag.color)}, 0.2)`,
                                                        }}
                                                    >
                                                        <span>#</span>
                                                        {tag.name}
                                                        <button
                                                            onClick={() => removeTag(tag.id)}
                                                            className="ml-0.5 rounded-full p-0.5 hover:bg-black/5 transition-colors"
                                                        >
                                                            <XIcon className="h-3 w-3" />
                                                        </button>
                                                    </Badge>
                                                ))}

                                                <div className="relative flex-1 min-w-[150px]">
                                                    <input
                                                        onChange={(e) => setTagInput(e.target.value)}
                                                        value={tagInput}
                                                        onBlur={() => setTimeout(() => setIsFocused(false), 200)}
                                                        onFocus={() => setIsFocused(true)}
                                                        placeholder={selectedTags.length > 0 ? "Add another tag..." : "Add up to 4 tags for this post..."}
                                                        className="w-full border-none outline-none text-sm font-medium text-neutral-600 placeholder:text-neutral-400 focus:ring-0 bg-transparent py-1"
                                                    />

                                                    {isFocused && suggestions.length > 0 && (
                                                        <div className="absolute top-full left-0 mt-3 w-72 bg-white border border-neutral-200 rounded-2xl shadow-2xl overflow-hidden z-100 animate-in slide-in-from-top-2 duration-200">
                                                            <div className="bg-neutral-50 px-4 py-3 border-b border-neutral-100">
                                                                <span className="text-[11px] font-bold text-neutral-400 uppercase tracking-wider">Popular tags</span>
                                                            </div>
                                                            <div className="max-h-60 overflow-y-auto">
                                                                {suggestions.map((tag: Tag) => (
                                                                    <div
                                                                        key={tag.id}
                                                                        className="px-4 py-3 hover:bg-neutral-50 cursor-pointer flex items-center gap-3 transition-colors border-b border-neutral-50 last:border-0"
                                                                        onMouseDown={() => selectTag(tag)}
                                                                    >
                                                                        <div className="h-8 w-8 rounded-lg flex items-center justify-center text-sm font-bold bg-neutral-100" style={{ color: tag.color }}>#</div>
                                                                        <div className="flex flex-col">
                                                                            <span className="text-sm font-semibold text-neutral-900">{tag.name}</span>
                                                                            <span className="text-xs text-neutral-500 line-clamp-1">{tag.description}</span>
                                                                        </div>
                                                                    </div>
                                                                ))}
                                                            </div>
                                                        </div>
                                                    )}
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="px-2">
                                        <PostEditor
                                            value={draftPost.content}
                                            onChange={(newContent) => setDraftPost((prev) => ({ ...prev, content: newContent }))}
                                        />
                                    </div>
                                </div>
                            ) : (
                                <div className="p-8 min-h-[500px] prose prose-neutral max-w-none prose-img:rounded-2xl prose-headings:font-extrabold prose-a:text-primary">
                                    <h1 className="text-4xl sm:text-5xl font-extrabold mb-8">{draftPost.title || "Untitled"}</h1>
                                    {draftPost.coverImageUrl && (
                                        <img src={draftPost.coverImageUrl} className="w-full rounded-2xl mb-8" alt="Cover" />
                                    )}
                                    <div className="flex flex-wrap gap-2 mb-10">
                                        {selectedTags.map(t => (
                                            <span key={t.id} className="text-sm font-bold" style={{ color: t.color }}>#{t.name}</span>
                                        ))}
                                    </div>
                                    {/* Simplified Markdown Preview representation */}
                                    <div className="text-neutral-600 leading-relaxed whitespace-pre-wrap">
                                        {draftPost.content || "Post content will be displayed here..."}
                                    </div>
                                </div>
                            )}
                        </div>

                        {/* Sticky Action Bar */}
                        <div className="mt-8 flex flex-col sm:flex-row items-center justify-between gap-4 p-4 bg-white rounded-2xl border border-neutral-200 shadow-lg">
                            <div className="flex items-center gap-2 text-neutral-400">
                                <ClipboardClockIcon className="h-4 w-4" />
                                <span className="text-xs font-medium italic">Last saved: Just now</span>
                            </div>

                            <div className="flex items-center gap-3 w-full sm:w-auto">
                                <Button
                                    variant="secondary"
                                    onClick={saveDraft}
                                    className="flex-1 sm:flex-none h-11 px-6 rounded-xl font-semibold bg-neutral-100 text-neutral-600 hover:bg-neutral-200 border-0 transition-all"
                                >
                                    <Save className="h-4 w-4 mr-2" />
                                    Save Draft
                                </Button>
                                <Button
                                    onClick={publishPost}
                                    className="flex-1 sm:flex-none h-11 px-8 rounded-xl font-bold bg-primary text-white hover:opacity-90 shadow-[0_4px_14px_rgba(121,68,255,0.3)] border-0 transition-all active:scale-[0.98]"
                                >
                                    <Send className="h-4 w-4 mr-2" />
                                    Publish
                                </Button>
                            </div>
                        </div>
                    </div>
                </div>
            </main>

            {/* Background Decorative Elements */}
            <div className="fixed top-0 left-0 w-full h-full -z-10 pointer-events-none opacity-40">
                <div className="absolute top-[-10%] left-[-5%] w-[40%] h-[60%] bg-primary/5 blur-[120px] rounded-full" />
                <div className="absolute bottom-[-10%] right-[-5%] w-[40%] h-[60%] bg-purple-500/5 blur-[120px] rounded-full" />
            </div>
        </div>
    )
}
