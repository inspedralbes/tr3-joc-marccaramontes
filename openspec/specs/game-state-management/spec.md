# game-state-management Specification

## Purpose
TBD - created by archiving change fix-death-screen-visibility. Update Purpose after archive.
## Requirements
### Requirement: Editor Safe Start
The `GameManager` SHALL automatically transition to `GameState.Playing` if it initializes (Start) within a designated gameplay scene (e.g., "SampleScene") while currently in `GameState.Menu`.

#### Scenario: Direct scene entry in Editor
- **WHEN** the "SampleScene" is played directly from the Unity Editor
- **THEN** the `GameManager` MUST set its state to `GameState.Playing` to enable gameplay logic and death processing

