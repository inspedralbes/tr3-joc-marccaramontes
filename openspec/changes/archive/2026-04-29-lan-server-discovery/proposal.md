## Why

Typing local IP addresses manually is a friction point for players in a LAN environment. This change introduces an automatic discovery mechanism that allows the client to find the server's IP address without user intervention, while keeping the Room ID system for game organization.

## What Changes

- **Automatic Server Discovery**: Implementation of a UDP-based discovery system where the Host broadcasts its presence and Clients listen for it.
- **UI Integration**: The Lobby UI will show the status of the discovery and automatically fill the server address field when a server is found.
- **Maintained Workflow**: Players still need to provide their Name and the Room ID to join, but they no longer need to know the Host's IP.

## Capabilities

### New Capabilities
- `lan-discovery`: Provides the infrastructure for broadcasting and receiving server presence on the local network via UDP.

### Modified Capabilities
- `multiplayer-lobby`: The joining process is simplified by removing the manual IP entry requirement when a server is discovered.

## Impact

- **Assets/Networking/NetworkManager.cs**: Will be updated to accept IP updates from the discovery system.
- **Assets/Networking/LobbyController.cs**: UI logic will be updated to handle discovery states and auto-fill.
- **New Scripts**: `LANDiscoveryManager.cs` to handle the low-level UDP socket communication.
- **Network Traffic**: Small periodic UDP packets on the local network (broadcast).
