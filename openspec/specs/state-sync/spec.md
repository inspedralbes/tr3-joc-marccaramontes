## ADDED Requirements

### Requirement: Player Position Sync
The system SHALL synchronize the position of all players in a room in real-time using the `MOVE` event type.

#### Scenario: Update Remote Ghost
- **WHEN** a local player moves
- **THEN** the client MUST emit its new position with the type `MOVE` to the server, and the server SHALL relay it with the sender's `playerId` to all other clients to update their local ghosts

### Requirement: Enemy Spawn Sync
The Host client SHALL be the only authority for spawning enemies.

#### Scenario: Host Spawns Enemy
- **WHEN** the `EnemySpawner` on the Host triggers a spawn
- **THEN** the Host MUST send an "enemy_spawn" event with position and ID to the server, and all other clients SHALL instantiate that enemy at the specified position
