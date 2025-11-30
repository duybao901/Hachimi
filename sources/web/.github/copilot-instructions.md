<!-- .github/copilot-instructions.md - guidance for AI coding agents -->
# Copilot instructions for this repository

Purpose: give AI coding agents the concrete, repo-specific knowledge they need to be productive here. This repository currently contains no source files at the checked path (`d:/Document/Project/Hachimi/sources/web`). If files are added, follow the mapping and checks below.

1. First steps (when repo is non-empty)
- **Locate entrypoints**: look for `package.json`, `pyproject.toml`, `Cargo.toml`, `go.mod`, `pom.xml` at repository root or in `src/`, `apps/`, `services/`.
- **Identify the app type**: if `package.json` exists and lists React/Vue/Next/Angular deps, treat this as a frontend app; look for `next.config.js`, `vite.config.*`, `nuxt.config.*` for framework specifics.
- **Backend detection**: look for `api/`, `server/`, `functions/`, `backend/` folders; check for `Dockerfile`, `requirements.txt`, `package.json` inside them.

2. Architecture and boundaries (how to infer)
- **Monorepo vs single-app**: if root contains `packages/` or `apps/`, assume monorepo (workspaces). Check `pnpm-workspace.yaml`, `lerna.json`, `turbo.json`, or `package.json` `workspaces` field for workspace rules.
- **Service boundaries**: infer from folders named `api`, `web`, `worker`, `cron`, `db` and from `Dockerfile`/`docker-compose.yml` service blocks.
- **Data flow & integration**: search for `fetch(`, `axios`, `graphql` queries, or `@/services/*` imports to find cross-service calls. Look at `env` files, `.env.*`, and `config/*` for service endpoints and secrets usage.

3. Developer workflows & commands (how to run/build/test)
- **If `package.json` exists (node/js repos)**: prefer the scripts defined there. Typical commands to try in PowerShell:
```
npm ci
npm run build
npm run dev
npm run test
```
- **If monorepo with pnpm**: use `pnpm install` and `pnpm -w` workspace commands.
- **Docker**: if `docker-compose.yml` present, use `docker compose up --build`.
- **CI**: check `.github/workflows/*.yml` for test/build steps used by maintainers.

4. Project-specific conventions to prefer here
- **Files/paths**: prefer `src/` as canonical source; `dist/` or `build/` as output — do not edit outputs unless asked.
- **Environment**: sensitive values are not stored in repo. Use `.env.example` or `config/*` as canonical variable names.
- **TypeScript**: if `tsconfig.json` exists, use its `paths` mapping (look at `baseUrl` and `paths`), and respect `skipLibCheck`/`strict` settings when fixing types.

5. Integration points & external dependencies
- **Databases**: presence of `prisma/`, `migrations/`, `schema.prisma`, or `alembic/` indicates an ORM-managed schema—avoid changing migrations without maintainer sign-off.
- **Third-party services**: detect `SENTRY_DSN`, `AWS_`, `GCP_`, `AZURE_` environment variables in `env` or CI workflows to discover external integrations.

6. How to produce a useful code change
- **Always check for existing scripts and tests**: prefer to add or update `scripts` in `package.json` rather than creating ad-hoc run commands.
- **Small, focused changes**: implement the minimal patch that passes existing tests and adheres to repo style (ESLint, Prettier, fmt files). If format tools are present, run them before proposing a PR.
- **Document assumptions**: when code requires new env variables, add them to `.env.example` and mention in your PR description.

7. If the repository is empty (this repo's current state)
- Ask the user which subpath contains the project sources or whether they want scaffolding.
- If the user asks to scaffold, propose a minimal manifest (e.g., `package.json`, `README.md`, `.gitignore`) and show the exact files you will create.

8. Examples (what to look for and how to act)
- Example: "If you see `apps/web/package.json` with `next` dependency, run `npm install` in `apps/web` and `npm run dev` there." Provide the exact path in commands.
- Example: "If you see `services/api` with `Dockerfile` and `requirements.txt`, run `docker build -t local-api ./services/api` and `pip install -r requirements.txt` in `services/api` virtualenv for local testing." Use PowerShell commands if giving CLI examples.

9. Merge guidance (when existing `.github/copilot-instructions.md` exists)
- Preserve maintainer-written sections; prefer to add a short `Repo state` paragraph at top describing detected structure and any commands needed to build/run.

10. Where to ask clarifying questions
- If key files or commands are missing, ask: "Which folder contains the app entrypoint?" or "Do you use npm/pnpm/yarn or another package manager?" or "Do you prefer a scaffolded start or will you add existing sources?"

---
Please confirm: do you want me to scaffold a minimal web project here, or point this file at an existing subfolder (if sources are in a different path)?
