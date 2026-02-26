import { useState } from "react"
import { cn } from "@/lib/utils"
import { Button } from "../ui/button"
import type { Reaction } from "@/types/queries/Reactions/reaction"
import { useAuthStore } from "@/store/auth.store"

interface ReactionSelectorProps {
    postId: string
    reactions: Reaction[]
    side?: "top" | "right" | "bottom" | "left"
    showCount?: boolean
    onReact?: (postId: string, userId: string, reactionTypeId: string) => void
    showHoverEffect?: boolean
}

export function ReactionSelector({
    postId,
    reactions = [],
    side = "top",
    showCount = true,
    onReact,
    showHoverEffect = true
}: ReactionSelectorProps) {
    const [isHovered, setIsHovered] = useState(false)
    const { currentUser } = useAuthStore.getState();

    const totalReactionCount = reactions.reduce((acc, curr) => acc + curr.count, 0)
    const hasAnyReaction = reactions.some(r => r.isReactionByCurrentUser)

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
            onMouseEnter={() => showHoverEffect && setIsHovered(true)}
            onMouseLeave={() => showHoverEffect && setIsHovered(false)}
        >
            {/* Reaction Summary (Visible) */}
            <Button
                variant="ghost"
                className={cn(
                    "flex items-center px-2 py-1 h-auto rounded-sm hover:bg-gray-100",
                    !showCount && "flex-col gap-1 p-2 h-12 w-12",
                    hasAnyReaction && "bg-gray-50 text-primary" // Highlight if user has reacted
                )}
            >
                <div className="flex">
                    {reactions.length > 0 && reactions.some(r => r.count > 0 || r.isReactionByCurrentUser) ? (
                        reactions
                            .filter(r => r.count > 0 || r.isReactionByCurrentUser)
                            .slice(0, 3)
                            .map((r, i) => (
                                <div
                                    key={r.id}
                                    className={cn(
                                        "rounded-full border-2 border-white bg-gray-100 p-1 flex items-center justify-center transition-transform",
                                        i > 0 && "-ml-2.5",
                                        !showCount && i > 0 && "hidden"
                                    )}
                                >
                                    <img src={r.url} width={!showCount ? "24" : "18"} height={!showCount ? "24" : "18"} alt={r.name} />
                                </div>
                            ))
                    ) : (
                        <div className="w-10 h-10 rounded-full border-2 border-white bg-gray-100 p-1 flex items-center justify-center">
                            <img src={reactions[0]?.url || "https://assets.dev.to/assets/fire-f60e7a582391810302117f987b22a8ef04a2fe0df7e3258a5f49332df1cec71e.svg"} width={!showCount ? "24" : "18"} height={!showCount ? "24" : "18"} alt="Default" />
                        </div>
                    )}
                </div>
                {showCount && (
                    <div className="ml-2 text-[13px] text-inherit">
                        {totalReactionCount} <span className="hidden sm:inline">Reactions</span>
                    </div>
                )}
            </Button>

            {/* Reaction Popover (Hidden until hover) */}
            <div
                className={cn(
                    "absolute z-50 transition-all duration-200 h-20",
                    popoverVariants[side],
                    popoverAnimate[side]
                )}
            >
                <div className="p-2 bg-white border border-gray-200 rounded-lg shadow-xl flex gap-2 h-full">
                    {reactions.length > 0 ? (
                        reactions.map((reaction) => (
                            <button
                                key={reaction.id}
                                className={cn(
                                    "p-2 hover:bg-gray-100 rounded-md transition-all hover:scale-110 flex flex-col items-center gap-1 group w-10 h-10 cursor-pointer",
                                    reaction.isReactionByCurrentUser && "bg-primary/10" // Highlight specific reaction
                                )}
                                title={reaction.name}
                                onClick={() => onReact?.(postId, currentUser?.id || "", reaction.id)}
                            >
                                <img src={reaction.url} width="24" height="24" alt={reaction.name} className="transition-transform group-hover:scale-125" />
                                <span className="text text-gray-500 mt-1">{reaction.count}</span>
                            </button>
                        ))
                    ) : (
                        <div className="px-3 py-1 text-xs text-gray-500">No reactions available</div>
                    )}
                </div>
            </div>
        </div>
    )
}
