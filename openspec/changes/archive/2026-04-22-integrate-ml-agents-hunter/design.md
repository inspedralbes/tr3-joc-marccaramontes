## Context

The current `HunterAgent` in Unity is a basic ML-Agents implementation that pursues a single, predefined `Transform` target. In a multiplayer environment, this target needs to be dynamic to account for multiple players. Furthermore, the synchronization logic currently uses the `SPAWN_ENEMY` event for position updates, which is semantically incorrect and lacks the interpolation needed for smooth movement on remote clients.

## Goals / Non-Goals

**Goals:**
- Make the hunter agent aware of all active players and pursue the nearest one.
- Standardize agent networking with a dedicated `ENEMY_SYNC` event.
- Implement smooth movement (Lerp) for clients receiving agent updates.
- Ensure the agent's AI logic only runs on the Host.

**Non-Goals:**
- Re-training the agent's neural network (using existing training logic).
- Adding complex pathfinding (sticking to ML-Agents' direct movement).
- Implementing multiple hunter agents in one match (limited to one for simplicity).

## Decisions

### Decision 1: Dynamic Target Acquisition via GameManager
**Rationale:** The `HunterAgent` will query the `GameManager` or a `PlayerRegistry` to find all active player transforms. It will calculate the squared distance to each and select the minimum to avoid expensive square root operations during target selection.
**Alternatives:** Passing targets from the Host to the agent via network, but local lookup is faster and more reliable.

### Decision 2: Dedicated `ENEMY_SYNC` Event
**Rationale:** Reusing `SPAWN_ENEMY` for position updates is misleading. A separate `ENEMY_SYNC` event allows for more precise packet structure (ID, position, activity state) and clearly distinguishes initial creation from ongoing state synchronization.

### Decision 3: Lerp-based Interpolation on Clients
**Rationale:** Directly setting `transform.position` results in jerky movement if packets are delayed. Using `Vector3.Lerp` to move toward the "target network position" provides a much smoother visual experience.
**Alternatives:** RigidBody-based synchronization, but Lerp is more predictable and lightweight for this simple agent.

## Risks / Trade-offs

- **[Risk]** Agent stuttering if sync interval is too high.
  - **Mitigation:** Allow for a configurable sync interval (e.g., 0.05s - 0.1s) and use a smoothing factor in the Lerp calculation.
- **[Risk]** Host CPU overhead from running ML-Agents logic.
  - **Mitigation:** Keep the agent's observation space and neural network architecture minimal.

## Hierarchies & Prefabs

- **HunterAgent Prefab**:
  - `HunterAgent` (IA logic)
  - `HunterAgentNetworkSync` (Network logic)
  - `Rigidbody2D` (Physics, set to Kinematic on clients)
  - `CircleCollider2D` (Trigger for collision/rewards)
