# Architecture and security boundaries

## Components

| Component | Current responsibility |
| --- | --- |
| `Program.cs` | Registers controllers, Swagger, JWT bearer authentication, authorization, and the in-memory EF Core context. |
| `Controllers/UserController.cs` | Registration, login, token creation, password verification, and the protected user-list endpoint. |
| `Models/` | User, registration/login request, and EF Core context types. |
| `appsettings*.json` | Local logging and JWT configuration. |
| `Dockerfile` / `docker-compose.yml` | Local container build and configuration mount. |

## Request flow

1. Registration validates the request model, hashes the password, and stores the user in the process-local in-memory database.
2. Login verifies the stored hash and creates a signed bearer token.
3. The user-list route requires the bearer authentication middleware.
4. Restarting the application clears registered users because there is no durable store.

## Security boundary

The checked-in JWT setting is a public, sample-only value—not a secret and not suitable for any deployment. It must be replaced through a secure configuration mechanism before any non-local use. The current JWT validation settings and simple user lifecycle are appropriate only for an instructional demo; they do not establish a production authentication posture.

## Reliability gaps

- No persistent store, migrations, test suite, CI workflow, health checks, rate limiting, or centralized telemetry.
- No key rotation, recovery, verification, lockout, or abuse controls.
- The Docker Compose configuration mounts application configuration directly, which is acceptable only for a local learning environment.
- Runtime and package versions require a supported-runtime modernization plan before active maintenance.
