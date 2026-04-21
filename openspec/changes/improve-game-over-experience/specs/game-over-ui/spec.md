## MODIFIED Requirements

### Requirement: Results Panel Navigation
The results panel SHALL include functional "Retry" and "Main Menu" buttons with high-contrast, responsive visual feedback to allow players to control the game flow after a match.

#### Scenario: Retry Button Action
- **WHEN** the player clicks the "Retry" button
- **THEN** the system MUST restart the current game mode in the current scene and reset the time scale to 1.0

#### Scenario: Main Menu Button Action
- **WHEN** the player clicks the "Main Menu" button
- **THEN** the system MUST return the player to the "Menu" scene, reset the game state, and reset the time scale to 1.0

### Requirement: Button Auto-Registration
The system SHALL ensure that navigation buttons in the results panel are automatically linked to the `GameManager` logic and the hover animation system.

#### Scenario: Dynamic Button Listener Assignment
- **WHEN** the `ResultsUIRegisterer` registers with the `GameManager`
- **THEN** it MUST assign the appropriate `GameManager` methods to the `OnClick` events and attach a `ButtonHoverEffect` component to each button
