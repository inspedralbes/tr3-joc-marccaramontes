## ADDED Requirements

### Requirement: Online Multiplayer Mode
The system SHALL support an "Online" game mode where survival time is synchronized via the server.

#### Scenario: Start Online Game
- **WHEN** the Host clicks "Start Match" in the Lobby
- **THEN** the server MUST broadcast a "match_start" event to all participants, and all clients SHALL load the game scene simultaneously
