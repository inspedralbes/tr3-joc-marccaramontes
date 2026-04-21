## ADDED Requirements

### Requirement: Local High Score Storage
The system SHALL save the player's best survival time to the local machine across gaming sessions.

#### Scenario: Save New High Score
- **WHEN** the current survival time exceeds the saved "Best Time"
- **THEN** the system MUST update the local storage with the new value

### Requirement: Record Achievement Feedback
The system SHALL identify when a new record has been achieved to allow visual highlighting in the UI.

#### Scenario: New Record Flag
- **WHEN** a match ends and the survival time is greater than the previous best
- **THEN** the system MUST set a boolean flag `isNewRecord` to TRUE for the duration of the Results UI display
