import Header from '@/components/Layout/Header/Header'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/')({
    component: Home,
})

function Home() {
    return (
        <div>
            <Header></Header>
            <h2 className="font-bold text-xl mb-4">Latest Posts</h2>
            {/* Render list post */}
        </div>
    )
}
