## ADDED Requirements

### Requirement: Thematic Waiting Panel
The Waiting Panel SHALL use the "Infernal" visual style, including blood-red text colors and gothic-style button borders defined in `InfernalTheme.asset`.

#### Scenario: Visual application
- **WHEN** the Lobby scene is loaded or updated via `Setup Lobby UI`
- **THEN** all Text elements in the Waiting Panel SHALL have the `bloodRed` color.

### Requirement: Pulsating Room Code
The Room Code text SHALL pulsate gently while the player is waiting to provide visual feedback that the game is active.

#### Scenario: Pulse animation
- **WHEN** the Waiting Panel becomes active
- **THEN** the `RoomCodeText` SHALL trigger the `PulseScale` coroutine from `UIAnimationManager`.

### Requirement: Waiting Panel Layout
The Waiting Panel SHALL be centered and use a semi-transparent dark background to allow background pulsating circles to remain visible.

#### Scenario: Layout centering
- **WHEN** switching from Main Panel to Waiting Panel
- **THEN** the Waiting Panel elements SHALL appear centered on the screen with a consistent padding.
