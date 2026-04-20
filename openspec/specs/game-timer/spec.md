## ADDED Requirements

### Requirement: Real-time Survival Timer
The system SHALL track and display the current survival time in seconds on the HUD during gameplay.

#### Scenario: Timer Updates During Play
- **WHEN** the `GameState` is `Playing`
- **THEN** the HUD timer text MUST update every frame showing the elapsed time in "F2" format (e.g., 12.45s)

### Requirement: Timer HUD Registration
The system SHALL allow the HUD timer UI component to register itself with the `GameManager` upon initialization.

#### Scenario: Successful HUD Registration
- **WHEN** the `SampleScene` loads
- **THEN** the `ResultsUIRegisterer` MUST pass the reference of the timer text object to the `GameManager`
