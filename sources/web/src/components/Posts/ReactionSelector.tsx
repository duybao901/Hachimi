import { useState } from "react"
import { useQuery } from "@tanstack/react-query"
import { GetPublicReactions } from "@/services/reaction.service"
import { cn } from "@/lib/utils"
import { Button } from "../ui/button"

interface ReactionSelectorProps {
    postId: string
    initialReactionCount?: number
    initialReactions?: { icon: string }[]
    side?: "top" | "right" | "bottom" | "left"
    showCount?: boolean
}

export function ReactionSelector({
    initialReactionCount = 0,
    initialReactions = [],
    side = "top",
    showCount = true
}: ReactionSelectorProps) {
    const [isHovered, setIsHovered] = useState(false)

    const { data: reactionsResponse } = useQuery({
        queryKey: ["public-reactions"],
        queryFn: () => GetPublicReactions(),
        staleTime: 1000 * 60 * 60, // 1 hour
    })

    const availableReactions = reactionsResponse?.data?.value || []

    const popoverVariants = {
        top: "bottom-full left-0 pb-2 origin-bottom-left",
        right: "left-full top-0 pl-2 origin-left",
        bottom: "top-full left-0 pt-2 origin-top-left",
        left: "right-full top-0 pr-2 origin-right"
    }

    const popoverAnimate = {
        top: isHovered ? "opacity-100 scale-100 translate-y-0" : "opacity-0 scale-95 translate-y-2 pointer-events-none",
        right: isHovered ? "opacity-100 scale-100 translate-x-0" : "opacity-0 scale-95 -translate-x-2 pointer-events-none",
        bottom: isHovered ? "opacity-100 scale-100 translate-y-0" : "opacity-0 scale-95 -translate-y-2 pointer-events-none",
        left: isHovered ? "opacity-100 scale-100 translate-x-0" : "opacity-0 scale-95 translate-x-2 pointer-events-none"
    }

    return (
        <div
            className="relative flex items-center"
            onMouseEnter={() => setIsHovered(true)}
            onMouseLeave={() => setIsHovered(false)}
        >
            {/* Reaction Summary (Visible) */}
            <Button
                variant="ghost"
                className={cn(
                    "flex items-center px-2 py-1 h-auto rounded-sm hover:bg-gray-100",
                    !showCount && "flex-col gap-1 p-2 h-12 w-12"
                )}
            >
                <div className="flex">
                    {initialReactions.length > 0 ? (
                        initialReactions.map((r, i) => (
                            <div
                                key={i}
                                className={cn(
                                    "rounded-full border-2 border-white bg-gray-100 p-1 flex items-center justify-center",
                                    i > 0 && "-ml-2.5",
                                    !showCount && i > 0 && "hidden" // Only show first icon in tiny sidebar if count is hidden
                                )}
                            >
                                <img src={r.icon} width={!showCount ? "24" : "18"} height={!showCount ? "24" : "18"} alt="Reaction" />
                            </div>
                        ))
                    ) : (
                        <div className="rounded-full border-2 border-white bg-gray-100 p-1 flex items-center justify-center">
                            <img src="https://assets.dev.to/assets/fire-f60e7a582391810302117f987b22a8ef04a2fe0df7e3258a5f49332df1cec71e.svg" width={!showCount ? "24" : "18"} height={!showCount ? "24" : "18"} alt="Fire" />
                        </div>
                    )}
                </div>
                {showCount && (
                    <div className="ml-2 text-[13px] text-(--link-color)">
                        {initialReactionCount} <span className="hidden sm:inline">Reactions</span>
                    </div>
                )}
            </Button>

            {/* Reaction Popover (Hidden until hover) */}
            <div
                className={cn(
                    "absolute z-50 transition-all duration-200",
                    popoverVariants[side],
                    popoverAnimate[side]
                )}
            >
                <div className="p-1.5 bg-white border border-gray-200 rounded-lg shadow-xl flex gap-1">
                    {availableReactions.length > 0 ? (
                        availableReactions.map((reaction) => (
                            <button
                                key={reaction.id}
                                className="p-2 hover:bg-gray-100 rounded-md transition-transform hover:scale-125"
                                title={reaction.name}
                                onClick={() => {
                                    console.log(`Reacted with ${reaction.name} to post`)
                                    // Logic to send reaction would go here (Command side)
                                }}
                            >
                                {/* {"query-api"/} */}
                                <img src={reaction.url} width="24" height="24" alt={reaction.name} />
                                <p>{reaction.count}</p>
                            </button>
                        ))
                    ) : (
                        <div className="px-3 py-1 text-xs text-gray-500">Loading...</div>
                    )}
                </div>
            </div>
        </div>
    )
}
