## Why

Multiplayer mode is currently non-functional due to three critical issues: a protocol/parsing error in the server that prevents players from joining rooms, a mismatch in event naming between Unity and the Node.js server, and a lack of player identity tracking in broadcasted messages. This change fixes these fundamental issues to enable functional 1vs1 multiplayer matches.

## What Changes

- **Server-side JSON Fix**: Correct `GameService.js` to properly parse nested JSON payloads from Unity, ensuring players are added to the correct rooms.
- **Event Name Alignment**: Synchronize message types (e.g., renaming Unity's `update_position` to `MOVE`) to ensure the server recognizes and broadcasts player actions.
- **Player Identity Injection**: Modify the server's broadcast logic to inject the sender's `playerId` into outgoing messages, allowing remote clients to identify which player performed an action.
- **Client-side Identity Handling**: Update `NetworkManager` and `RemotePlayerManager` to use the injected `playerId` for spawning and moving "ghost" players.
- **Robust Lobby Flow**: Ensure `LobbyController` and `NetworkManager` correctly handle room IDs and host status during transitions.

## Capabilities

### New Capabilities
- `multiplayer-identity-sync`: Implementation of a unified player identification system that persists through all network packets to resolve "ghost" player ambiguity.

### Modified Capabilities
- `network-core`: Updating the message "envelope" and protocol handling to support robust JSON parsing and event routing.
- `state-sync`: Redefining how position and action data are broadcast and applied to remote entities.

## Impact

- **Server**: `GameService.js` (room management, broadcast logic), `SocketController.js` (message routing).
- **Client (Unity)**: `NetworkManager.cs` (DTOs, message handlers), `PlayerMovement.cs` (sync rate, event names), `RemotePlayerManager.cs` (ghost instantiation/updates), `HunterAgentNetworkSync.cs` (enemy sync logic).
- **Protocols**: All WebSocket communication between Unity and the Game Service.
