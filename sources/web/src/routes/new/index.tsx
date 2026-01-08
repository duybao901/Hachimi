import { createFileRoute, Link } from "@tanstack/react-router"
import Logo from "@/assets/horse_logo.png"
import { X as XIcon, ClipboardClock as ClipboardClockIcon } from "lucide-react"
import { Button } from "@/components/ui/button"
import { useEffect, useRef, useState } from "react"
import { cn } from "@/lib/utils"
import { Textarea } from "@/components/ui/textarea"
import type { Tag } from "@/types/tag"
import { hexToRgb } from "@/utils/hexToRgb"
import { fetchSearchTags } from "@/services/tag.service"
import PostEditor from "@/components/Posts/PostEditor"

export const Route = createFileRoute("/new/")({
  component: RouteComponent,
})

const MAX_LINES = 4

function RouteComponent() {
  const [editMode, setEditMode] = useState<boolean>(true)
  const textareaRef = useRef<HTMLTextAreaElement | null>(null)
  const [input, setInput] = useState("")
  const [selectedTags, setSelectedTags] = useState<Tag[]>([
    {
      id: "1",
      name: "javascript",
      description: "All about JavaScript",
      color: "#f7df1e",
    },
  ])
  const [suggestions, setSuggestions] = useState<any>([])
  const [isFocused, setIsFocused] = useState(false)

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
    if (!isFocused && input === "") {
      return
    }

    const fetchData = async () => {
      try {
        const res = await fetchSearchTags(input)
        setSuggestions(res.data.value)
      } catch (error) {
        console.error("Error fetching tag suggestions:", error)
      }
    }

    const timer = setTimeout(fetchData, 300)

    return () => clearTimeout(timer)
  }, [input, isFocused])

  function selectTag(tag: Tag) {
    if (selectedTags.some((t) => t.id === tag.id)) return

    setSelectedTags([...selectedTags, tag])
    setInput("")
    setSuggestions([])
  }

  function removeTag(tagId: string) {
    setSelectedTags(selectedTags.filter((t) => t.id !== tagId))
  }

  const onBlur = () => {
    setIsFocused(false)
    setSuggestions([])
  }

  const onChange = (value: string) => {
    setInput(value)
    if (value === "") setSuggestions([])
  }

  return (
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
            <Button variant="ghost" className="hover:text-primary">
              <XIcon className="w-5 h-5" />
            </Button>
          </div>

          {/* Main content */}
          <div
            className="col-span-8 bg-white rounded-md border border-gray-100
                          h-[calc(100vh-var(--header-height)-var(--article-form-actions-height))]"
          >
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
                      value={input}
                      onChange={() => onChange((event.target as HTMLInputElement).value)}
                      onBlur={() => onBlur()}
                      placeholder={
                        selectedTags.length > 0
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
                  <PostEditor></PostEditor>
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
  )
}
