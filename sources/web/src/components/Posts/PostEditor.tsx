import { useEffect, useState } from "react";
import MDEditor from "@uiw/react-md-editor";

function PostEditor() {
  const [value, setValue] = useState<any>("**Hello world!!!**");

  useEffect(() => {
    const timer = setTimeout(() => {
      // set to stored value after 2 seconds
    }, 2000);

    return () => clearTimeout(timer);
  }, [value])

  return (
    <div
      data-color-mode="light"
      className="devto-editor"
    >
      <MDEditor
        value={value}
        onChange={setValue}
        preview="edit"        
        visibleDragbar={false}      
      />
    </div>
  );
}

export default PostEditor;