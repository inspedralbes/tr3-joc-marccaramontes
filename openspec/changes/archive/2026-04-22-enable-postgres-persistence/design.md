## Context

The current microservices architecture (API Service and Game Service) uses volatile in-memory repositories for storing user profiles and game results. To meet technical requirements for real persistence, the system needs to integrate with a PostgreSQL database.

## Goals / Non-Goals

**Goals:**
- Provide persistent storage for users and game results using PostgreSQL.
- Implement environment-based switching between In-Memory and PostgreSQL repositories.
- Ensure the system remains functional (falling back to In-Memory) if the database is unavailable.
- Define a clear SQL schema for the database.

**Non-Goals:**
- Implement complex database migrations (initial schema setup only).
- Support other database engines (e.g., MongoDB, MySQL) beyond PostgreSQL and In-Memory.

## Decisions

### 1. Centralized Repository Selection
Instead of manual instantiation in each service, we will implement a simple conditional logic based on the `DATABASE_URL` environment variable.
- **Rationale**: Keeps services decoupled from the specific implementation while allowing easy switching for dev/prod environments.

### 2. Schema Definition
A `schema.sql` file will be provided in `Server/common/scripts/` to initialize the database.
- **`users` table**: `username` (PK), `stats` (JSONB).
- **`results` table**: `id` (Serial), `room_id`, `player_name`, `score`, `duration`, `timestamp`.

### 3. Graceful Fallback
The PostgreSQL implementation will catch connection errors during initialization. If it fails, the service will log a warning and instantiate the In-Memory version instead.
- **Rationale**: Ensures developer experience isn't broken if they don't have Postgres running locally.

## Risks / Trade-offs

- **[Risk]** Database connection timeout slows down service startup.
  - **Mitigation**: Set a reasonable timeout for the initial connection check.
- **[Risk]** Data inconsistency between In-Memory and Postgres if switched dynamically.
  - **Mitigation**: Environment variables are static per session; the strategy is decided at startup.
- **[Risk]** JSONB complexity in stats.
  - **Mitigation**: Use simple key-value merging for statistics.
