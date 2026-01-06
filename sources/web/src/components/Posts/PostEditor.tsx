import { useState } from "react";
import MDEditor from "@uiw/react-md-editor";

function PostEditor() {
    const [value, setValue] = useState<any>("**Hello world!!!**");

    return <div data-color-mode="light">
      <MDEditor
        value={value}
        onChange={setValue}
        preview="edit"      // edit + preview song song
        height={500}
      />
    </div>
}

export default PostEditor;