## ADDED Requirements

### Requirement: Local Player Death Camera
Upon local player death in 1vs1 mode, if the rival is alive, the camera SHALL immediately transition to follow the rival.

#### Scenario: Follow the survivor
- **WHEN** the local player dies and the rival is still alive
- **THEN** the camera's focus MUST switch from the dead player's transform to the `RemotePlayerManager`'s reference for the rival.

### Requirement: Spectator UI overlay
A spectator UI SHALL appear when viewing a rival's gameplay after death.

#### Scenario: Spectator overlay visibility
- **WHEN** the camera is following a rival
- **THEN** a "SPECTATING" indicator SHALL be visible on screen.
