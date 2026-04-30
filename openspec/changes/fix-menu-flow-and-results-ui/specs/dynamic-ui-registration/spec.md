## ADDED Requirements

### Requirement: UI Self-Registration
The `GameManager` SHALL provide a mechanism for UI components in a game scene to register their references upon being loaded.

#### Scenario: Results Panel Registration
- **WHEN** the `SampleScene` is loaded and a `ResultsUI` component is initialized
- **THEN** it MUST register its `GameObject` and child text references to the `GameManager`

### Requirement: Robust Results Display
The `GameManager` SHALL use registered UI references to display results, avoiding the use of `GameObject.Find`.

#### Scenario: Show Results with Registered UI
- **WHEN** the game transitions to `GameState.GameOver` and `ShowResults()` is called
- **THEN** it MUST use the previously registered references to update text and show the panel
