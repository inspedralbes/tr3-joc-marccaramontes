## ADDED Requirements

### Requirement: Enhanced Results Stats
The results panel SHALL display the player's best survival time alongside the current score.

#### Scenario: Display High Score
- **WHEN** the results panel is shown
- **THEN** it MUST display the `BestTime_Solo` value from persistence

#### Scenario: New Record Feedback
- **WHEN** the current survival time exceeds the previous best
- **THEN** the results panel MUST show a visual indicator for a "NEW RECORD"
