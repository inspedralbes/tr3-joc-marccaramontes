## ADDED Requirements

### Requirement: Real-time Kill HUD Tracking
The system SHALL display the current number of kills (enemies destroyed) during gameplay.

#### Scenario: Update Kill Counter
- **WHEN** the player destroys an enemy
- **THEN** the `KillsHUDText` MUST increment its value by 1.

#### Scenario: Kill Pulse Animation
- **WHEN** the kill count increases
- **THEN** the `KillsHUDText` MUST execute a brief scale-up pulse animation (e.g., scale from 1.0 to 1.2 and back).

### Requirement: Best Time Record HUD Display
The system SHALL display the player's personal best survival record during gameplay.

#### Scenario: Real-time Record Approach
- **WHEN** the current `survivalTime` is within 5 seconds of `bestTime`
- **THEN** the `BestTimeHUDText` SHOULD change its color to alert the player.

#### Scenario: Record Breaking Signal
- **WHEN** the current `survivalTime` exceeds `bestTime`
- **THEN** the `TimerCenterHUDText` MUST change its color to orange (#FF8000) for the remainder of the session.
