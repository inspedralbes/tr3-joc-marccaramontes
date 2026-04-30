## Why

The enemy bullets (specifically from Stalkers) currently lack the high-intensity neon/fluorescent visual style present in other game elements like the player, enemies, and player bullets. While the existing specification requires an "intense neon orange" color, the current implementation uses a standard orange that does not trigger the Bloom post-processing effect, making the projectiles look flat and less threatening. This change will bring the implementation in line with the visual standards of the game.

## What Changes

- **Modified**: `EnemyBulletMaterial.mat` will be updated to use HDR colors for the outline to trigger the Bloom post-processing effect.
- **Modified**: `EnemyBullet.cs` will be updated to handle dynamic material initialization to ensure the neon effect is applied correctly, similar to how player bullets are handled.
- **Modified**: `Enemy.cs` will be reviewed to ensure the Stalker-type enemies instantiate the bullets with the correct initial parameters.

## Capabilities

### New Capabilities
<!-- None -->

### Modified Capabilities
- `enemy-projectiles`: Clarify and enforce the HDR/Neon intensity requirements for projectiles to ensure they consistently trigger post-processing effects.

## Impact

- **Assets**: `Assets/EnemyBulletMaterial.mat` will be modified to use HDR colors.
- **Scripts**: `Assets/EnemyBullet.cs` (initialization), `Assets/Enemy.cs` (spawning logic).
- **Gameplay**: Significantly improved visibility and "danger" signaling for enemy projectiles.
