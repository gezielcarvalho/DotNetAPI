# Project charter

## Learning objective

Practice a basic ASP.NET Core API lifecycle: registration, password hashing, JWT creation/validation, protected routes, Swagger, and local containerization.

## Scope represented today

- Registration and login endpoints backed by an in-memory Entity Framework Core context.
- Password hashing with PBKDF2.
- JWT bearer authentication and a protected user-list endpoint.
- Local Docker build and Compose configuration.

## Not in scope

- A production identity provider, durable user store, or account-recovery workflow.
- Key rotation, secret management, MFA, email verification, lockout, abuse prevention, or audit logging.
- Production deployment, monitoring, support, test automation, or CI.

## Ownership and decision gate

This repository remains a personal learning archive. Do not use it to authenticate a production system. Retain it only as an explanatory reference or archive it after owner review.

## Modernization acceptance criteria

1. Move to a supported .NET runtime and align framework packages.
2. Replace public sample configuration with secret injection and key-rotation practices.
3. Adopt a durable datastore with migrations and privacy/data-retention decisions.
4. Define an authentication threat model, validation/error contract, and authorization policy.
5. Add automated tests, CI, structured logs, health checks, and deployment evidence.
