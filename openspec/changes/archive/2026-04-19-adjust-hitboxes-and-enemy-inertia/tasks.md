## 1. Hitbox Standardization

- [x] 1.1 Update `Enemy.prefab`: Set `CircleCollider2D.radius` to 0.25.
- [x] 1.2 Update `SampleScene.unity`: Replace the Player's `BoxCollider2D` with a `CircleCollider2D` and set radius to 0.25.
- [x] 1.3 Update `PlayerMovement.cs`: Modify `DrawCircles()` to ensure it correctly fetches the `radius` from the `CircleCollider2D` for the line renderer.

## 2. Enemy Movement Refactor

- [x] 2.1 Update `Enemy.cs`: Add `currentDirection` (Vector3) and `turnSpeed` (float) fields.
- [x] 2.2 Update `Enemy.cs`: Replace the `Update()` movement logic with the new inertia-based rotation and Dot Product speed penalty.
- [x] 2.3 Update `Enemy.cs`: Ensure Z-axis remains locked to 0 during calculations.

## 3. Verification

- [x] 3.1 Verify Player and Enemy hitboxes appear at the correct size (0.5 diameter).
- [x] 3.2 Test enemy movement: confirm they move fast in straight lines and slow down noticeably when turning.
- [x] 3.3 Confirm that player death still triggers correctly upon contact with the new hitbox size.
