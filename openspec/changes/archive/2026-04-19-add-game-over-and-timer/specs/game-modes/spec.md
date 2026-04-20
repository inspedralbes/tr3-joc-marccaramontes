## ADDED Requirements

### Requirement: Solo Mode Death Flow
The system SHALL transition to the `GameOver` state and show the results panel when the player dies in Solo mode.

#### Scenario: Solo Death Shows Results
- **WHEN** the player dies in Solo mode
- **THEN** the system MUST transition to `GameState.GameOver` and show the results panel with the final survival time
