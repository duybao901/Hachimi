import { useEffect, useRef, useState } from "react"
import { Textarea } from "@/components/ui/textarea"
import { Button } from "@/components/ui/button"
import { Link } from "@tanstack/react-router"

function QuickPostBox() {
  const containerRef = useRef<HTMLDivElement>(null)
  const textareaRef = useRef<HTMLTextAreaElement>(null)
  const [expanded, setExpanded] = useState(false)

  // CLICK OUTSIDE
  useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (
        containerRef.current &&
        !containerRef.current.contains(e.target as Node)
      ) {

        setExpanded(false)

        if (textareaRef.current) {
          textareaRef.current.style.height = "40px"
        }
      }
    }

    document.addEventListener("mousedown", handleClickOutside)
    return () => document.removeEventListener("mousedown", handleClickOutside)
  }, [])


  useEffect(() => {
    if (textareaRef.current) {
      if (expanded) {
        textareaRef.current.style.height = "40px"
        requestAnimationFrame(() => {
          if (textareaRef.current) {
            textareaRef.current.style.height = "120px"
          }
        })
      }
    }
  }, [expanded])

  return (
    <div
      ref={containerRef}
      className="p-2 bg-white rounded-sm border border-gray-200"
      onClick={() => setExpanded(true)}
    >
      <Textarea        
        ref={textareaRef}
        className="h-10 resize-none overflow-hidden -mb-1.5 placeholder:text-base font-light"
        placeholder="What's on your mind?"
      />

      {expanded && (
        <div className="w-full flex items-center justify-between mt-4">
          <p className="text-xs text-(--link-color-secondary)">
            <span className="font-bold">Quickie Posts</span> show up in the feed
            but not notifications or your profile –{" "}
            <Link to="/new" className="text-primary">
              Open Full Editor
            </Link>
          </p>

          <div className="flex items-center gap-1">
            <span className="text-sm text-(--link-color-secondary)">
              0/256
            </span>
            <Button className="h-[34px]">Post</Button>
          </div>
        </div>
      )}
    </div>
  )
}

export default QuickPostBox;
