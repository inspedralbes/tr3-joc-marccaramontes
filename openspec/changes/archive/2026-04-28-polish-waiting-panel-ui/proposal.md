## Why

The current Waiting Panel in the Lobby is visually basic and does not match the "Infernal" theme established in the Main Menu. A polished waiting experience is crucial for player retention while waiting for opponents.

## What Changes

- **Visual Theme Integration**: Apply the `InfernalTheme` to all UI elements in the Waiting Panel.
- **Dynamic Feedback**: Add animations to the Room Code and Status Text.
- **Layout Improvement**: Center the Waiting Panel elements and add a background that matches the Lobby's atmosphere.

## Capabilities

### New Capabilities
- `lobby-waiting-visuals`: Defines the visual requirements for the waiting state in the multiplayer lobby.

### Modified Capabilities
- `multiplayer-lobby`: Update the lobby requirements to include thematic consistency for the waiting state.

## Impact

- **Affected Systems**: `Lobby.unity` scene, `LobbyController.cs` (UI references).
- **APIs**: None.
- **Assets**: `InfernalTheme.asset`, `UIThemeSO.cs`.
