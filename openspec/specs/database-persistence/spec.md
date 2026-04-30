# database-persistence Specification

## Purpose
Defines the SQL schema and connectivity requirements for the microservices architecture.

## Requirements

### Requirement: SQL Schema Definition
The system SHALL provide a SQL schema that defines the structure for `users` and `results` tables.

#### Scenario: Schema initialization
- **WHEN** the `schema.sql` script is executed against a PostgreSQL database
- **THEN** it creates the `users` table (with `username` as PK and `stats` as JSONB) and the `results` table (with room tracking and scores).

### Requirement: Database Connectivity
The PostgreSQL repositories SHALL establish a connection to the database using the connection string provided in the `DATABASE_URL` environment variable.

#### Scenario: Connection established
- **WHEN** the `DATABASE_URL` is valid and the service starts
- **THEN** the PostgreSQL repository successfully connects and is ready for operations.

### Requirement: Connection Resilience
The PostgreSQL repositories SHALL handle connection failures gracefully during initialization.

#### Scenario: Connection failure fallback
- **WHEN** the service fails to connect to the database at startup
- **THEN** it logs a warning and allows the service to fall back to an In-Memory implementation.
