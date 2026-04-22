## MODIFIED Requirements

### Requirement: Real-time Survival Timer
The system SHALL track and display the current survival time in seconds on the HUD during gameplay using a single, high-visibility centralized display as the single source of truth.

#### Scenario: Timer Updates During Play
- **WHEN** the `GameState` is `Playing`
- **THEN** the HUD timer text MUST update every frame showing the elapsed time in "F2" format (e.g., 12.45s).

#### Scenario: Centralized Styled Display
- **WHEN** the `HUD` is active
- **THEN** the timer MUST be anchored to the top-center of the screen with a font size >= 48 and colored white throughout the match duration.

### Requirement: Timer HUD Registration
The system SHALL ensure that exactly one HUD timer UI component is registered with the `GameManager` upon initialization, preventing duplicate or ghost timers.

#### Scenario: Successful HUD Registration
- **WHEN** the `SampleScene` loads
- **THEN** the `ResultsUIRegisterer` MUST pass the reference of the unique timer text object within the `HUDGroup` to the `GameManager`.

#### Scenario: Duplicate Timer Removal
- **WHEN** the `SampleScene` is initialized or repaired
- **THEN** any `TimerHUD` GameObject not part of the `HUDGroup` MUST be deactivated or removed to ensure visual clarity.
