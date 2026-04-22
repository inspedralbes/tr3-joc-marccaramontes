## Context

The `SampleScene` currently contains a duplicated `TimerHUD` GameObject at the root of `CanvasResultados` which remains at 0.00s throughout gameplay, creating confusion. Simultaneously, the `GameManager` and `ResultsUIRegisterer` are coupled with obsolete record-tracking logic (`bestTime`) and UI elements (`BestTimeHUD`, `NewRecordBadge`) that are no longer requested by the project scope. 

Crucially, the Results Panel often displays "0" for kills and survival time due to broken hardcoded references in the scene's `GameManager` instance and a sequential animation logic that can fail or appear "stuck" if not properly initialized.

## Goals / Non-Goals

**Goals:**
- **UI Decoupling**: Ensure only the `HUDGroup/TimerHUD` is active and updated.
- **Logic Simplification**: Remove all local high-score persistence and its associated UI.
- **Data Integrity**: Guarantee that match stats (Kills and Time) are correctly displayed every time.
- **Visual Impact**: Introduce a clear "PARTIDA FINALIZADA" title and improve the Game Over transition.

**Non-Goals:**
- Implementing an online leaderboard (out of scope).
- Adding complex particle effects or shaders for the UI (keeping it lean/performant).

## Decisions

### 1. Hierarchy Pruning (UI Cleanup)
- **Decision**: Manually delete the root `TimerHUD` in `SampleScene.unity`.
- **Rationale**: It is redundant and unassigned. The one inside `HUDGroup` is correctly registered and updated by the `GameManager`.
- **Alternatives**: Keeping it and updating both (creates clutter and consumes unnecessary draw calls).

### 2. Logic & Persistence Pruning
- **Decision**: Remove `bestTime`, `isNewRecord`, `LoadHighScore()`, and `SaveHighScore()` from `GameManager.cs`.
- **Rationale**: The project no longer requires local records. Removing this simplifies the `GameManager` and eliminates redundant `PlayerPrefs` calls.
- **Alternatives**: Just hiding the UI but keeping the logic (leads to "dead code" and potential bugs).

### 3. Immediate Stat Assignment
- **Decision**: Modify `ShowResultsSequence` to set `killsText.text` and `p1TimeText.text` immediately before starting the `CountText` animation.
- **Rationale**: If the coroutine is delayed or the `UIAnimationManager` is missing, the player will still see the final values instead of a default "0". 
- **Alternatives**: Relying solely on the `CountText` coroutine (risky if the game state freezes or frames drop).

### 4. Results UI Modernization
- **Decision**: Add a "WinnerText" or "GameOverTitle" to the `PanelResultados` that defaults to "PARTIDA FINALIZADA" for Solo mode.
- **Rationale**: The panel currently feels like a simple "list" rather than a definitive game-over screen.
- **Implementation**: The `ResultsUIRegisterer` will be updated to include a `titleText` reference.

## Risks / Trade-offs

- **[Risk] Broken Unity References** → **Mitigation**: Update `GameManager.FindResultsPanelExhaustive()` to include the new title text and remove search logic for the deleted record elements.
- **[Risk] Scene Sync** → **Mitigation**: Since `SampleScene` is modified directly, ensure the `SceneSetupHelper` is run to "heal" any missing links for the new Title object.
