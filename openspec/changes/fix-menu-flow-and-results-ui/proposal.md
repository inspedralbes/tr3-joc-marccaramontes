## Why

The game currently starts immediately in the menu without waiting for player input, and in multiplayer, the results screen fails to find the necessary UI components when the scene reloads for player 2 or when showing results. This leads to a confusing player experience and broken game loops.

## What Changes

- **Game State Protection**: Implement checks in `GameManager` to ensure death processing and survival timers only run during active gameplay, preventing unintended restarts from the Menu scene.
- **Robust UI Resolution**: Replace or augment `GameObject.Find` in `GameManager.ShowResults` with a more reliable reference system, ensuring the results panel and texts are found even after scene reloads.
- **Menu/Game Separation**: Explicitly separate the "Menu" scene logic from the "Game" scene logic in the `GameManager`.

## Capabilities

### New Capabilities
- `game-state-management`: Control the lifecycle of a game session (Ready, Playing, Paused, Finished) and prevent logic from running in unauthorized states.
- `dynamic-ui-registration`: A mechanism where UI elements in the game scene register their references to the persistent `GameManager` when they are loaded.

### Modified Capabilities
- `game-modes`: Refine the start/stop requirements for Solo and Multiplayer modes to respect the new state management.

## Impact

- **GameManager.cs**: Centralized state management and improved UI finding/registration logic.
- **PlayerMovement.cs / Enemy.cs**: Addition of state checks before triggering death or other gameplay-sensitive events.
- **UI Prefabs**: Potential addition of a small "UI Link" script to register elements with the `GameManager`.
- **Scene Structure**: Ensuring the `SampleScene` and `Menu` scene have the expected UI objects or registration hooks.
