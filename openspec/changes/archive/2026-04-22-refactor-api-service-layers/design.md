## Context

The `api-service` currently concentrates all its logic within a single `index.js` file. This includes HTTP route handling, business logic (room management, hashing), and direct instantiation of repository implementations. This structure violates technical requirement 4.2 and hinders maintainability and testing.

## Goals / Non-Goals

**Goals:**
- Implement a three-layer architecture: Controller, Service, and Repository.
- Decouple services from repository implementations using Dependency Injection.
- Centralize all business logic and validations within the Service layer.
- Maintain the existing external API contract for the Unity client.

**Non-Goals:**
- Refactoring the `game-service` in this specific change (to be handled in a separate task).
- Introducing an external Dependency Injection library (manual injection is preferred for simplicity).
- Changing the underlying database technology (Postgres/InMemory repositories remain as they are).

## Decisions

### Decision 1: Manual Dependency Injection
**Rationale:** Given the current scale of the project, a full DI container would add unnecessary complexity. Manual constructor injection in `index.js` provides enough flexibility and clear traceability.
**Alternatives Considered:** Using a library like `awilix` (overkill) or using a Singleton pattern for repositories (harder to test).

### Decision 2: Service-Layer State Management
**Rationale:** The `rooms` object, which represents the transient game state, will be moved from `index.js` to `RoomService`. This ensures that room logic (creation, joining) is encapsulated and can be easily tested.

### Decision 3: Controller as a Thin Layer
**Rationale:** Controllers will only handle request validation (e.g., checking for missing fields in the body) and response formatting. All heavy lifting and integration between multiple repositories will happen in the Service layer.

## Risks / Trade-offs

- **[Risk]** Breaking the Unity client integration during refactoring.
  - **Mitigation:** Comprehensive manual testing of the `api-service` endpoints before and after the refactor to ensure JSON responses remain identical.
- **[Risk]** Increased boilerplate code for small operations.
  - **Mitigation:** The long-term benefits of testability and adherence to requirements outweigh the initial cost of more files.

## Hierarchy & Structure

```text
Server/api-service/
├── controllers/
│   ├── RoomController.js   (req, res, delegating to RoomService)
│   └── ResultController.js (req, res, delegating to ResultService)
├── services/
│   ├── RoomService.js      (Rooms object, room logic, uses IUserRepository)
│   └── ResultService.js    (Results logic, uses IResultRepository and IUserRepository)
└── index.js                (App setup, DI wiring)
```
