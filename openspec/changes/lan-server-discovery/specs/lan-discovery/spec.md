## ADDED Requirements

### Requirement: LAN Server Broadcasting
The system SHALL allow a Host to broadcast its server information periodically on the local network via UDP.

#### Scenario: Host starts broadcasting
- **WHEN** the player clicks "Create Room" in the Lobby
- **THEN** the system SHALL begin sending UDP broadcast packets on port 4545 containing the local IP address every 2 seconds.

### Requirement: LAN Server Listening
The system SHALL allow a Client to listen for server broadcast packets on the local network and update the connection settings.

#### Scenario: Client discovers server
- **WHEN** the Client is on the Lobby screen and a valid broadcast packet is received
- **THEN** the system SHALL automatically update the server address input field and the NetworkManager configuration with the discovered IP.

### Requirement: Discovery Status Feedback
The system SHALL provide visual feedback to the user regarding the discovery process.

#### Scenario: Discovery in progress
- **WHEN** the Lobby screen is opened and no server is yet found
- **THEN** the system SHALL display a "Buscando servidor..." message near the server address field.

#### Scenario: Server found
- **WHEN** a server is successfully discovered
- **THEN** the system SHALL update the status message to "Servidor detectado en [IP]".

### Requirement: Manual Override
The system SHALL allow users to manually enter an IP address as a fallback.

#### Scenario: User enters IP manually
- **WHEN** the user starts typing in the server address field
- **THEN** the system SHALL temporarily pause automatic updates to that field to avoid overwriting the user's manual entry.
