## ADDED Requirements

### Requirement: Solo Mode Death Flow
The system SHALL transition to the `GameOver` state and show the results panel when the player dies in Solo mode.

#### Scenario: Solo Death Shows Results
- **WHEN** the player dies in Solo mode
- **THEN** the system MUST transition to `GameState.GameOver` and show the results panel with the final survival time

### Requirement: Online Multiplayer Mode
The system SHALL support an "Online" game mode where survival time is synchronized via the server.

#### Scenario: Start Online Game
- **WHEN** the Host clicks "Start Match" in the Lobby
- **THEN** the server MUST broadcast a "match_start" event to all participants, and all clients SHALL load the game scene simultaneously
