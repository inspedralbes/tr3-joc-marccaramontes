## ADDED Requirements

### Requirement: Server Connection
The Unity client MUST be able to establish and maintain a connection with the Node.js server via Socket.io.

#### Scenario: Successful Connection
- **WHEN** the game starts or the user enters the Lobby
- **THEN** the `NetworkManager` SHALL emit a "connected" status and establish a unique socket session

### Requirement: Connection Resilience
The system SHALL handle unexpected disconnections by notifying the user and returning them to the main menu.

#### Scenario: Server Disconnection
- **WHEN** the socket connection is lost during a match
- **THEN** the game MUST stop, show a "Connection Lost" message, and load the Menu scene
