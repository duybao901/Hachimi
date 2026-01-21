import MDEditor from "@uiw/react-md-editor"
import rehypeSanitize from "rehype-sanitize";

type PostEditorProps = {
  value: string
  onChange: (value: string) => void
}

function PostEditor({ value, onChange }: PostEditorProps) {
  return (
    <div data-color-mode="light" className="devto-editor">
      <MDEditor
        value={value}
        onChange={(v) => onChange(v ?? "")}
        preview="edit"
        visibleDragbar={false}
        height={500}
        previewOptions={{
          rehypePlugins: [[rehypeSanitize]],
        }}
      />
    </div>
  )
}

export default PostEditor
