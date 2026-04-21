## ADDED Requirements

### Requirement: Match Result Reporting
The system SHALL report the final match results (survival time) to the server via an HTTP POST request upon player death in online mode.

#### Scenario: Send Final Survival Time
- **WHEN** the player dies in an online match
- **THEN** the Unity client MUST send a POST request to `/api/results` with the `roomId`, `playerName`, and `survivalTime`
