## MODIFIED Requirements

### Requirement: Multiple Implementations Support
Each repository SHALL provide at least two implementations: one using a real database (PostgreSQL) and one in-memory for testing and development.

#### Scenario: Switching implementations
- **WHEN** the environment variable `DATABASE_URL` is NOT present or is empty
- **THEN** the services MUST use the In-Memory implementations.

#### Scenario: Using PostgreSQL
- **WHEN** the environment variable `DATABASE_URL` is provided and the connection is successful
- **THEN** the services MUST use the PostgreSQL implementations.
