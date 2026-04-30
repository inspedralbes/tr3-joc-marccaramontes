## Why

Despite the implementation of the "Devil Daggers" style Game Over, the results panel fails to appear upon player death. This is primarily due to a synchronization gap between the UI registration and the `GameManager`'s death sequence, as well as potential coroutine breaks when the `UIAnimationManager` singleton is missing or not yet initialized in Unity 6.

## What Changes

- **Robust Emergency UI Discovery**: Update `GameManager` to perform an exhaustive search for the Results Panel and its components if the registered references are missing at the time of death.
- **Fail-safe UI Sequence**: Modify the death sequence to display the Results Panel instantly if the `UIAnimationManager` is missing, ensuring the game loop is never blocked.
- **Early UI Registration**: Move `ResultsUIRegisterer` logic from `Start` to `Awake` to ensure the UI is known to the `GameManager` as early as possible.
- **Improved Singleton Persistence**: Ensure `UIAnimationManager` initializes correctly in Unity 6 environments and provides clear warnings without stopping the execution flow.

## Capabilities

### New Capabilities
- `self-healing-ui`: A mechanism within the `GameManager` to re-discover and re-attach UI components on the fly during critical transitions.

### Modified Capabilities
- `game-over-ui`: Updated requirements for registration timing and fallback behavior.
- `ui-animation-system`: Requirements for non-blocking execution when the animator is missing.

## Impact

- **Assets/GameManager.cs**: Logic for emergency UI discovery and non-blocking death sequence.
- **Assets/ResultsUIRegisterer.cs**: Change execution order to `Awake`.
- **Assets/UIAnimationManager.cs**: Ensure singleton robustness.
