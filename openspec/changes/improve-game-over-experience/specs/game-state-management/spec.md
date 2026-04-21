## ADDED Requirements

### Requirement: Death Transition State
The `GameManager` SHALL include a temporary state to handle the delay between player death and the final Game Over state.

#### Scenario: Death Trigger
- **WHEN** the player dies during gameplay
- **THEN** the `GameManager` MUST set its state to `GameState.DeathTransition`

#### Scenario: Transition End
- **WHEN** the death delay has elapsed
- **THEN** the `GameManager` MUST set its state to `GameState.GameOver`
