import { useEffect, useState } from "react"
import MDEditor from "@uiw/react-md-editor"
import { usePostStore } from "@/store/post.store"

function PostEditor() {
  const currentEditPost = usePostStore((s) => s.currentEditPost)
  const setContent = usePostStore((s) => s.setCurrentEditPostContent)

  const [value, setValue] = useState<string>("")

  // sync store → editor (khi load post / back page)
  // useEffect(() => {
  //   setValue(currentEditPost?.content ?? "")
  // }, [currentEditPost])

  // debounce editor → store
  useEffect(() => {
    const timer = setTimeout(() => {
      setContent(value)
    }, 500) 

    return () => clearTimeout(timer)
  }, [value, setContent])

  return (
    <div data-color-mode="light" className="devto-editor">
      <MDEditor
        value={value}
        onChange={(v) => setValue(v ?? "")}
        preview="edit"
        visibleDragbar={false}
        height={400}
      />
    </div>
  )
}

export default PostEditor
