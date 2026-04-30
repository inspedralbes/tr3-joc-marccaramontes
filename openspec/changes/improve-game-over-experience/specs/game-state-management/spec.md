## MODIFIED Requirements

### Requirement: Editor Safe Start
The `GameManager` SHALL automatically transition to `GameState.Playing` if it initializes (Start) within a designated gameplay scene (e.g., "SampleScene") while currently in `GameState.Menu`.

#### Scenario: Direct scene entry in Editor
- **WHEN** the "SampleScene" is played directly from the Unity Editor
- **THEN** the `GameManager` MUST set its state to `GameState.Playing` to enable gameplay logic and death processing

### Requirement: Death State Freeze
The `GameManager` SHALL immediately freeze the game simulation upon player death to focus on the results.

#### Scenario: Immediate Time Scale Freeze
- **WHEN** `ProcessDeath` is called
- **THEN** the system MUST set `Time.timeScale` to 0 and transition the state to `GameState.DeathTransition`
