## ADDED Requirements

### Requirement: Local High Score Persistence
The system SHALL save and load the player's best survival time locally.

#### Scenario: First Time Playing
- **WHEN** the game finishes for the first time
- **THEN** the system MUST save the current survival time as the new best time

#### Scenario: Breaking a Record
- **WHEN** the current survival time is greater than the stored best time
- **THEN** the system MUST update the stored best time with the new value

#### Scenario: Not Breaking a Record
- **WHEN** the current survival time is less than or equal to the stored best time
- **THEN** the system MUST NOT update the stored best time
