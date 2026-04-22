## ADDED Requirements

### Requirement: Pure JSON Protocol
The system SHALL use standardized JSON messages for all real-time communication, following a `{type: string, payload: object}` structure.

#### Scenario: Synchronize Position
- **WHEN** a client sends a "MOVE" message via native WebSocket
- **THEN** the Game Service SHALL relay this JSON payload to all other participants in the same room.

### Requirement: Native WebSocket Transport
Communication SHALL be performed directly over the WebSocket protocol (RFC 6455) without using higher-level abstractions like Socket.io.

#### Scenario: Establish Connection
- **WHEN** the client connects to the Gateway URL via a standard WebSocket client
- **THEN** the connection MUST be successfully upgraded and handled by the Game Service.
