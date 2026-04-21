## Context

The current `GameManager` processes death by immediately transitioning to `GameState.GameOver` and showing the results panel. This happens in a single frame, which is jarring and lacks the "impact" required for a high-intensity 2D shooter. We are moving towards a "Devil Daggers" inspired approach. Additionally, the project is migrating to **Unity 6 (6000+)**, which strictly enforces the **New Input System**, requiring the removal of all legacy `UnityEngine.Input` calls to prevent runtime exceptions.

## Goals / Non-Goals

**Goals:**
- Implement a 0.1s screen flash and immediate `Time.timeScale = 0` on death.
- Create a particle-based fragmentation effect for the player.
- Implement a `UIAnimationManager` for Fade-In and Numeric Tickers.
- **Unity 6 Migration**: Fully transition `PlayerMovement` and `PlayerShooting` to the New Input System (no legacy fallbacks).
- **Modern EventSystem**: Configure the `EventSystem` to use `InputSystemUIInputModule`.
- Persist high scores using `PlayerPrefs`.

**Non-Goals:**
- Implementing online leaderboards.
- Modifying enemy AI or gameplay balance.
- Re-implementing the Input System from scratch (using existing `PlayerInput` components).

## Decisions

### 1. Game State and Time Control
- **Decision**: Use `Time.timeScale = 0` immediately in `ProcessDeath`.
- **Rationale**: Replicates the "Devil Daggers" freeze-frame impact.

### 2. Unity 6 Input Handling
- **Decision**: Eliminate all `UnityEngine.Input` code. For mouse positioning, use `Mouse.current.position.ReadValue()`.
- **Rationale**: Unity 6 throws `InvalidOperationException` if `activeInputHandler` is set to `1` (New Input System only) and any legacy Input code is executed.

### 3. EventSystem Compatibility
- **Decision**: Update `SceneSetupHelper` to replace `StandaloneInputModule` with `InputSystemUIInputModule`.
- **Rationale**: The legacy module is incompatible with the New Input System and causes errors in Unity 6 when interacting with UI.

### 4. Manager Infrastructure
- **Decision**: Ensure `UIAnimationManager` is a singleton and that `SceneSetupHelper` creates an `_Managers` object to hold it.
- **Rationale**: Prevents `NullReferenceException` when `GameManager` tries to call animation methods during the death sequence.

## Risks / Trade-offs

- **[Risk]** `Mouse.current` might be null in some environments. → **Mitigation**: Add null checks for `Mouse.current` and use `Camera.main.ScreenToWorldPoint` only with the new mouse position data.
- **[Risk]** Buttons not clickable if `Time.timeScale = 0`. → **Mitigation**: Ensure UI components use `Unscaled Time` and `InputSystemUIInputModule` is properly configured.
- **[Risk]** Conflict with existing `PlayerInput` setups. → **Mitigation**: Ensure all input reading happens through the `moveAction` and `attackAction` variables already present in the scripts.

## UI Hierarchy (Unity 6 Optimized)
```
_Managers (Object)
 └── UIAnimationManager (Script)

CanvasResultados (Canvas + ResultsUIRegisterer)
 ├── DeathFlash (Image: White, Alpha 0)
 └── PanelResultados (CanvasGroup + VerticalLayoutGroup)
      ├── Título ("PARTIDA FINALIZADA")
      ├── StatsContainer
      │    ├── SurvivalTime (Ticker Text)
      │    └── Kills (Ticker Text)
      ├── BestTimeText
      └── ButtonGroup
           ├── RetryButton (Button + ButtonHoverEffect)
           └── MenuButton (Button + ButtonHoverEffect)

EventSystem (InputSystemUIInputModule)
```
