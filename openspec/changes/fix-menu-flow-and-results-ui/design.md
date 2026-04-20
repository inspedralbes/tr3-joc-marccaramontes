## Context

The `GameManager` is a persistent Singleton that manages the game flow across scenes. Currently, it lacks a strict state machine, leading to race conditions where death logic triggers during scene transitions or while in the Menu scene (likely due to the `Player` object being active or the `platformRadius` check failing before the scene is fully set up). Additionally, the `GameManager` loses UI references when a scene is reloaded (like between Player 1 and Player 2 turns in Multiplayer) because those UI objects are destroyed and recreated, while the `GameManager` persists.

## Goals / Non-Goals

**Goals:**
- Implement a `GameState` enum in `GameManager` (e.g., `Menu`, `Playing`, `GameOver`).
- Ensure `ProcessDeath` and `Update` (survival time) only execute when `GameState == Playing`.
- Implement a robust way for UI elements to re-link with the `GameManager` after scene loads using a "Registration" pattern.
- Fix the specific error where `PanelResultados` is not found by ensuring it's either correctly found or registered.

**Non-Goals:**
- Completely rewriting the `GameManager` or the UI system.
- Adding new game features or visual assets.

## Decisions

### 1. Enum-based State Machine
**Rationale**: Simple and effective for a game of this scale. It prevents logic from running in inappropriate scenes or timing windows.
**Alternative**: Checking `SceneManager.GetActiveScene().name`. *Rejected* as it's string-based and less flexible than an enum for internal logic.

### 2. UI Self-Registration via "UIRegisterer" Component
**Rationale**: Instead of `GameManager` searching for UI using `GameObject.Find` (which is slow and error-prone), a small `UIRegisterer` script will be added to the UI prefabs. Upon `Start`, it will pass its references to the `GameManager`.
**Alternative**: Keeping `GameObject.Find` but making it more robust. *Rejected* because it's still brittle and depends on exact string names in the hierarchy.

## Risks / Trade-offs

- **[Risk]**: If the `UIRegisterer` script is not added to new UI elements, they won't work. → **Mitigation**: Add a log warning in `GameManager` if a required UI element is missing when needed.
- **[Risk]**: Singleton race conditions during `Awake`. → **Mitigation**: Use `Start` for registration to ensure `GameManager.Instance` is already established.

## Visualización del Flujo

### Máquina de Estados de Juego
```ascii
       [Start App]
            │
            ▼
      ┌───────────┐
      │   MENU    │◄────────────────┐
      └─────┬─────┘                 │
            │ (Start Game)          │ (Return to Menu)
            ▼                       │
      ┌───────────┐                 │
      │  PLAYING  │                 │
      └─────┬─────┘                 │
            │ (Death)               │
            ▼                       │
      ┌───────────┐                 │
      │ GAME OVER │─────────────────┘
      └─────┬─────┘
            │ (Multiplayer Next Turn)
            ▼
      [Reload Scene] ──▶ (Reset to PLAYING)
```

### Jerarquía de Prefabs (UI de Resultados)
Para que el sistema de registro funcione, el objeto `PanelResultados` en la escena `SampleScene` debe tener la estructura esperada:
- **PanelResultados** (Componente: `ResultsUI` que se registra en `GameManager`)
    - **P1TimeText** (TextMeshProUGUI)
    - **P2TimeText** (TextMeshProUGUI)
    - **WinnerText** (TextMeshProUGUI)
