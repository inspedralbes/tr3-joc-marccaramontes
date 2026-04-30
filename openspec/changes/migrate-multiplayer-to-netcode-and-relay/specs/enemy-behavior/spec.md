## MODIFIED Requirements

### Requirement: Enemy Spawning
The system SHALL spawn enemies only on the Host instance. The Host is responsible for managing wave timers and deciding where and when to instantiate enemies. Once instantiated, the Host MUST use `NetworkObject.Spawn()` to replicate the enemy to all clients.

#### Scenario: Enemy Instantiation
- **WHEN** the `spawnRate` timer elapses in the `EnemySpawner` on the Host
- **THEN** a new instance of the networked `Enemy.prefab` is created and spawned across the network
