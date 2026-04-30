## ADDED Requirements

### Requirement: Automated Transform Synchronization
The system SHALL use the `NetworkTransform` component to synchronize position and rotation of players and enemies. The synchronization MUST include interpolation to ensure smooth visual movement on remote clients.

#### Scenario: Smooth Player Movement
- **WHEN** a local player moves their character
- **THEN** the `NetworkTransform` SHALL automatically transmit the update to other clients, who will interpolate the ghost's position to match the owner's within the configured threshold

### Requirement: Networked Actions via RPCs
Gameplay actions that affect other players or game state (e.g., Shooting, Damage) MUST be implemented using **ServerRpc**. The server/host executes the logic and uses **ClientRpc** or state synchronization to notify others.

#### Scenario: Networked Shooting
- **WHEN** a player presses the attack button
- **THEN** they SHALL call a `ShootServerRpc()`
- **THEN** the Host SHALL instantiate the networked bullet prefab and spawn it across all clients

### Requirement: Shared Game State Synchronization
The system SHALL use `NetworkVariable<T>` to synchronize persistent game values that are managed by the Host. This includes the match timer, wave count, and global difficulty multiplier.

#### Scenario: Global Timer Sync
- **WHEN** the match starts and the Host increments the survival timer
- **THEN** all clients MUST see the updated timer value reflected in their UI via the `OnValueChanged` callback of the `NetworkVariable`
