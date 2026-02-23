# Hachimi Project

Hachimi is a sophisticated web application built with a **.NET distributed microservices-style backend** and a **modern React frontend**. It follows **Clean Architecture**, **CQRS**, and **DDD** principles.

## 🏗️ Technical Architecture

### 🔙 Backend (.NET)
The backend is split into multiple projects to enforce CQRS and separation of concerns:

- **Command**: Handles write operations and business logic.
  - `Command.Domain`: Core entities and aggregates.
  - `Command.Application`: Command handlers and services.
  - `Command.Infrastructure`: External integrations.
- **Query**: Handles read operations and data retrieval.
  - Optimized for performance with separate read models.
- **Contract**: Shared definitions (Commands, Queries, Events, DTOs).
- **Authorization**: Identity and access management.
- **ProjectionWorker**: Background worker for async data projections.
- **ReverseProxy (ApiGateway)**: Central entry point for all API requests.

**Key Technologies:**
- **Language**: C# 12 / .NET 9
- **Messaging**: `MediatR` (internal), `MassTransit` (distributed events)
- **Patterns**: Aggregate Roots, Domain Events, Result Pattern, Versioned APIs.

---

### ⚛️ Frontend (React)
A modern SPA built for speed and developer experience.

- **Vite + TypeScript**: Fast build and type safety.
- **Routing**: `TanStack Router` (file-based routing).
- **Data Fetching**: `TanStack Query` (React Query).
- **State Management**: `zustand` (lightweight state).
- **Styling**: `Tailwind CSS v4` + `Radix UI` (primitive components).
- **Forms**: `React Hook Form` + `Zod`/`Yup` validation.

---

## 📂 Project Structure

```text
Hachimi/
├── docs/                # SQL scripts and documentation
├── sources/
│   ├── core/            # .NET Backend Solution
│   │   ├── src/         # Domain-specific projects (Command, Query, etc.)
│   │   └── tests/       # Unit and Integration tests
│   └── web/             # React Frontend Project
│       ├── src/
│       │   ├── components/ # UI components
│       │   ├── routes/     # TanStack Router routes
│       │   ├── services/   # API abstraction layer
│       │   └── store/      # Zustand state logic
```

## 🚀 Patterns Observed
1. **CQRS**: Complete separation of read and write paths.
2. **Result Pattern**: Uniform error handling across the backend.
3. **Event-Driven**: Use of Domain Events for side effects and projections.
4. **Rich Domain Model**: Use of `AggregateRoot` with domain logic encapsulation.
