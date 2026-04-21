## ADDED Requirements

### Requirement: Death Slow Motion
The system SHALL reduce the game speed when the player dies to emphasize the event.

#### Scenario: Player Dies
- **WHEN** the player's health reaches zero or they go out of bounds
- **THEN** the system MUST set `Time.timeScale` to 0.3 or lower immediately

### Requirement: Results Display Delay
The system SHALL wait for a short period before displaying the results panel after death.

#### Scenario: Post-Death Wait
- **WHEN** the death sequence starts
- **THEN** the system MUST wait for 1.5 seconds (real time) before showing the results panel

### Requirement: Transition Cleanup
The system SHALL clear the gameplay area of active threats when the death transition is complete.

#### Scenario: Cleanup Enemies and Bullets
- **WHEN** the results panel is about to be displayed
- **THEN** the system MUST destroy all active enemy and bullet game objects in the scene
