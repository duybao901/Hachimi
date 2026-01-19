import { createFileRoute, Link } from "@tanstack/react-router"
import Logo from "@/assets/horse_logo.png"
import { X as XIcon, ClipboardClock as ClipboardClockIcon } from "lucide-react"
import { Button } from "@/components/ui/button"
import { useEffect, useRef, useState } from "react"
import { cn } from "@/lib/utils"
import { Textarea } from "@/components/ui/textarea"
import type { Tag } from "@/types/tag"
import { hexToRgb } from "@/utils/hexToRgb"
import { fetchSearchTags, getTagListFromIds } from "@/services/tag.service"
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
import { SaveDraftPost } from "@/services/post.service"
import type { DraftPost, SaveDraftPostCommand } from "@/types/commands/Posts/posts"
import { extractValidationMessages } from "@/utils/extractValidationMessages"
import type { ValidationErrorResponse } from "@/types/api"

export const Route = createFileRoute("/new/")({
  component: RouteComponent,
})

const MAX_LINES = 4
const DRAFT_POST_KEY = "editor-v2-hachimi/new"

function RouteComponent() {
  const textareaRef = useRef<HTMLTextAreaElement | null>(null)

  const [editMode, setEditMode] = useState<boolean>(true)
  const [tagInput, setTagInput] = useState("")
  const [selectedTags, setSelectedTags] = useState<Tag[] | []>([])
  const [suggestions, setSuggestions] = useState<Tag[] | []>([])
  const [isFocused, setIsFocused] = useState(false)
  const [isLoadingTag, setIsLoadingTag] = useState<boolean>(false)

  const initialDraft = (() => {
    const raw = localStorage.getItem(DRAFT_POST_KEY)
    return raw
      ? JSON.parse(raw)
      : {
        title: "",
        content: "",
        tagList: [],
        coverImageUrl: "",
      }
  })()

  const [draftPost, setDraftPost] = useState<DraftPost>(initialDraft)

  useEffect(() => {
    const timer = setTimeout(() => {
      localStorage.setItem(
        DRAFT_POST_KEY,
        JSON.stringify({
          ...draftPost,
          updatedAt: Date.now()
        })
      )
    }, 400)

    return () => clearTimeout(timer)
  }, [draftPost])

  useEffect(() => {
    if (!isFocused && tagInput === "") {
      return
    }

    const fetchData = async () => {
      try {
        setIsLoadingTag(true)
        const res = await fetchSearchTags(tagInput)
        if (res.data) {
          setSuggestions(res.data.value)
        }
      } catch (error: any) {
        toast.error(error.message)
      } finally {
        setIsLoadingTag(false)
      }
    }

    const timer = setTimeout(fetchData, 300)

    return () => clearTimeout(timer)
  }, [tagInput, isFocused])

  useEffect(() => {
    const fetchTagsListFromIdsAsync = async () => {
      try {
        const res = await getTagListFromIds(draftPost.tagList)
        if (res.data.value) {
          setSelectedTags(res.data.value)
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

    fetchTagsListFromIdsAsync();
  }, [draftPost.tagList])

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

  function removeTag(tagId: string) {
    const tags = selectedTags.filter((t) => t.id !== tagId)
    setSelectedTags(tags)
    setDraftPost((draft) => ({
      ...draft,
      tagList: tags.map((t) => t.id),
    }))
  }

  const onBlur = () => {
    setIsFocused(false)
    setSuggestions([])
  }

  const saveDraftPost = async () => {
    try {
      const draftPostData: SaveDraftPostCommand = {
        title: draftPost?.title || "",
        content: draftPost?.content || "",
        tagIds: selectedTags.map((tag) => tag.id),
        coverImageUrl: draftPost.coverImageUrl,
      }

      const res = await SaveDraftPost(draftPostData)
      toast.success(res.data.value)
      setDraftPost({
        title: "",
        content: "",
        tagList: [],
        coverImageUrl: "",
      })

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

  const postTitleOnChange = (e: any) => {
    setDraftPost((prev) => ({
      ...prev,
      title: e.target.value
    }))
  }

  return (
    <>
      <div className="bg-gray-50 min-h-screen">
        <div className="max-w-6xl mx-auto">
          <div className="w-full grid grid-cols-12">
            {/* Header left */}
            <div className="col-span-8 flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Link to="/">
                  <img src={Logo} className="w-8 h-8" />
                </Link>
                <h3 className="text-md font-semibold">Create Post</h3>
              </div>

              <div className="flex items-center gap-2">
                <Button
                  variant="ghost"
                  onClick={() => setEditMode(true)}
                  className={cn(
                    "text-[15px] text-(--link-color) font-normal hover:text-primary hover:bg-primary/10",
                    editMode && "font-semibold"
                  )}
                >
                  Edit
                </Button>

                <Button
                  variant="ghost"
                  onClick={() => setEditMode(false)}
                  className={cn(
                    "text-[15px] text-(--link-color) font-normal hover:text-primary hover:bg-primary/10",
                    !editMode && "font-semibold"
                  )}
                >
                  Preview
                </Button>
              </div>
            </div>

            {/* Header right */}

            <div className="col-span-4 flex items-center justify-end py-2">
              <Dialog>
                <DialogTrigger>
                  <Button variant="ghost" className="hover:text-primary">
                    <XIcon className="w-5 h-5" />
                  </Button>
                </DialogTrigger>
                <DialogContent>
                  <DialogHeader>
                    <DialogTitle>You have unsaved changes</DialogTitle>
                    <DialogDescription>
                      You've made changes to your post. Do you want to navigate
                      to leave this page?
                    </DialogDescription>
                  </DialogHeader>
                  <DialogFooter>
                    <Button type="submit">
                      Yes, Leave the page
                    </Button>
                    <DialogClose asChild>
                      <Button variant="secondary">No, Keep editing</Button>
                    </DialogClose>
                  </DialogFooter>
                </DialogContent>
              </Dialog>
            </div>

            {/* Main content */}
            <div className="col-span-8 bg-white rounded-md border border-gray-100">
              {editMode ? (
                <div className="h-[calc(100vh-var(--header-height) - var(--article-form-actions-height))] flex flex-col overflow-y-auto">
                  {/* Post Actions */}
                  <div className="flex items-center gap-2 p-8">
                    <Button variant="border">Upload Cover Image</Button>
                    <Button variant="border">🍌 Generate Image</Button>
                    <Button variant="border">Cover Video Link</Button>
                  </div>

                  {/* Post Title */}
                  <div className="w-full h-auto mt-4 p-0 px-8">
                    <Textarea
                      value={draftPost?.title}
                      onChange={postTitleOnChange}
                      ref={textareaRef}
                      rows={1}
                      placeholder="New post title here..."
                      onInput={handleInput}
                      className="p-0 w-full text-5xl font-extrabold resize-none border-0
                              focus:ring-0 outline-none
                              overflow-hidden
                              placeholder:text-(--link-color-secondary) max-h-60"
                    />
                  </div>

                  <div className="mt-4 px-8">
                    <div className="flex gap-2">
                      {selectedTags.map((tag) => (
                        <div
                          className="bg-gray-100 rounded px-3 py-1 text-sm flex items-center"
                          style={{
                            backgroundColor: `rgba(${hexToRgb(tag.color)}, 0.1)`,
                          }}
                        >
                          # {tag.name}
                          <XIcon
                            onClick={() => removeTag(tag.id)}
                            className="ml-1 w-5 h-5 hover:text-red-500 cursor-pointer"
                          />
                        </div>
                      ))}

                      <input
                        onChange={(e) => setTagInput(e.target.value)}
                        value={tagInput}
                        onBlur={() => onBlur()}
                        placeholder={
                          isLoadingTag
                            ? "Searching..."
                            : selectedTags && selectedTags.length > 0
                              ? "Add another..."
                              : "Add up to 4 tags..."
                        }
                        className="border-none outline-none font-light text-(--base-90) placeholder:text-(--link-color-secondary) focus:ring-0 bg-transparent"
                        onFocus={() => setIsFocused(true)}
                      />
                    </div>

                    {suggestions.length > 0 && (
                      <div className="h-46">
                        <div className="bg-white border h-full border-gray-200 rounded-md mt-1 overflow-y-auto shadow-md z-10 ">
                          <h2 className="font-semibold p-4">Top tags</h2>
                          <div className="w-full bg-gray-100 h-px"></div>
                          {suggestions.map((tag: Tag) => (
                            <div
                              key={tag.id}
                              className="px-4 py-4 hover:bg-gray-100 cursor-pointer group"
                              onMouseDown={() => selectTag(tag)}
                            >
                              <div className="group-hover:text-primary font-medium text-[15px]">
                                <span style={{ color: tag.color }}>#</span>
                                {tag.name}
                              </div>
                              <div className="text-sm font-light text-(--link-color-secondary)">
                                {tag.description}
                              </div>
                            </div>
                          ))}
                        </div>
                      </div>
                    )}
                  </div>

                  {/* Post Body */}
                  <div className="w-full mt-4">
                    <PostEditor value={draftPost.content}
                      onChange={(newContent) =>
                        setDraftPost((prev) => ({
                          ...prev,
                          content: newContent
                        }))
                      }></PostEditor>
                  </div>
                </div>
              ) : (
                <div className="w-full">
                  Preview mode
                  {/* <RenderedPostPreview /> */}
                </div>
              )}
            </div>

            <div className="col-span-4">Writing a Great Post Title</div>

            <div className="mt-2 col-span-4 h-(--article-form-actions-height)">
              <div className="flex items-center gap-4">
                <Button>Publish</Button>
                <Button
                  variant="secondary"
                  className="font-normal bg-transparent"
                  onClick={saveDraftPost}
                >
                  Save Draft
                </Button>
                <Button
                  variant="secondary"
                  className="font-normal bg-transparent"
                >
                  <ClipboardClockIcon />
                </Button>
                <Button
                  variant="secondary"
                  className="font-normal bg-transparent"
                >
                  Revert new changes
                </Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  )
}
