## Why

The current Game Over state is abrupt and lacks a polished transition, which negatively impacts player immersion. By adopting a "Devil Daggers" inspired aesthetic, we can provide immediate, high-impact feedback upon death. Additionally, the project is migrating to Unity 6 (6000+), which requires specific fixes for the New Input System and EventSystem to prevent `InvalidOperationException` and `NullReferenceException`.

## What Changes

- **Death Sequence (Devil Daggers Style)**: Replace the current transition with a high-contrast, instant feedback sequence including a brief screen flash and time-freeze.
- **Fragmented Player Death**: The player character will now disintegrate into a "pixel explosion" (square particles) instead of simply stopping.
- **Dynamic Results UI**: 
    - **Fade-In Transition**: The results panel will fade in smoothly rather than appearing instantly.
    - **Digital Ticker**: Survival time and kill counts will "count up" rapidly with a digital ticker effect.
- **Reactive UI Buttons**: Buttons will feature a "Snap" scaling effect and color shift on hover to feel more responsive and "alive."
- **Unity 6 Compatibility Fixes (CRITICAL)**:
    - **Full Input Migration**: Remove all legacy `UnityEngine.Input` fallbacks in `PlayerMovement.cs` and `PlayerShooting.cs` to prevent "active Input handling" exceptions.
    - **Modern Event System**: Update `SceneSetupHelper` to use `InputSystemUIInputModule` instead of the legacy `StandaloneInputModule`.
    - **Manager Auto-Instantiation**: Update `SceneSetupHelper` to ensure an `_Managers` object with `UIAnimationManager` is created and configured in the scene.
- **High Score Persistence**: Implement local saving of the "Best Time" to encourage replayability.

## Capabilities

### New Capabilities
- `death-effects`: Handles the visual fragmentation of the player (particle explosion) and screen-flash effects.
- `persistence-manager`: Handles saving, loading, and comparing high scores using PlayerPrefs.
- `ui-animation-system`: A dedicated utility for handling UI Fades, counting text (tickers), and button hover effects, with proper Unity 6 singleton management.

### Modified Capabilities
- `game-over-ui`: Updated requirements for the results panel to include CanvasGroup for fading, improved layout for the "Best Time", and interactive button states.
- `game-state-management`: Requirements for transitioning through a "DeathTransition" state that freezes time (`Time.timeScale = 0`) immediately.

## Impact

- **Assets/GameManager.cs**: Central hub for state transitions, high score management, and triggering UI animations.
- **Assets/PlayerMovement.cs**: Updated to trigger the `death-effects`, stop input, and **REMOVED** all legacy `Input` calls.
- **Assets/PlayerShooting.cs**: **REMOVED** all legacy `Input` calls and updated mouse positioning for the New Input System.
- **Assets/ResultsUIRegisterer.cs**: Updated to include new UI references for high scores and link to the `ui-animation-system`.
- **Assets/Editor/SceneSetupHelper.cs**: Tooling update to ensure the new UI components (CanvasGroup, Button Listeners), **Modern EventSystem**, and **Manager Infrastructure** are correctly configured.
- **UI Assets**: Modification of the `PanelResultados` prefab to add a `CanvasGroup` and update styling.
