# Current-state documentation

## Purpose

This repository is a learning implementation of a .NET API that demonstrates user registration, password hashing, JWT bearer authentication, Swagger, Docker, and an Entity Framework Core in-memory store.

## Current state

- Target: ASP.NET Core on .NET 6.
- API surface: registration, login, and an authorized user-list endpoint.
- Persistence: Entity Framework Core in-memory database only; data is not durable between process instances.
- Authentication: locally configured symmetric JWT signing and bearer-token validation.
- Delivery: Dockerfile and Compose configuration are present.
- Tests and CI: none were found.

## Decision

Keep as a clearly labeled authentication learning reference, not as a portfolio flagship or deployable identity service. A future owner may archive it or create a separately scoped modernization effort.

See the [charter](project-charter.md), [architecture](architecture.md), and [delivery handoff](delivery-and-quality.md).
