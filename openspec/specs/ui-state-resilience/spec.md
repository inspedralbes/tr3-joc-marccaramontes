# ui-state-resilience Specification

## Purpose
TBD - created by archiving change fix-death-screen-visibility. Update Purpose after archive.
## Requirements
### Requirement: Automatic UI Visibility
The system MUST ensure that critical UI canvases, specifically the death screen, are initialized with a scale of (1, 1, 1) and are correctly positioned within the screen space to be visible to the player.

#### Scenario: Death screen initialization
- **WHEN** the `SampleScene` is loaded or the setup tool is run
- **THEN** the `CanvasResultados` MUST have a local scale of (1, 1, 1) and its render mode MUST be `Screen Space - Overlay`

### Requirement: Robust UI Component Registration
The system SHALL provide a mechanism to automatically link UI components (panels, buttons, text fields) to their respective registerer scripts to prevent null reference errors at runtime.

#### Scenario: Automated UI setup
- **WHEN** the "Setup HUD and Results UI" tool is executed in the editor
- **THEN** the `ResultsUIRegisterer` MUST be updated with correct references to `PanelResultados`, `P1TimeText`, `P2TimeText`, `WinnerText`, `RetryButton`, and `MenuButton`

