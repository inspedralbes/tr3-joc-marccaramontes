## Why

The current custom WebSocket/Node.js multiplayer system is difficult to scale, maintain, and requires manual management of every synchronization packet. Migrating to **Unity Netcode for GameObjects (NGO)** provides a robust, industry-standard framework with built-in synchronization, authority management, and seamless integration with **Unity Relay**. This allows for a more reliable multiplayer experience, easier development of new features, and simplifies connectivity for players by removing the need for manual server hosting or port forwarding.

## What Changes

- **Core Infrastructure**: Deprecation of the custom `NativeWebSocketClient` and custom `NetworkManager` in favor of Unity's `NetworkManager` (NGO).
- **Package Integration**: Addition of `com.unity.netcode.gameobjects` and `com.unity.services.relay` packages.
- **Object Identification**: Replacement of the custom `NetworkIdentity.cs` with the standard `NetworkObject` component.
- **Automated Synchronization**: Use of `NetworkTransform` for smooth position/rotation syncing, replacing manual `MOVE` and `ENEMY_SYNC` events.
- **State Management**: Implementation of `NetworkVariable` for synchronized game state (e.g., countdowns, player scores).
- **Authority Model**: Refactoring movement and combat to follow a Host-Authoritative model using `[ServerRpc]` and `[ClientRpc]`.
- **Relay Support**: Implementation of Unity Relay for NAT traversal, enabling players to connect over the internet without complex configurations.
- **BREAKING**: The current Node.js `game-service` will no longer handle real-time game traffic (movement, shooting). It may be retained only for meta-game API calls (results, rankings).

## Capabilities

### New Capabilities
- `netcode-integration`: Integration of the NGO framework and core NetworkManager setup.
- `relay-networking`: Implementation of Unity Relay and Lobby for internet connectivity.
- `ngo-state-sync`: Advanced state synchronization using NetworkVariables and RPCs.

### Modified Capabilities
- `network-core`: Transitioning from custom socket packets to NGO messaging.
- `player-positioning`: Replacing manual position emits with NetworkTransform.
- `enemy-behavior`: Updating AI authority to be strictly Host-managed via NGO.

## Impact

- **Assets/Networking/**: Extensive refactoring of `NetworkManager.cs` and `LobbyController.cs`. Removal of `NativeWebSocketClient.cs`.
- **Player & Enemy Scripts**: `PlayerMovement.cs`, `PlayerShooting.cs`, `Enemy.cs`, and `EnemySpawner.cs` will require significant updates to integrate NGO components and RPC logic.
- **Server Architecture**: The Node.js backend's real-time components will be deprecated. The `api-service` will need to be updated to accept results from the NGO Host.
- **Build Process**: New requirements for Unity Gaming Services (UGS) project configuration and package dependencies.
