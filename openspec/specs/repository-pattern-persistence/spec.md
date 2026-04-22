# repository-pattern-persistence Specification

## Purpose
Defines the requirements for Interface Isolation and support for Multiple Implementations (InMemory and PostgreSQL) within the repository pattern.

## Requirements

### Requirement: Repository-Interface Isolation
Each Repository SHALL have a corresponding interface (e.g., `IUserRepository.js`) that defines all data access methods.

#### Scenario: Interface implementation
- **WHEN** a new data access method is needed
- **THEN** it is first added to the interface as a method that throws "Not Implemented", ensuring all concrete implementations follow the contract.

### Requirement: Multiple Implementations Support
Each repository SHALL provide at least two implementations: one using a real database (PostgreSQL) and one in-memory for testing and development.

#### Scenario: Switching implementations
- **WHEN** the environment variable `USE_POSTGRES` is false
- **THEN** the API Service MUST use the `InMemoryUserRepository` instead of `PostgresUserRepository`.
