## ADDED Requirements

### Requirement: Remote Player Disconnection Handling
The `NetworkManager` MUST listen for the "player_left" event from the server and notify other components.

#### Scenario: Peer Leaves Room
- **WHEN** a remote player disconnects or leaves the room
- **THEN** the `NetworkManager` SHALL fire an `OnRemotePlayerLeft` event with the `playerId`.

### Requirement: Standardized Network Event Naming
The client and server MUST use the same event names for game actions to ensure reliable communication.

#### Scenario: Shooting Sync
- **WHEN** a player shoots
- **THEN** the client SHALL emit "player_shoot" and listen for "player_shot" (as per server translation) or use a unified name if the server is updated.
