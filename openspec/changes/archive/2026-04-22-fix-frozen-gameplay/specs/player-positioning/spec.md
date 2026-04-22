## ADDED Requirements

### Requirement: Local Player Self-Identification
The `PlayerMovement` system SHALL automatically identify as the "Local Player" when the game is running in a non-networked state (Solo mode or Editor test).

#### Scenario: Automatic authority in Solo mode
- **WHEN** the game is in `GameMode.Solo` or there is no active `NetworkManager` room
- **THEN** the `NetworkIdentity` component MUST set `isLocalPlayer` to true to enable physics and input
