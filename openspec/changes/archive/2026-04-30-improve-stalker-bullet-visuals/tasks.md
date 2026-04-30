## 1. Asset Preparation

- [x] 1.1 Update `Assets/EnemyBulletMaterial.mat` defaults to use a higher intensity for `_OutlineColor` (fallback).

## 2. Script Implementation

- [x] 2.1 Modify `Assets/EnemyBullet.cs` to include a method for initializing visual properties.
- [x] 2.2 In `EnemyBullet.cs`, implement material instantiation using the `Custom/SpriteOutline` shader.
- [x] 2.3 In `EnemyBullet.cs`, set the `_OutlineColor` with an HDR intensity (x15) and a default width (e.g., 2.5).

## 3. Integration and Verification

- [x] 3.1 Ensure `Enemy.cs` correctly spawns bullets and that they initialize their visuals upon creation.
- [x] 3.2 Verify in-game that Stalker bullets now have a visible glow/bloom effect consistent with player bullets.
- [x] 3.3 Confirm that bullet destruction and collision still work as expected.
