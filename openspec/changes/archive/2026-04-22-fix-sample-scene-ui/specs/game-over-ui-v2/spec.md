## ADDED Requirements

### Requirement: Results Title Display
The Results Panel SHALL display a prominent title text indicating the end of the match (e.g., "PARTIDA FINALIZADA" or "GAME OVER").

#### Scenario: Title Appearance
- **WHEN** the results UI is activated
- **THEN** a header text element MUST display "PARTIDA FINALIZADA" in a larger font size than the statistics.

### Requirement: Stat Reliability Guarantee
The Results Panel SHALL immediately display the correct match statistics (Kills and Survival Time) upon being shown to ensure data integrity for the player.

#### Scenario: Immediate Stat Initialization
- **WHEN** the results UI finishes its initial activation/fade-in
- **THEN** the survival time and kill count MUST be visible with their final match values.

### Requirement: Enhanced Stat Animations
The Results Panel SHALL use parallel or sequenced animations (pulses or fades) to reveal statistics, providing a high-impact "arcade recap" experience.

#### Scenario: Sequenced Stat Reveal
- **WHEN** the results panel title is shown
- **THEN** the Kills and Time statistics MUST follow with a slight delay and a visual pulse effect.

## REMOVED Requirements

### Requirement: High Score Persistence
**Reason**: Project scope change; local high-score records are no longer desired.
**Migration**: Remove `bestTime` variables and `PlayerPrefs` logic from `GameManager.cs`.

### Requirement: Record Achievement Feedback
**Reason**: Replaced by static results display without record comparisons.
**Migration**: Delete `BestTimeHUD`, `BestTimeText`, and `NewRecordBadge` from scenes and code.
