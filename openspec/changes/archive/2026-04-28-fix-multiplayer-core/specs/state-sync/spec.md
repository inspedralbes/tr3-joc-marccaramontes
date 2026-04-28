## MODIFIED Requirements

### Requirement: Player Position Sync
The system SHALL synchronize the position of all players in a room in real-time using the `MOVE` event type.

#### Scenario: Update Remote Ghost
- **WHEN** a local player moves
- **THEN** the client MUST emit its new position with the type `MOVE` to the server, and the server SHALL relay it with the sender's `playerId` to all other clients to update their local ghosts
