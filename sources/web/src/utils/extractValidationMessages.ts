export function extractValidationMessages(
  errors: Record<string, string[]>
): string[] {
  return Object.entries(errors).flatMap(([field, messages]) =>
    messages.map((msg) => `${field}: ${msg}`)
  )
}