## Context

The current networking implementation in Unity uses persistent Singletons (`NetworkManager`, `GameManager`) that survive scene transitions. However, scene-specific objects like `RemotePlayerManager` and `PlayerShooting` (on "ghost" prefabs) subscribe to `NetworkManager` events during `Start()` but never unsubscribe. This leads to `MissingReferenceException` when those objects are destroyed by a scene change, as the persistent `NetworkManager` still attempts to call their methods.

## Goals / Non-Goals

**Goals:**
- Eliminate `MissingReferenceException` during scene transitions.
- Ensure remote player visuals are destroyed when they disconnect.
- Synchronize room membership state with the server when returning to the menu.
- Fix the event naming mismatch (`player_shoot` vs `player_shot`).

**Non-Goals:**
- Implementing a full state-sync system (e.g., authoritative server).
- Adding new gameplay features.

## Decisions

### 1. Unified Event Lifecycle
We will use the `OnEnable`/`OnDisable` pattern for all network event subscriptions. This ensures that even if an object is disabled or the scene is changed, the `NetworkManager` stops attempting to notify destroyed instances.

**Rationale**: This is the standard Unity practice for preventing memory leaks and reference errors with persistent events.

### 2. Remote Player Removal
`NetworkManager` will be updated to listen for the `player_left` socket event. It will fire a new `OnRemotePlayerLeft` C# event. `RemotePlayerManager` will subscribe to this and destroy the corresponding GameObject in its dictionary.

**Rationale**: Prevents "ghost" players from remaining in the scene after they have disconnected.

### 3. Graceful Scene Transition
`GameManager.ReturnToMenu()` will be modified to check if `NetworkManager` is in a room and call a new `LeaveRoom()` method before loading the Menu scene.

**Rationale**: Ensures the server knows the player has left the room immediately, rather than waiting for a socket timeout.

### 4. Communication Sequence Diagram

```
[Unity Client]                     [Server]                      [Peer Client]
      |                               |                                |
      |--- player_shoot ------------->|                                |
      |                               |--- player_shot --------------->|
      |                               |                                |
      |--- (Disconnect/Menu) -------->|                                |
      |                               |--- player_left --------------->|
      |                               |                                |
```

## Risks / Trade-offs

- **[Risk]** → Late event arrival during scene load.
- **[Mitigation]** → `NetworkManager` should check for null listeners and handle missing room data gracefully.
- **[Trade-off]** → Using `OnDisable` for unregistration means if an object is simply deactivated (not destroyed), it stops receiving network updates. This is intended behavior for our current architecture.
