## Context

The current death sequence in `SampleScene` is failing because the `CanvasResultados` is scaled to (0,0,0) in the scene file, and the `GameManager` logic is too restrictive during editor testing. Additionally, the automated setup tool `SceneSetupHelper` doesn't link all the necessary UI components, leading to null references in the `ResultsUIRegisterer`.

## Goals / Non-Goals

**Goals:**
- Ensure `CanvasResultados` has a scale of (1,1,1).
- Ensure `GameManager` enters `Playing` state automatically when a game scene is loaded directly.
- Ensure `SceneSetupHelper` correctly assigns the `resultsPanel`, `p1TimeText`, `p2TimeText`, and `winnerText` references.
- Fix the logic in `GameManager.ShowResults` to find and register the UI if it hasn't been registered yet.

**Non-Goals:**
- Redesigning the UI layout or appearance.
- Adding new game modes or features.

## Decisions

### 1. Automatic State Transition in GameManager
- **Decision**: In `Start()`, `GameManager` will check if the current scene is "SampleScene" and the state is "Menu". If so, it will transition to "Playing".
- **Rationale**: This allows developers to test gameplay directly from the scene without going through the main menu, which is essential for rapid iteration.

### 2. Enhanced UI Registration
- **Decision**: Update `GameManager.ShowResults` to perform a "lazy registration" by looking for a `ResultsUIRegisterer` in the scene if one isn't already known.
- **Rationale**: This provides a fail-safe in case the initial `TryRegister` call from the UI fails or the `GameManager` was destroyed and recreated.

### 3. Automated Setup Validation
- **Decision**: Update `SceneSetupHelper` to search for `PanelResultados`, `P1TimeText`, `P2TimeText`, and `WinnerText` by name and assign them to the `ResultsUIRegisterer`.
- **Rationale**: Manually assigning these in every scene is error-prone. The tool should handle the full configuration.

## Risks / Trade-offs

- **[Risk]** → Transitioning to `Playing` automatically might interfere with custom initialization logic in the future.
- **Mitigation** → Only transition if the state is specifically `Menu` and we are in the `SampleScene`.
- **[Risk]** → Scale of (1,1,1) might make the UI too big if the Canvas is configured for World Space.
- **Mitigation** → Ensure `CanvasResultados` is set to `Screen Space - Overlay` in the setup tool.

## UI Hierarchy
```
CanvasResultados (Scale 1,1,1, Screen Space - Overlay)
└── PanelResultados (Active: false)
    ├── P1TimeText (TextMeshPro)
    ├── P2TimeText (TextMeshPro)
    ├── WinnerText (TextMeshPro)
    ├── RetryButton (Button)
    └── MenuButton (Button)
```
