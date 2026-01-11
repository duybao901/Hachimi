import { useEffect, useState } from "react"
import MDEditor from "@uiw/react-md-editor"
import { usePostStore } from "@/store/post.store"

function PostEditor() {
  const { setCurrentEditPostContent } = usePostStore.getState()

  const [value, setValue] = useState<string>("")

  useEffect(() => {
    const timer = setTimeout(() => {
      setCurrentEditPostContent(value)
    }, 500)

    return () => clearTimeout(timer)
  }, [value, setCurrentEditPostContent])

  return (
    <div data-color-mode="light" className="devto-editor">
      <MDEditor
        value={value}
        onChange={(v) => setValue(v ?? "")}
        preview="edit"
        visibleDragbar={false}
        height={500}
      />
    </div>
  )
}

export default PostEditor
