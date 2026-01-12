import { useState } from "react"
import MDEditor from "@uiw/react-md-editor"

function PostEditor() {

  const [value, setValue] = useState<string>("")

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
