## ADDED Requirements

### Requirement: Enemy Spawning
The system SHALL spawn enemies at random positions within a configurable radial range (min/max radius) centered on the player or the designated platform center.

#### Scenario: Enemy Instantiation
- **WHEN** the `spawnRate` timer elapses in the `EnemySpawner`
- **THEN** a new instance of the `Enemy.prefab` is created at a random angle and distance between `minSpawnRadius` and `maxSpawnRadius` from the `spawnCenter`

### Requirement: Enemy Movement
Active enemies SHALL move towards the player with inertia and turning penalties. They SHALL accelerate when moving in a straight line and decelerate when forced to make sharp turns.

#### Scenario: Follow Player with Inertia
- **WHEN** the `Enemy` script is active and a player object is found
- **THEN** the enemy adjusts its direction towards the player gradually based on its `turnSpeed`
- **THEN** its current velocity increases when the angle to the player is small, and decreases when the angle is large

### Requirement: Player Collision
The system SHALL detect when an enemy makes contact with the player and trigger the player's death sequence.

#### Scenario: Catch Player
- **WHEN** an enemy's trigger collider enters the player's collider
- **THEN** the `Die()` method in the `PlayerMovement` component is called, ending the game session

### Requirement: Reacción al impacto de bala
El enemigo DEBE ser destruido inmediatamente al entrar en contacto con un proyectil disparado por el jugador.

#### Scenario: Muerte por disparo
- **WHEN** un objeto con el script Bullet colisiona con el enemigo
- **THEN** el enemigo se destruye instantáneamente
