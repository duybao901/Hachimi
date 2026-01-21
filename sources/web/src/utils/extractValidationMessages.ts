import type { ValidationErrorItem } from "@/types/api";

export function extractValidationMessages(
  errors: Record<string, ValidationErrorItem>
): string[] {
  return Object.values(errors).map((e) => e.message)
}
