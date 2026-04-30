## Context

The game uses URP (Universal Render Pipeline) with a Bloom post-processing effect. Elements that should appear "neon" or "fluorescent" must use HDR (High Dynamic Range) colors that exceed the Bloom threshold (currently 0.9). Currently, `EnemyBulletMaterial.mat` uses standard colors, and `EnemyBullet.cs` does not initialize the material with HDR values, unlike other projectiles in the game.

## Goals / Non-Goals

**Goals:**
- Make Stalker bullets visually consistent with the neon aesthetic.
- Ensure the "threat" level of enemy projectiles is clearly signaled via high-intensity glow.
- Reuse the existing `Custom/SpriteOutline` shader.

**Non-Goals:**
- Changing the movement or collision logic of the bullets.
- Modifying other enemy types not related to the Stalker's projectiles.

## Decisions

- **Decision: Dynamic Material Instance**: `EnemyBullet.cs` will be updated to handle material initialization. While a global material change is possible, initializing the material in code ensures it uses the `Custom/SpriteOutline` shader and applies the correct HDR intensity regardless of prefab settings.
  - *Rationale*: Consistency with how player bullets and enemy bodies are handled in `PlayerShooting.cs` and `Enemy.cs`.
- **Decision: HDR Multiplication in Code**: The intensity will be controlled via code (e.g., `Color * 15f`) to ensure it consistently hits the Bloom threshold.
  - *Rationale*: It's easier to maintain and tune across different prefabs than manually setting HDR values in the Unity Inspector for every material.
- **Decision: Update Material Asset Defaults**: Update `EnemyBulletMaterial.mat` defaults to use higher intensity as a fallback.
  - *Rationale*: Better visualization in the Editor/Scene view without running the game.

## Risks / Trade-offs

- **[Risk] Performance from Material Instances** → Creating a new `Material` instance per bullet can increase draw calls (breaking batching).
  - *Mitigation*: Since the number of active bullets is managed and relatively low, the impact is minimal. If it becomes an issue, we can use `MaterialPropertyBlock`.
- **[Risk] Visual Noise** → Too much glow can make the screen chaotic.
  - *Mitigation*: We will match the intensity (x15) of existing projectiles to maintain balance.
