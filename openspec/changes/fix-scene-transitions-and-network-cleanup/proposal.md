## Why

The current network implementation has multiple "leaks" that cause `MissingReferenceException` when scenes are reloaded (Menu -> Lobby -> Game -> Menu). Additionally, inconsistencies in event naming (`player_shoot` vs `player_shot`) and missing disconnection handling for remote players lead to "ghost" players and unresponsive gameplay elements.

## What Changes

- **Event Cleanup**: Implement `OnDisable`/`OnDestroy` patterns in `RemotePlayerManager` and `PlayerShooting` to unregister from `NetworkManager` events.
- **Naming Unification**: Standardize event names between Client and Server (fixing the `player_shoot`/`player_shot` mismatch).
- **Disconnection Handling**: Add a `player_left` event listener to `NetworkManager` and `RemotePlayerManager` to destroy remote player objects when they leave the room.
- **Graceful Departure**: Update `GameManager.ReturnToMenu()` to notify the server that the player is leaving the room before switching scenes.

## Capabilities

### New Capabilities
- `network-lifecycle`: Covers the cleanup of network events and room membership during scene transitions and object destruction.

### Modified Capabilities
- `network-core`: Adding requirements for handling peer disconnection (`player_left`) and standardized event naming.
- `shooting-mechanics`: Ensuring remote shooting synchronization is correctly integrated with the network layer.

## Impact

- **Affected Code**: `NetworkManager.cs`, `RemotePlayerManager.cs`, `PlayerShooting.cs`, `GameManager.cs`.
- **API**: Changes to Socket.io event naming conventions.
- **Systems**: Multiplayer synchronization, Scene Management.
