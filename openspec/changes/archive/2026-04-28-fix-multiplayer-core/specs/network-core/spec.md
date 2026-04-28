## MODIFIED Requirements

### Requirement: Server Connection
The Unity client MUST be able to establish and maintain a connection with the Node.js server via native WebSockets.

#### Scenario: Successful Connection
- **WHEN** the game starts or the user enters the Lobby
- **THEN** the `NetworkManager` SHALL establish a native WebSocket connection and handle the protocol handshake correctly

### Requirement: Robust JSON Message Envelope
All communication between client and server SHALL follow a standard JSON envelope: `{"type": "EVENT_NAME", "payload": "JSON_STRING_PAYLOAD"}`.

#### Scenario: Valid packet parsing
- **WHEN** the server receives a message from Unity
- **THEN** it MUST correctly parse the outer envelope AND the nested JSON payload within the `payload` field before processing logic
