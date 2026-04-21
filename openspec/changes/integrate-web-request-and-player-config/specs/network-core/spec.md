## MODIFIED Requirements

### Requirement: Server Connection
The Unity client MUST be able to establish and maintain a connection with the Node.js server via Socket.io only after the room management phase via HTTP.

#### Scenario: Successful WebSocket Connection after HTTP Room Join
- **WHEN** the room creation or join request succeeds via HTTP
- **THEN** the `NetworkManager` SHALL establish the WebSocket connection specifically for the obtained `roomId`
