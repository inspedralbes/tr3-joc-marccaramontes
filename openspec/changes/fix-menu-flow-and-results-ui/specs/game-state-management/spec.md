## ADDED Requirements

### Requirement: Global Game State
The `GameManager` SHALL maintain a global `GameState` enum to track the current phase of the application (Menu, Playing, GameOver).

#### Scenario: Initial State is Menu
- **WHEN** the application starts and the "Menu" scene is loaded
- **THEN** `GameManager.CurrentState` MUST be `GameState.Menu`

### Requirement: State-Protected Logic
The `GameManager` and other gameplay scripts SHALL only process death, survival time, and enemy spawning when the `GameState` is `GameState.Playing`.

#### Scenario: Prevent Death in Menu
- **WHEN** the active scene is "Menu" and a "Die" event is triggered
- **THEN** the `GameManager` MUST NOT process the death or reload any scene

### Requirement: Transition to Playing
The `GameManager` SHALL transition to `GameState.Playing` only when a game mode is explicitly started from the Menu.

#### Scenario: Starting Solo Mode
- **WHEN** the player clicks "Solo" in the menu
- **THEN** the `GameManager` MUST set `CurrentState` to `GameState.Playing` before loading the game scene
