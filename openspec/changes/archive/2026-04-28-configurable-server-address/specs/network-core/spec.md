## MODIFIED Requirements

### Requirement: Server Connection
The Unity client MUST be able to establish and maintain a connection with the Node.js server via Native WebSockets. The connection address MUST be resolvable dynamically from user input or configuration.

#### Scenario: Successful Connection with Dynamic Address
- **WHEN** the user initiates a connection from the Lobby with a specific server address
- **THEN** the `NetworkManager` SHALL construct the Gateway URL (port 3000) and establish a unique WebSocket session through it
