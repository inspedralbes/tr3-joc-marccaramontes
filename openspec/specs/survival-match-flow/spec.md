## ADDED Requirements

### Requirement: Survival Timer Halting
In Online mode, the survival timer for the local player SHALL stop immediately upon their death, even if the match continues for other players.

#### Scenario: Local death stops timer
- **WHEN** the local player dies in a 1vs1 match
- **THEN** the HUD timer MUST stop updating and keep the final survival time displayed.

### Requirement: Waiting State Feedback
The system SHALL provide visual feedback when the local player has died but the match is still active for other participants.

#### Scenario: Waiting for rival
- **WHEN** the local player dies and at least one rival is still alive
- **THEN** the HUD MUST display a message indicating that the system is "Waiting for rival...".
