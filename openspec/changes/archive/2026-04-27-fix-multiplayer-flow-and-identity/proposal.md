## Why

The multiplayer mode is currently unplayable because local players are frozen due to incorrect authority assignment. Additionally, the lack of a clear identity link between the client and server causes scoring inconsistencies, and the game-over flow in 1vs1 mode provides no feedback to the first player who dies, making the game appear unresponsive.

## What Changes

- **Authority Fix**: `PlayerMovement` will now correctly identify the local player even when a network session is active, enabling movement and shooting.
- **Identity Synchronization**: `NetworkManager` will explicitly store the player's identity (`localPlayerId`) provided by the server/lobby to ensure consistent death reporting and scoring.
- **Improved 1vs1 Survival Flow**: `GameManager` will provide visual feedback (e.g., "Waiting for rival...") and stop the local timer immediately when the local player dies, while keeping the match active until the rival also dies.
- **Automatic Cleanup**: Ensure remote "ghosts" are properly identified and don't interfere with local authority logic.

## Capabilities

### New Capabilities
- `multiplayer-identity-sync`: Ensures player IDs are consistent across HTTP and WebSocket communications.
- `survival-match-flow`: Manages the transition from active play to spectating/waiting in 1vs1 matches.

### Modified Capabilities
- `player-positioning`: Updating how authority is determined at runtime.

## Impact

- **Assets/PlayerMovement.cs**: Logic for `isLocalPlayer` assignment in `Awake`.
- **Assets/Networking/NetworkManager.cs**: Storage and use of `localPlayerId`.
- **Assets/Networking/LobbyController.cs**: Capturing and passing identity from the lobby to the manager.
- **Assets/GameManager.cs**: Death sequence and 1vs1 state management.
