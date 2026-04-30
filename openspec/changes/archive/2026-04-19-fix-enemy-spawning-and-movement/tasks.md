## 1. Prefab Update

- [x] 1.1 Add the `Enemy` script component to `Enemy.prefab`.
- [x] 1.2 Set the default `speed` value to 3.0 in the `Enemy` component within the prefab.

## 2. Scene Configuration

- [x] 2.1 Update the `EnemySpawner` in `SampleScene.unity` to reference the `Enemy.prefab` asset (GUID `b60710603916f974193cdac1a58b2dd6`) instead of a scene instance.
- [x] 2.2 Delete the manually placed "Enemy" GameObject from the Hierarchy in `SampleScene.unity`.

## 3. Verification

- [x] 3.1 Verify that enemies spawn correctly at the configured intervals.
- [x] 3.2 Confirm that spawned enemies move towards the player's position.
- [x] 3.3 Ensure that colliding with an enemy triggers the `Die()` method and restarts the scene.
