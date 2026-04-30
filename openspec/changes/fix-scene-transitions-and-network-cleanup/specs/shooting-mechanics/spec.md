## ADDED Requirements

### Requirement: Remote Bullet Spawning Sync
The `RemotePlayerManager` or `PlayerShooting` components MUST correctly spawn bullets for other players based on network events.

#### Scenario: Receiving Shot Event
- **WHEN** the `OnRemotePlayerShot` event is fired by `NetworkManager`
- **THEN** the local "ghost" corresponding to the `playerId` MUST instantiate a `bulletPrefab` at the specified position and rotation.
