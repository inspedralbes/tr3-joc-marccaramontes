## ADDED Requirements

### Requirement: Repository Abstraction
The backend MUST implement the Repository Pattern for `User`, `Game`, and `Result` entities to decouple business logic from data storage.

#### Scenario: Save User Data
- **WHEN** a Service calls `UserRepository.save(user)`
- **THEN** the active Repository implementation (PostgreSQL or InMemory) SHALL persist the data correctly.

### Requirement: Multiple Implementations
The system SHALL support at least two implementations of each repository: one for persistent storage (PostgreSQL) and one for transient storage (InMemory).

#### Scenario: Switching Implementations
- **WHEN** the server environment is set to "test"
- **THEN** the system MUST instantiate `InMemory` repositories instead of the database-backed ones.
