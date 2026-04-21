## Context

The current Game Over flow relies on a "registration" pattern where the UI informs the `GameManager` of its presence. If this registration fails or is delayed (common in Unity 6 due to scene loading order), the `GameManager` remains in a state where it has no references to display the results. Additionally, if the `UIAnimationManager` is missing, the coroutines in `GameManager` may hang or crash, preventing the UI from ever being shown.

## Goals / Non-Goals

**Goals:**
- Ensure the Game Over panel appears even if registration failed.
- Prevent coroutine halts when the `UIAnimationManager` singleton is null.
- Accelerate UI registration by moving it to the earliest possible lifecycle hook (`Awake`).

**Non-Goals:**
- Redesigning the visual look of the Game Over screen.
- Adding new animations or transitions.

## Decisions

### 1. Self-Healing Discovery Logic
- **Decision**: Implement a fallback search in `GameManager.ShowResults` using `Resources.FindObjectsOfTypeAll<GameObject>()`.
- **Rationale**: This is a heavy operation but only runs once upon death. It guarantees that the UI is found even if it was disabled or not yet registered.

### 2. Awake-time Registration
- **Decision**: Move `ResultsUIRegisterer.TryRegister` from `Start` to `Awake`.
- **Rationale**: Ensures the `GameManager` knows about the UI before the first frame of gameplay, reducing the race condition window.

### 3. Non-blocking UI Calls
- **Decision**: Update `GameManager` and `UIAnimationManager` to check for `Instance != null` and proceed with instant state changes (e.g., `SetActive(true)`, `alpha = 1`) if the animator is missing.
- **Rationale**: Visual polish (fades) should never compromise functional reliability (showing the score).

## Risks / Trade-offs

- **[Risk]** Heavy performance hit during `Resources.FindObjectsOfTypeAll`. → **Mitigation**: Only execute as a last-resort fallback in `ShowResults`.
- **[Risk]** `Awake` order between `GameManager` and `ResultsUIRegisterer` is non-deterministic. → **Mitigation**: Use a retry loop or check `GameManager.Instance` with a fallback inside the registration logic.

## UI Flow State Machine
```
[Death Event] ──▶ [Sequence Start]
                      │
                      ▼
             [Discovery Phase] ──(Missing Refs?)──▶ [Resources Search]
                      │                                    │
                      ▼                                    ▼
             [Animation Phase] ──(No Animator?)──▶ [Instant Show]
                      │                                    │
                      ▼                                    ▼
             [Ready for Input] ◀───────────────────────────┘
```
