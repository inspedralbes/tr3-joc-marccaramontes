# socket-event-controller Specification

## Purpose
Defines the WebSocket routing and broadcasting logic for microservices to ensure consistent message handling and centralized room management.

## Requirements

### Requirement: Standardized Message Routing
The WebSocket server SHALL use a dedicated Controller (SocketController) to route incoming JSON packets based on their `type` field.

#### Scenario: Routing a JOIN_ROOM message
- **WHEN** the server receives a `{ "type": "JOIN_ROOM", "payload": { ... } }` packet
- **THEN** the SocketController MUST identify the type and call the corresponding `handleJoinRoom` method in the Service layer.

### Requirement: Centralized Broadcast Management
The system SHALL centralize the broadcasting of messages to rooms within the Service layer, ensuring consistent delivery to all connected clients except the sender (when appropriate).

#### Scenario: Broadcasting player movement
- **WHEN** a MOVE message is processed
- **THEN** the GameService MUST iterate through the sockets in the sender's room and send the packet to all other clients.

### Requirement: Validated Room Connections
The GameService MUST validate that a room exists and the player is registered in the Repository before allowing a connection to be finalized.

#### Scenario: Unknown player joining
- **WHEN** a player not in the UserRepository attempts to join a socket room
- **THEN** the system MUST log a warning and handle the connection according to business rules.
