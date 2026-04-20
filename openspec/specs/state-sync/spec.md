## ADDED Requirements

### Requirement: Player Position Sync
The system SHALL synchronize the position of all players in a room in real-time.

#### Scenario: Update Remote Ghost
- **WHEN** a local player moves
- **THEN** the client MUST emit its new position to the server, which SHALL relay it to all other clients to update the local representation (ghost) of that player

### Requirement: Enemy Spawn Sync
The Host client SHALL be the only authority for spawning enemies.

#### Scenario: Host Spawns Enemy
- **WHEN** the `EnemySpawner` on the Host triggers a spawn
- **THEN** the Host MUST send an "enemy_spawn" event with position and ID to the server, and all other clients SHALL instantiate that enemy at the specified position
