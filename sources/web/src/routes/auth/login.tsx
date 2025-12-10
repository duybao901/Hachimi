import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/auth/login')({
    component: Login,
})

function Login() {
    return (
        <div>
            <h1>Login page</h1>
        </div>
    )
}
