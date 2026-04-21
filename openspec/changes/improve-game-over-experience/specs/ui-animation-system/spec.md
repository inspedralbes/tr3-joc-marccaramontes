## ADDED Requirements

### Requirement: Global UI Fade In/Out
The system SHALL provide a unified method for fading UI panels smoothly using Alpha transparency.

#### Scenario: Fade In Results Panel
- **WHEN** the results UI is shown
- **THEN** the `CanvasGroup.alpha` MUST transition from 0 to 1 over 0.5 seconds

### Requirement: Numeric Digital Ticker
The system SHALL animate numeric values in the UI by counting up rapidly from zero to the target value.

#### Scenario: Survival Time Ticker
- **WHEN** the results UI finishes fading in
- **THEN** the survival time text MUST count from 0 to the final value within 1 second

### Requirement: Interactive Button Scale
The system SHALL provide visual feedback for button interactions by scaling the button's size upon hovering.

#### Scenario: Button Hover Feedback
- **WHEN** the player hovers over a button in the results panel
- **THEN** the button scale MUST increase by 10% (1.1x) smoothly and revert to 1.0x on pointer exit
