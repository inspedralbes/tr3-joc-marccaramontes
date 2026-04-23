## ADDED Requirements

### Requirement: Competitive Survival Logic
In Online mode, the game SHALL continue running as long as at least one player is alive. The winner is determined by who survived for a longer duration.

#### Scenario: Local player dies first
- **WHEN** the local player falls in a 1vs1 match and the rival is still alive
- **THEN** the local player's survival timer MUST stop, but the game MUST NOT transition to the Results screen yet.

#### Scenario: Rival dies first
- **WHEN** a `DEATH` event is received for the rival while the local player is still alive
- **THEN** a HUD notification SHALL appear stating "Rival has fallen! Keep going!", and the match continues until the local player dies.

### Requirement: Duel HUD Synchronization
The system SHALL display real-time status of both players in the HUD during a match.

#### Scenario: HUD real-time updates
- **WHEN** a survival match is in progress
- **THEN** the HUD MUST show two timers or status indicators: "You: [Time]" and "Rival: [Time/Status]".
