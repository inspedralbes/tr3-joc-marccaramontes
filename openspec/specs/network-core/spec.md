## ADDED Requirements

### Requirement: Server Connection
The Unity client MUST be able to establish and maintain a connection with the Node.js server via Native WebSockets. The connection address MUST be resolvable dynamically from user input or configuration.

#### Scenario: Successful Connection with Dynamic Address
- **WHEN** the user initiates a connection from the Lobby with a specific server address
- **THEN** the `NetworkManager` SHALL construct the Gateway URL (port 3000) and establish a unique WebSocket session through it

### Requirement: Connection Resilience
The system SHALL handle unexpected disconnections by notifying the user and returning them to the main menu.

#### Scenario: Server Disconnection
- **WHEN** the socket connection is lost during a match
- **THEN** the game MUST stop, show a "Connection Lost" message, and load the Menu scene

### Requirement: Agent State Synchronization
The system SHALL provide a dedicated networking event, `ENEMY_SYNC`, to transmit the current state (position, rotation, activity) of non-player agents from the Host to all clients.

#### Scenario: Real-time agent sync
- **WHEN** the Host's hunter agent moves during a match
- **THEN** it MUST emit an `ENEMY_SYNC` event at regular intervals to update all clients

### Requirement: Client-Side Movement Smoothing
Remote clients SHALL use interpolation to smoothly transition agents between received network positions, preventing jerky or "teleporting" movement.

#### Scenario: Smooth agent movement
- **WHEN** a client receives an `ENEMY_SYNC` update
- **THEN** it SHALL smoothly move the local proxy of the agent toward the new position using a Lerp-based approach

### Requirement: Robust JSON Message Envelope
All communication between client and server SHALL follow a standard JSON envelope: `{"type": "EVENT_NAME", "payload": "JSON_STRING_PAYLOAD"}`.

#### Scenario: Valid packet parsing
- **WHEN** the server receives a message from Unity
- **THEN** it MUST correctly parse the outer envelope AND the nested JSON payload within the `payload` field before processing logic
