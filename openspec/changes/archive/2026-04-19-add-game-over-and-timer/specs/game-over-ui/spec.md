## ADDED Requirements

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
