## ADDED Requirements

### Requirement: Player Name Configuration
The system MUST allow the user to define a display name before participating in any online match.

#### Scenario: Name Entry Required
- **WHEN** the user opens the Lobby scene
- **THEN** the "Create Room" and "Join Room" buttons MUST be disabled until the player name input field is non-empty

### Requirement: Name Persistence and Sync
The player's name MUST be synchronized with other participants in the match.

#### Scenario: View Remote Player Name
- **WHEN** a player joins a room
- **THEN** their defined name MUST be visible to all other participants in the UI or over their character
