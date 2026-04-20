## ADDED Requirements

### Requirement: State-Aware Mode Start
The system SHALL ensure that a game mode is only fully active and "Playing" when it has been explicitly triggered from the Menu.

#### Scenario: Return to Menu Resets State
- **WHEN** the player returns to the menu
- **THEN** the `GameManager` MUST reset the `GameState` to `Menu`

### Requirement: Mode Transitions
The `GameManager` SHALL manage transitions between game states based on the active mode (Solo or Multiplayer).

#### Scenario: Multiplayer Results State
- **WHEN** the second player in Multiplayer mode dies
- **THEN** the `GameManager` MUST transition to `GameState.GameOver` before showing results
