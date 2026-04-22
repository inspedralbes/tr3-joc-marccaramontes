## Why

To comply with technical requirement 4.2 of the project, which mandates a clear separation of responsibilities (Controller, Service, Repository) and the use of the Repository pattern to decouple business logic from persistence technology. The current implementation in `api-service/index.js` mixes these layers, making it harder to maintain, scale, and test independently.

## What Changes

- **Refactor `Server/api-service/index.js`**: Remove business logic and direct repository instantiation, turning it into a pure entry point that configures and starts the server.
- **New Directory Structure**: Create `controllers/` and `services/` directories within `Server/api-service/`.
- **Layered Implementation**:
    - **Controllers**: Move HTTP request/response handling to dedicated controller classes.
    - **Services**: Move business logic (room creation, results processing, hashing) to service classes.
- **Dependency Injection**: Implement a basic DI pattern in `index.js` to inject repository implementations into services, making them technology-agnostic.
- **Security**: Centralize password hashing and validation within the Service layer.

## Capabilities

### New Capabilities
- `layered-api-architecture`: Formalizes the internal structure of microservices following the Controller-Service-Repository pattern.

### Modified Capabilities
- `repository-pattern-persistence`: Clarify that Services must use Repositories via interfaces without knowing the underlying implementation.
- `game-state-management`: Update how room lifecycle is managed through the new service layer.

## Impact

- **Affected Code**: `Server/api-service/index.js` will be completely overhauled.
- **Microservices**: Establishes a template for how other services (like `game-service`) should be structured.
- **Testing**: Enables unit testing of `RoomService` and `ResultService` using `InMemory` repositories without needing an Express server.
