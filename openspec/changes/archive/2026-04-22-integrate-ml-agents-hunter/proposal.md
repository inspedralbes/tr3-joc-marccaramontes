## Why

To fulfill technical requirement 4.6 (ML-Agents integration) by refining the existing `HunterAgent` to be smarter and better integrated into the multiplayer environment. This change also addresses requirement 4.5 by improving the stability and smoothness of agent synchronization across the network.

## What Changes

- **Dynamic Target Acquisition**: Refactor `HunterAgent` to dynamically find and pursue the nearest active player instead of a static transform.
- **Improved Network Synchronization**: Introduce a dedicated `ENEMY_SYNC` event to replace the semantically incorrect `SPAWN_ENEMY` for position updates.
- **Client-Side Interpolation**: Implement position smoothing (Lerp) in `HunterAgentNetworkSync` for remote clients to eliminate "teleporting" movement.
- **Host-Only AI Execution**: Ensure ML-Agents decision-making only runs on the Host to maintain consistent game state.
- **Semantic Naming**: Align networking event names with their actual purpose.

## Capabilities

### New Capabilities
- `ml-agents-hunter`: Detailed behavior and training objectives for the hunter agent.

### Modified Capabilities
- `network-core`: Update common networking events to include agent synchronization.
- `enemy-behavior`: Refine how enemies interact with multiple players in a networked session.

## Impact

- **Affected Code**: `Assets/HunterAgent.cs`, `Assets/HunterAgentNetworkSync.cs`, `Assets/Networking/NetworkManager.cs`.
- **Gameplay**: The hunter agent will now actively pursue all players, providing a more challenging and consistent experience for both host and clients.
- **Network**: Slightly increased bandwidth usage for position updates, mitigated by a optimized sync interval and interpolation.
