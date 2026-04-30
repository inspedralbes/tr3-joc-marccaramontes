## Context

The current `GameManager` implements a state machine with `Menu`, `Playing`, and `GameOver` states. However, in Solo mode, the `ProcessDeath` method triggers an immediate scene reload, bypassing the `GameOver` state and the `PanelResultados` UI. There is also no real-time display of the `survivalTime` to the player during gameplay.

## Goals / Non-Goals

**Goals:**
- Implement a real-time HUD timer that updates every frame while `GameState == Playing`.
- Unify the death flow so Solo mode also displays the results panel.
- Add functional "Retry" and "Main Menu" buttons to the results panel.
- Extend the `ResultsUIRegisterer` to automate the linkage of these new UI elements.

**Non-Goals:**
- Redesigning the game's visual aesthetic.
- Adding high-score persistence (local or cloud) in this task.
- Changing the core gameplay mechanics (movement, spawning).

## Decisions

### 1. Unified Death Flow in GameManager
**Rationale**: By treating Solo mode similarly to the end of a Multiplayer session, we reduce code duplication and ensure a consistent player experience.
**Alternative**: Creating a separate `SoloGameOverPanel`. *Rejected* as it adds unnecessary complexity and asset overhead.

### 2. HUD Timer via Update Loop
**Rationale**: Updating a TMP text field in `GameManager.Update` while in the `Playing` state is the simplest way to provide real-time feedback.
**Alternative**: Using a coroutine or an event-based system. *Rejected* as the `GameManager` already tracks `survivalTime` in `Update`, so adding a UI update there is trivial and performant enough for this scale.

### 3. Button Linkage via GameManager Methods
**Rationale**: The `GameManager` is a persistent Singleton. Linkage via the `ResultsUIRegisterer` ensures that even when the scene reloads, the buttons can find the `GameManager` instance.
**Alternative**: Hard-coding listeners in `ResultsUIRegisterer`. *Rejected* as it keeps logic scattered; the `GameManager` should remain the central authority for flow.

## Risks / Trade-offs

- **[Risk]**: Buttons might lose their `OnClick` listeners if the scene reloads and they are not properly registered. → **Mitigation**: `ResultsUIRegisterer` will manually add listeners to the buttons during its `Start` or `TryRegister` phase to ensure they always point to the current `GameManager` instance.
- **[Risk]**: HUD Timer text might be null in some scenes (like Menu). → **Mitigation**: Use a null check before updating the text in `GameManager.Update`.

## UI Hierarchy & Flow

### Updated Results Panel Prefab
- **PanelResultados** (with `ResultsUIRegisterer`)
    - **P1TimeText** (Existing)
    - **P2TimeText** (Existing)
    - **WinnerText** (Existing)
    - **RetryButton** (NEW)
    - **MenuButton** (NEW)

### HUD Timer
- **HUDCanvas**
    - **TimerText** (Registered via `ResultsUIRegisterer` or a separate `HUDRegisterer`)
