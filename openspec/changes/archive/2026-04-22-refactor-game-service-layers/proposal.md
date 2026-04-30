## Why

To maintain architectural consistency across the backend and fully comply with technical requirements 4.1 and 4.2. The `game-service` currently has mixed responsibilities in `index.js`, which needs to be separated into Controller, Service, and Repository layers to ensure scalability and decoupled logic.

## What Changes

- **Refactor `Server/game-service/index.js`**: Convert it into a thin entry point that initializes the WebSocket server and wires dependencies.
- **New `SocketController.js`**: Create a controller to handle WebSocket message routing and packet parsing.
- **New `GameService.js`**: Create a service to manage the room state (`rooms` map) and broadcasting logic.
- **Dependency Injection**: Use manual DI to inject repositories into the `GameService`.
- **Improved Room Management**: Move the `Map` of room sockets to the service layer.

## Capabilities

### New Capabilities
- `socket-event-controller`: Defines how WebSocket events are dispatched and handled in a layered architecture.

### Modified Capabilities
- `layered-api-architecture`: Extend this capability to include WebSocket-based microservices.
- `game-state-management`: Update room lifecycle management in the context of persistent socket connections.

## Impact

- **Affected Code**: `Server/game-service/index.js` will be heavily refactored.
- **Architecture**: Completes the transition of the entire backend to a standardized layered pattern.
- **Maintainability**: Makes it easier to add new game events without cluttering the main entry point.
