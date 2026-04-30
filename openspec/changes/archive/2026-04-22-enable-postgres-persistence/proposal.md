## Why

To satisfy technical requirement 4.2 (Real Database Persistence) and ensure data persists beyond application restarts. This change transitions the project from a volatile in-memory state to a functional PostgreSQL database while maintaining a stable fallback for development environments without a running database.

## What Changes

- **Environment-Based Persistence**: Update microservices to automatically switch between `InMemory` and `Postgres` repositories based on the presence of a `DATABASE_URL` environment variable.
- **Shared Data Access Layer**: Finalize the `PostgresUserRepository` and `PostgresResultRepository` to handle standard SQL operations for game entities.
- **Initialization Script**: Provide a simple `schema.sql` to set up the necessary tables (`users`, `results`) with correct types (including JSONB for stats).
- **Graceful Fallback**: Ensure the system remains functional even if database connectivity fails, falling back to In-Memory mode to keep the game playable during development.

## Capabilities

### New Capabilities
- `database-persistence`: Defines the SQL schema and connectivity requirements for the microservices architecture.

### Modified Capabilities
- `repository-pattern-persistence`: Clarify the switching logic between real and test implementations.

## Impact

- **Infrastructure**: Requires a PostgreSQL instance for real persistence.
- **Reliability**: Improves data durability for user statistics and match results.
- **Configuration**: Introduces new environment variables for database credentials.
