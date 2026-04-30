## Context

The project currently relies on a custom WebSocket-based networking layer and a Node.js relay server. This setup requires manual synchronization of every game event (movement, shooting, spawning) via JSON packets. While functional, it lacks standard features like interpolation, automatic state synchronization, and NAT traversal. **Unity Netcode for GameObjects (NGO)** will modernize this by providing a component-based synchronization model and integrating with **Unity Relay**.

## Goals / Non-Goals

**Goals:**
- Transition from manual WebSocket relay to **Host-Authoritative** state synchronization.
- Implement smooth movement interpolation using `NetworkTransform`.
- Use `NetworkVariable` for global game state (e.g., survival time, wave status).
- Enable internet play via **Unity Relay** (Join Codes).
- Maintain compatibility with the existing Node.js `api-service` for result persistence.

**Non-Goals:**
- Implementation of Dedicated Server (Headless) mode.
- Advanced lag compensation (backtracking) for projectiles.
- Full rewrite of the 2D physics engine.

## Decisions

### 1. Host-Authoritative Architecture
**Rationale:** We will use the "Host" model where one player acts as both a server and a client. This is ideal for 1v1 or small-group survival games as it simplifies authority management and doesn't require dedicated server hosting.
**Alternatives:** *Client-Authoritative* (leads to extreme cheating and desync) or *Dedicated Server* (higher infrastructure cost/complexity).

### 2. Synchronization Strategy
**Rationale:**
- **Movement:** Use `NetworkTransform` for players and enemies. This component handles interpolation (smoothing) and bandwidth management automatically.
- **Actions:** Use `[ServerRpc]` for shooting and spawning. The Host validates the action and performs the instantiation.
- **State:** Use `NetworkVariable<T>` for shared data like the game timer and difficulty multiplier.
**Alternatives:** Manual messaging via `CustomMessagingManager` (too much boilerplate).

### 3. Unity Relay for Connectivity
**Rationale:** Unity Relay provides a STUN/TURN-like service that allows clients to connect behind NATs without port forwarding. It is the modern standard for Unity P2P games.
**Alternatives:** Manual IP entry (cumbersome for users) or custom UDP hole punching.

### 4. Prefab Hierarchy
Each networked entity requires a `NetworkObject` component.
- **Player Prefab:**
  - `NetworkObject`
  - `NetworkTransform` (Sync Position & Rotation)
  - `PlayerMovement` (Inherits from `NetworkBehaviour`)
- **Enemy Prefabs:**
  - `NetworkObject`
  - `NetworkTransform`
  - `Enemy` (Inherits from `NetworkBehaviour`, Host-only AI logic)

## Risks / Trade-offs

- **[Risk] Host Advantage** → Since the Host is a client, they have 0ms latency. Mitigation: For a cooperative/survival game, this is less critical than in a competitive FPS.
- **[Risk] Migration Complexity** → Removing the custom socket layer might break existing UI flows. Mitigation: We will refactor `NetworkManager` to maintain the same public API (Events) where possible to minimize UI changes.
- **[Risk] Bandwidth** → NGO can be chatty. Mitigation: Tune `NetworkTransform` send rates and use `NetworkVariable` only for data that changes infrequently.
