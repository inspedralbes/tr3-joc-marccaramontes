## Why

Currently, hitboxes for both the player and enemies are inconsistent and potentially too large for a precise "survivor" style gameplay experience. Additionally, enemy movement is basic and perfectly linear, making them too predictable and hard to dodge effectively. Introducing inertia will make them feel more "alive" and reward skillful player positioning.

## What Changes

- **Hitbox Standardization**: Set all character hitboxes (Player and Enemy) to a diameter of 0.5 units (radius 0.25).
- **Enemy Inertia Logic**: Implement a more advanced movement system for enemies where they accelerate in straight lines but lose speed while turning.
- **Player Component Sync**: Replace the Player's `BoxCollider2D` with a `CircleCollider2D` to match the Enemy's physical representation.

## Capabilities

### New Capabilities
<!-- None -->

### Modified Capabilities
- `enemy-behavior`: Update the movement requirements to include inertia and turning penalties.
- `player-positioning`: Update the hitbox requirement to standardize on a 0.5 diameter circle.

## Impact

- **Assets**: `Assets/Enemy.prefab` (updated collider radius).
- **Scenes**: `Assets/Scenes/SampleScene.unity` (updated Player collider type and size).
- **Scripts**: `Assets/Enemy.cs` (new movement logic), `Assets/PlayerMovement.cs` (visual alignment with new collider size).
