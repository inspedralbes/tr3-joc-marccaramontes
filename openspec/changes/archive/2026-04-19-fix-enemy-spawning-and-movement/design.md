## Context

Enemies currently fail to move and spawn correctly because the `Enemy.cs` script is not assigned to the `Enemy.prefab` asset. Additionally, the `EnemySpawner` in the scene is configured to clone an existing (and non-functional) scene instance instead of the base prefab asset.

## Goals / Non-Goals

**Goals:**
- Ensure all spawned enemies have the `Enemy` component and can move towards the player.
- Ensure the `EnemySpawner` creates enemies from the correct prefab asset.
- Remove redundant or static enemy objects from the scene hierarchy.

**Non-Goals:**
- Refactoring the movement algorithm or adding obstacle avoidance.
- Implementing an object pooling system for enemies at this stage.

## Decisions

- **Prefab Component Attachment**: The `Enemy` component will be added to the `Enemy.prefab`. This ensures every instance created by the spawner has the necessary logic.
- **Asset-Based Spawning**: The `EnemySpawner` in `SampleScene.unity` will be updated to reference the `Enemy.prefab` asset GUID rather than a scene instance ID. This is a best practice to ensure consistent behavior.
- **Default Speed**: The `speed` variable in the `Enemy.prefab` will be set to a default value (3.0) to ensure immediate visible movement upon fix.

## Risks / Trade-offs

- **Risk**: Setting the prefab speed might overwrite scene-specific variations if they existed. 
  - **Mitigation**: We confirmed the current scene instances are not moving, so resetting to a functional default is the safest path.
- **Risk**: Removing the manual enemy might make it harder to test a single enemy in isolation.
  - **Mitigation**: Isolation testing can be done by adjusting the spawner's `spawnRate` or by temporarily dragging the prefab into the scene during development.
