import MDEditor from "@uiw/react-md-editor"

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
      />
    </div>
  )
}

export default PostEditor
