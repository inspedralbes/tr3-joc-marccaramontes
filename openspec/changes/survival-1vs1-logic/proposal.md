## Why

The current multiplayer logic ends the match for everyone as soon as one player dies. For a competitive "Survival 1vs1" mode, the game should continue until both players have fallen, allowing the second player to attempt to beat the first player's time.

## What Changes

- **Asynchronous Death Logic**: When a player dies in Online mode, they transition to a spectator state instead of triggering a global Game Over.
- **Duel HUD**: A new HUD overlay to track both the local player's survival time and the rival's status/time.
- **Spectator Mode**: Upon death, the camera will automatically follow the surviving rival.
- **Comparative Results Screen**: Updated UI to show both survival times side-by-side and declare a winner based on longevity.
- **End-of-Match Synchronization**: The match only officially ends when the server confirms both players have died or disconnected.

## Capabilities

### New Capabilities
- `survival-1vs1-mode`: Core logic for competitive survival where the winner is the one who lives longer.
- `spectator-camera`: System to handle camera transitions after local player death.

### Modified Capabilities
- `game-state-management`: Update state transitions to handle "Partial Game Over" (one player dead, match continuing).
- `multiplayer-results`: Change the results UI to compare two players instead of just showing one score.

## Impact

- **Affected Systems**: `GameManager.cs`, `PlayerMovement.cs`, `ResultsUIRegisterer.cs`, `NetworkManager.cs`.
- **UI**: HUD and Results panel layout changes.
- **Dependencies**: None.
