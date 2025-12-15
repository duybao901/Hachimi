import { createFileRoute, Link, useNavigate } from '@tanstack/react-router'
import { Controller, useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import axios from 'axios'

import {
  Field,
  FieldError,
  FieldGroup,
  FieldLabel,
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { Button } from '@/components/ui/button'
import { toast } from 'sonner'
import { useState } from 'react'
import { Spinner } from '@/components/ui/spinner'
import { guestGuard } from '@/guards/guestGuard'

const formSchema = z.object({
  name: z
    .string()
    .max(32, "Name must be at most 32 characters."),
  email: z
    .string(),
  password: z
    .string()
    .min(6, "Password must be at least 6 characters.")
    .max(20, "Password must be at most 20 characters."),
  confirmPassword: z
    .string()
    .min(6, "Confirm Password must be at least 6 characters.")
})

function Register() {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: "",
      email: "",
      password: "",
      confirmPassword: ""
    },
  })

  const navigate = useNavigate();

  const [isLoading, setIsLoading] = useState<boolean>(false);

  const onSubmit = async (data: z.infer<typeof formSchema>) => {
    try {
      setIsLoading(true);
      await axios.post("http://localhost:5000/auth-api/v1/authen/register", data)
      toast.success("Register successfully! Please check your email to verify your account.");
      setIsLoading(false);
      navigate({ to: '/auth/login' });
    } catch (error: any) {
      setIsLoading(false);
      if (error.response) {
        toast.error(error.response?.data?.Detail || error.message || "Server error");
      }
    }
  }

  return (
    <div className='w-full flex items-center justify-center gap-10 p-10'>
      <div className='w-[640px] p-12'>
        <div className='mb-6 text-center'>
          <h1 className='text-3xl font-bold mb-2'>
            Join the DEV Community
          </h1>
          <p>
            DEV Community is a community of 3,620,351 amazing developers
          </p>
        </div>
        <form id="register-form" onSubmit={form.handleSubmit(onSubmit)}>
          <FieldGroup>
            <Controller
              name="name"
              control={form.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="name">
                    Name
                  </FieldLabel>
                  <Input
                    {...field}
                    id="name"
                    aria-invalid={fieldState.invalid}
                    placeholder="alice"
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
                  {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
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
                  {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                </Field>
              )}
            />

            <Controller
              name="confirmPassword"
              control={form.control}
              render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                  <FieldLabel htmlFor="confirmPassword">Confirm Password</FieldLabel>
                  <Input
                    {...field}
                    id="confirmPassword"
                    type="password"
                    placeholder="••••••"
                  />
                  {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                </Field>
              )}
            />
            <Button type='submit' disabled={isLoading}>
              {
                isLoading ? <Spinner></Spinner> : "Register"
              }
            </Button>

            <p className='w-full italic mt-4 text-xs text-center text-muted-foreground px-24'>
              By signing in, you are agreeing to our <Link to="/auth/login" className='text-primary'>privacy policy, terms of use</Link> and <Link to="/auth/login" className='text-primary'>code of conduct</Link>.
            </p>

            <hr className='w-full h-px bg-gray-200 mt-6 mb-6'></hr>

            <p className='text-sm text-center'>
              Already have an account? <Link to="/auth/login" className="text-primary hover:underline hover:underline-offset-4">Log in.</Link>
            </p>
          </FieldGroup>
        </form>
      </div>
    </div>
  )
}

export const Route = createFileRoute('/auth/register')({
  beforeLoad: () => {
    guestGuard();
  },
  component: Register,
})

export default Register