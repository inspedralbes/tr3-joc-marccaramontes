## ADDED Requirements

### Requirement: Agent State Synchronization
The system SHALL provide a dedicated networking event, `ENEMY_SYNC`, to transmit the current state (position, rotation, activity) of non-player agents from the Host to all clients.

#### Scenario: Real-time agent sync
- **WHEN** the Host's hunter agent moves during a match
- **THEN** it MUST emit an `ENEMY_SYNC` event at regular intervals to update all clients

### Requirement: Client-Side Movement Smoothing
Remote clients SHALL use interpolation to smoothly transition agents between received network positions, preventing jerky or "teleporting" movement.

#### Scenario: Smooth agent movement
- **WHEN** a client receives an `ENEMY_SYNC` update
- **THEN** it SHALL smoothly move the local proxy of the agent toward the new position using a Lerp-based approach
