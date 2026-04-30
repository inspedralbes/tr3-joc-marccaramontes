## Why

Currently, enemies spawn but remain static outside the view area, and the initial enemy in the scene does not move. This is caused by the `Enemy.cs` script not being attached to the `Enemy.prefab` and the `EnemySpawner` incorrectly referencing a scene instance instead of the prefab asset.

## What Changes

- **Attach Script**: Assign the `Enemy.cs` component to the `Enemy.prefab` asset.
- **Fix Spawner Reference**: Update the `EnemySpawner` component in `SampleScene.unity` to use the `Enemy.prefab` asset from the Project folder.
- **Scene Cleanup**: Remove the static "Enemy" object from the Hierarchy in `SampleScene.unity` to ensure all enemies are managed by the spawner.

## Capabilities

### New Capabilities
- `enemy-behavior`: Defines the logic for spawning enemies at a specific distance and their movement towards the player's position.

### Modified Capabilities
<!-- No requirement changes for existing player capabilities. -->

## Impact

- **Assets**: `Assets/Enemy.prefab` (modified to include `Enemy.cs`).
- **Scenes**: `Assets/Scenes/SampleScene.unity` (updated `EnemySpawner` reference and removed static enemy).
- **Scripts**: `Assets/EnemySpawner.cs` (logic remains the same, but its configuration in the scene is corrected).
