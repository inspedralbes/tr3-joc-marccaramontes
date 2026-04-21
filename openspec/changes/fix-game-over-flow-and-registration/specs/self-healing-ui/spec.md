## ADDED Requirements

### Requirement: Emergency Component Recovery
The `GameManager` MUST be capable of finding and re-attaching the Results Panel and its sub-components using exhaustive scene search if primary references are null during the death sequence.

#### Scenario: Null Results Panel on Death
- **WHEN** the `ShowResults` method is called and `resultsPanel` is NULL
- **THEN** the system MUST use `Resources.FindObjectsOfTypeAll` to locate the "PanelResultados" object and re-bind its components (Time Text, Buttons, etc.)

### Requirement: Instant Fallback Activation
The UI system SHALL skip animations and display the Results Panel instantly if the `UIAnimationManager` is unavailable to prevent blocking the game flow.

#### Scenario: Missing UI Animation Manager
- **WHEN** the `ShowResultsSequence` is initiated and `UIAnimationManager.Instance` is NULL
- **THEN** the system MUST set `resultsPanel.SetActive(true)` and `CanvasGroup.alpha = 1` immediately without yielding
