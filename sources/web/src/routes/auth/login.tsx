import { createFileRoute, Link, useNavigate } from "@tanstack/react-router"
import { Controller, useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"

import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Checkbox } from "@/components/ui/checkbox"
import { Label } from "@/components/ui/label"
import { toast } from "sonner"
import { useState } from "react"
import { Spinner } from "@/components/ui/spinner"
import { login } from "@/services/auth.service"
import { guestGuard } from "@/guards/guestGuard"

const formSchema = z.object({
  email: z.string(),
  password: z
    .string()
    .min(6, "Password must be at least 6 characters.")
    .max(20, "Password must be at most 20 characters."),
})

function Login() {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  })

  const navigate = useNavigate()

  const [isLoading, setIsLoading] = useState<boolean>(false)

  const onSubmit = async (data: z.infer<typeof formSchema>) => {
    try {
      setIsLoading(true)
      await login(data.email, data.password)
      toast.success("Login successfully!")
      setIsLoading(false)
      localStorage.setItem("isFirstLogin", "true")
      navigate({ to: '/' });
    } catch (error: any) {
      setIsLoading(false)
      if (error.response) {
        toast.error(
          error.response?.data?.Detail || error.message || "Server error"
        )
      }
    }
  }

  return (
    <div className="w-full flex items-center justify-center gap-10 p-10">
      <div className="w-[640px] p-12">
        <div className="mb-6 text-center">
          <h1 className="text-3xl font-bold mb-2">Join the DEV Community</h1>
          <p>DEV Community is a community of 3,620,351 amazing developers</p>
        </div>
        <form id="register-form" onSubmit={form.handleSubmit(onSubmit)}>
          <FieldGroup>
            <Controller
              name="email"
              control={form.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="email">Email</FieldLabel>
                  <Input
                    {...field}
                    id="email"
                    placeholder="alice@gmail.com"
                    autoComplete="on"
                    required
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />

            <Controller
              name="password"
              control={form.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="password">Password</FieldLabel>
                  <Input
                    {...field}
                    id="password"
                    type="password"
                    placeholder="••••••"
                  />
                  {fieldState.invalid && (
                    <FieldError errors={[fieldState.error]} />
                  )}
                </Field>
              )}
            />

            <div className="flex items-center justify-between">
              <div className="flex items-center gap-2">
                <Checkbox id="rememberme" />
                <Label className="font-normal" htmlFor="rememberme">
                  Remember me
                </Label>
              </div>
              <Link
                to="/auth/forgot"
                className="text-sm text-primary hover:underline hover:underline-offset-4 ml-2"
              >
                Forgot your password?
              </Link>
            </div>
            <Button type="submit" disabled={isLoading}>
              {isLoading ? <Spinner></Spinner> : "Log in"}
            </Button>
          </FieldGroup>
        </form>

        <p className="w-full italic mt-4 text-xs text-center text-muted-foreground px-24">
          By signing in, you are agreeing to our{" "}
          <Link to="/auth/login" className="text-primary">
            privacy policy, terms of use
          </Link>{" "}
          and{" "}
          <Link to="/auth/login" className="text-primary">
            code of conduct
          </Link>
          .
        </p>

        <hr className="w-full h-px bg-gray-200 mt-6 mb-6"></hr>

        <p className="text-sm text-center">
          New to DEV Community?{" "}
          <Link
            to="/auth/register"
            className="text-primary hover:underline hover:underline-offset-4"
          >
            Create an account
          </Link>
        </p>
      </div>
    </div>
  )
}

export const Route = createFileRoute("/auth/login")({
  beforeLoad: () => {
    console.log("before load...")
    guestGuard();
  },
  component: Login,
})

export default Login
