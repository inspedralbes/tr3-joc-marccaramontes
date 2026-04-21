## Context

The current `GameManager` processes death by immediately transitioning to `GameState.GameOver` and showing the results panel. This happens in a single frame, which is jarring. The `survivalTime` is tracked but not persisted across sessions.

## Goals / Non-Goals

**Goals:**
- Implement a 1.5-second "Death Sequence" with reduced time scale.
- Persist high scores (Best Time) using `PlayerPrefs`.
- Automatically clear the scene of enemies and bullets after death.
- Update the results UI to show the current score vs. the personal best.

**Non-Goals:**
- Implementing a full online leaderboard (local persistence only).
- Creating complex death animations (visual effects only via time-scale and UI).
- Modifying the core game-loop or wave system.

## Decisions

### 1. Game State Transition
- **Decision**: Add a new `GameState.DeathTransition` value to the enum.
- **Rationale**: This allows us to disable player input and enemy AI while waiting for the transition timer to finish without being fully in the `GameOver` state yet.
- **Alternative**: Use a boolean `isTransitioning`. *Rejected* as it clutters the state machine logic.

### 2. Time Scale and Coroutines
- **Decision**: Use `Time.timeScale = 0.3f` during the transition and `WaitForSecondsRealtime` for the delay.
- **Rationale**: Simple to implement and provides immediate visual feedback that "something happened."
- **Alternative**: Animate individual objects. *Rejected* for being too complex and error-prone.

### 3. Persistence
- **Decision**: Use `PlayerPrefs` with a key `BestTime_Solo`.
- **Rationale**: Perfect for small, local data like high scores in a simple project.
- **Alternative**: JSON file. *Rejected* as it's overkill for a single float value.

### 4. Scene Cleanup
- **Decision**: `GameManager` will call a cleanup method that uses `GameObject.FindGameObjectsWithTag("Enemy")` and `"Bullet"` to destroy active entities.
- **Rationale**: Ensures the results panel is the only focus and prevents background noise.

## Risks / Trade-offs

- **[Risk]** NRE during cleanup if objects are already being destroyed. → **Mitigation**: Use `Destroy(obj)` which is safe to call on already flagged objects.
- **[Risk]** `Time.timeScale` affecting UI animations. → **Mitigation**: Ensure UI animations (if any) are set to "Unscaled Time".
- **[Risk]** Coroutine duplication if `ProcessDeath` is called twice. → **Mitigation**: Add a check for `currentState == GameState.DeathTransition` at the start of `ProcessDeath`.

## State Machine Diagram
```
[Playing] ───(ProcessDeath)──▶ [DeathTransition] ───(Wait 1.5s)──▶ [GameOver]
    ▲                                                                 │
    └───────────────────────────(Retry)───────────────────────────────┘
```
