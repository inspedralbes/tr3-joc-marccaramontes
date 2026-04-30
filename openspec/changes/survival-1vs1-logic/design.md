## Context

The current `GameManager` logic is designed for a single-player experience or a shared-end multiplayer where anyone's death ends the game. To achieve a 1vs1 competitive mode, we need to track individual death states and times, and only trigger the game-over transition when all participants have reached the end state.

## Goals / Non-Goals

**Goals:**
- Decouple local player death from the global `GameState`.
- Implement a spectator mode that switches the camera to the surviving player.
- Extend the `ResultsUIRegisterer` to handle dual-player statistics.
- Sync death times through the `NetworkManager`.

**Non-Goals:**
- Supporting more than 2 players in this phase.
- Implementing a replay system.
- Changing the AI behavior (Hunters will naturally switch targets when one player is removed).

## Decisions

### Decision: State Tracking in GameManager
We will add a `Dictionary<string, float> playerDeathTimes` to `GameManager` to track who has died and when.

**Rationale:**
By tracking death times by PlayerId, we can easily compare them at the end of the match and know if the game should continue or end.

### Decision: Virtual Spectator State
We will introduce a `Spectator` state in `PlayerMovement` (or a flag) that disables input and visuals but keeps the object alive or transitions the camera focus.

**Rationale:**
Completely destroying the player object might break camera references. Keeping a "ghost" or simply reparenting the camera to the rival's `Ghost` object is more stable.

### Decision: Comparative UI in ResultsUIRegisterer
The `ResultsUIRegisterer` will be updated to accept a `MatchResults` object containing both player names and times.

**Rationale:**
The current UI only shows one time. We need to duplicate or extend the time display fields to show the "Versus" comparison.

## Risks / Trade-offs

- **Risk**: Desync between the two players' clocks.
- **Mitigation**: Use the server-sent `DEATH` event time as the "official" survival time for the rival.

## UI Flow State Machine

```
[Playing] 
    |
    |-- (Local Player Dies) --> [Local Spectating] (HUD: "Rival is still alive...")
    |                               |
    |-- (Rival Dies) -------------- | -- (Rival Dies) --> [Game Over Results]
```

## Hierarchical Changes
- **CanvasResultados**: Add a new text field for `RivalTimeText`.
- **HUD**: Add a small overlay for `RivalStatus`.
