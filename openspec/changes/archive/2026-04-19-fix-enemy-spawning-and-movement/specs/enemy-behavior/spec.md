## ADDED Requirements

### Requirement: Enemy Spawning
The system SHALL spawn enemies at random positions within a configurable radial range (min/max radius) centered on the player or the designated platform center.

#### Scenario: Enemy Instantiation
- **WHEN** the `spawnRate` timer elapses in the `EnemySpawner`
- **THEN** a new instance of the `Enemy.prefab` is created at a random angle and distance between `minSpawnRadius` and `maxSpawnRadius` from the `spawnCenter`

### Requirement: Enemy Movement
Active enemies SHALL constantly move towards the current position of the player object.

#### Scenario: Follow Player
- **WHEN** the `Enemy` script is active and a player object is found
- **THEN** the enemy updates its position every frame to move towards the player at the defined `speed`

### Requirement: Player Collision
The system SHALL detect when an enemy makes contact with the player and trigger the player's death sequence.

#### Scenario: Catch Player
- **WHEN** an enemy's trigger collider enters the player's collider
- **THEN** the `Die()` method in the `PlayerMovement` component is called, ending the game session
