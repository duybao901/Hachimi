import React, { FC } from "react"
import ReactMarkdown, { MarkdownAsync } from "react-markdown"
import remarkGfm from "remark-gfm"
import rehypePrettyCode from "rehype-pretty-code"
import rehypeSanitize, { defaultSchema } from "rehype-sanitize"

interface MarkdownRendererProps {
  content: string
}

/* ---------------------------------- */
/* Pretty Code Configuration (Shiki)  */
/* ---------------------------------- */
const prettyCodeOptions = {
  theme: "github-dark",
  keepBackground: false
}

/* ---------------------------------- */
/* Sanitize Schema Extension          */
/* (Allow Shiki inline styles/classes)*/
/* ---------------------------------- */
const sanitizeSchema = {
  ...defaultSchema,
  attributes: {
    ...defaultSchema.attributes,
    code: [
      ...(defaultSchema.attributes?.code || []),
      ["className"]
    ],
    span: [
      ...(defaultSchema.attributes?.span || []),
      ["className"],
      ["style"]
    ],
    div: [
      ...(defaultSchema.attributes?.div || []),
      ["className"]
    ],
    pre: [
      ...(defaultSchema.attributes?.pre || []),
      ["className"]
    ]
  }
}

/* ---------------------------------- */
/* Component Renderers                */
/* ---------------------------------- */
const markdownComponents = {
  h1: ({ children }: any) => (
    <h1 className="text-4xl font-extrabold mt-10 mb-6">{children}</h1>
  ),
  h2: ({ children }: any) => (
    <h2 className="text-3xl font-bold mt-8 mb-4">{children}</h2>
  ),
  h3: ({ children }: any) => (
    <h3 className="text-2xl font-semibold mt-6 mb-3">{children}</h3>
  ),

  p: ({ children }: any) => (
    <p className="leading-7 text-gray-800 dark:text-gray-300">
      {children}
    </p>
  ),

  a: ({ href, children }: any) => (
    <a
      href={href}
      target="_blank"
      rel="noopener noreferrer"
      className="text-blue-600 hover:underline"
    >
      {children}
    </a>
  ),

  blockquote: ({ children }: any) => (
    <blockquote className="border-l-4 border-gray-300 pl-4 italic text-gray-600 my-6">
      {children}
    </blockquote>
  ),

  /* Inline code only.
     Block code handled by Shiki automatically */
  code({ inline, children }: any) {
    if (inline) {
      return (
        <code className="bg-gray-100 dark:bg-gray-800 px-1 py-0.5 rounded text-sm">
          {children}
        </code>
      )
    }
    return <code>{children}</code>
  },

  table: ({ children }: any) => (
    <div className="overflow-x-auto my-6">
      <table className="w-full border-collapse text-sm">
        {children}
      </table>
    </div>
  ),

  thead: ({ children }: any) => (
    <thead className="bg-gray-100 dark:bg-gray-800">
      {children}
    </thead>
  ),

  th: ({ children }: any) => (
    <th className="border border-gray-300 dark:border-gray-700 px-4 py-2 text-left font-semibold">
      {children}
    </th>
  ),

  td: ({ children }: any) => (
    <td className="border border-gray-200 dark:border-gray-700 px-4 py-2">
      {children}
    </td>
  ),

  tr: ({ children }: any) => (
    <tr className="even:bg-gray-50 dark:even:bg-gray-900">
      {children}
    </tr>
  ),

  img: ({ src, alt }: any) => (
    <img
      src={src ?? ""}
      alt={alt ?? ""}
      className="rounded-lg my-6"
    />
  ),

  ul: ({ children }: any) => (
    <ul className="list-disc pl-6 space-y-2 my-4">{children}</ul>
  ),

  ol: ({ children }: any) => (
    <ol className="list-decimal pl-6 space-y-2 my-4">{children}</ol>
  ),

  li: ({ children }: any) => (
    <li className="leading-6">{children}</li>
  )
}

/* ---------------------------------- */
/* Markdown Renderer                  */
/* ---------------------------------- */
export const MarkdownRenderer: FC<MarkdownRendererProps> = ({ content }) => {
  return (
    <div className="prose prose-lg max-w-none dark:prose-invert">
      <ReactMarkdown 
        remarkPlugins={[remarkGfm]}
        rehypePlugins={[
          [rehypePrettyCode, prettyCodeOptions],
          [rehypeSanitize, sanitizeSchema]
        ]}
        components={markdownComponents}
      >
        {content}
      </ReactMarkdown>
    </div>
  )
}
