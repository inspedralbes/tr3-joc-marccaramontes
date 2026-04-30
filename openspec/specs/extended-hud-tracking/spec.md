## ADDED Requirements

### Requirement: Kill Counter Display
The system SHALL track and display the total number of enemies defeated by the player in real-time.

#### Scenario: Real-time Kill Updates
- **WHEN** an enemy is destroyed by the player
- **THEN** the `GameManager` MUST increment the kill counter AND update the HUD text element.

#### Scenario: Visual Feedback on Kill
- **WHEN** a kill is registered
- **THEN** the kill counter text MUST perform a pulse scale animation.

### Requirement: Best Time Record Tracking
The system SHALL persist and display the best survival time recorded for the current game mode.

#### Scenario: Approaching Record Warning
- **WHEN** the current survival time is within 5 seconds of the `bestTime`
- **THEN** the record display color MUST change to yellow as a warning.
