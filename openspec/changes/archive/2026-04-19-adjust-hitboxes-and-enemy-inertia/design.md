## Context

Character hitboxes are currently inconsistent (Box vs Circle) and movement is perfectly linear. This lack of weight makes the game feel static and reduces the skill gap for dodging.

## Goals / Non-Goals

**Goals:**
- Standardize all character hitboxes to a 0.5 diameter circle.
- Implement physics-inspired "weight" for enemies through inertia.
- Ensure the Player script correctly visualizes the new hitbox size.

**Non-Goals:**
- Implementing a full physics-based movement (using Rigidbody forces) for enemies; we will maintain transform-based movement for precise control but simulate inertia.

## Decisions

- **Inertia Implementation**: 
  - Add `currentDirection` and `turnSpeed` to `Enemy.cs`.
  - Use `Vector3.Slerp` or `Vector3.RotateTowards` to transition from current to target direction.
  - Speed penalty: The final speed will be `baseSpeed * Mathf.Max(0.2f, Vector3.Dot(currentDirection, targetDirection))`. This ensures they slow down up to 80% when forced to make a U-turn.
- **Hitbox Standardization**:
  - Both Player and Enemy will use `CircleCollider2D` with `radius = 0.25`.
  - The `PlayerMovement.cs` gizmo/line renderer logic will be updated to fetch this radius automatically.

## Risks / Trade-offs

- **Risk**: Enemies might feel "too slippery" if the `turnSpeed` is too low.
  - **Mitigation**: Expose `turnSpeed` as a public variable in the Inspector for easy balancing.
- **Risk**: Very fast players might easily "break" the enemy AI by circling them.
  - **Mitigation**: The `turnSpeed` should be high enough to catch a player moving at normal speeds but low enough to allow dodge windows.
