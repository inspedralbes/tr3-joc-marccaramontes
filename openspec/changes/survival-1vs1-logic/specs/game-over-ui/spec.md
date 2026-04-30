## MODIFIED Requirements

### Requirement: Results Panel Navigation
The results panel SHALL include functional "Retry" and "Main Menu" buttons to allow players to control the game flow after a match.

#### Scenario: Retry Button Action
- **WHEN** the player clicks the "Retry" button
- **THEN** the system MUST restart the current game mode in the current scene

#### Scenario: Main Menu Button Action
- **WHEN** the player clicks the "Main Menu" button
- **THEN** the system MUST return the player to the "Menu" scene and reset the game state

### Requirement: Button Auto-Registration
The system SHALL ensure that navigation buttons in the results panel are automatically linked to the `GameManager` logic.

#### Scenario: Dynamic Button Listener Assignment
- **WHEN** the `ResultsUIRegisterer` registers with the `GameManager`
- **THEN** it MUST assign the appropriate `GameManager` methods to the `OnClick` events of the Retry and Menu buttons

### Requirement: Results Title Display
The Results Panel SHALL display a prominent title text indicating the end of the match (e.g., "PARTIDA FINALIZADA" or "GAME OVER").

#### Scenario: Title Appearance
- **WHEN** the results UI is activated
- **THEN** a header text element MUST display "PARTIDA FINALIZADA" in a larger font size than the statistics.

### Requirement: Stat Reliability Guarantee
The Results Panel SHALL immediately display the correct match statistics (Kills and Survival Time) upon being shown to ensure data integrity for the player.

#### Scenario: Immediate Stat Initialization
- **WHEN** the results UI finishes its initial activation/fade-in
- **THEN** the survival time and kill count MUST be visible with their final match values.

### Requirement: Enhanced Stat Animations
The Results Panel SHALL use parallel or sequenced animations (pulses or fades) to reveal statistics, providing a high-impact "arcade recap" experience.

#### Scenario: Sequenced Stat Reveal
- **WHEN** the results panel title is shown
- **THEN** the Kills and Time statistics MUST follow with a slight delay and a visual pulse effect.

## ADDED Requirements

### Requirement: Competitive Results Display
In 1vs1 mode, the Results Panel SHALL display the survival times of both the local player and the rival to declare a clear winner.

#### Scenario: Victory display
- **WHEN** the local player's survival time is greater than the rival's
- **THEN** a "VICTORIA!" title SHALL appear and both times MUST be visible for comparison.

#### Scenario: Defeat display
- **WHEN** the local player's survival time is less than the rival's
- **THEN** a "DERROTA!" title SHALL appear and both times MUST be visible for comparison.
