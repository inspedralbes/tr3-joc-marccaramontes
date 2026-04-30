## MODIFIED Requirements

### Requirement: Server Connection
**REMOVED**: Replaced by Unity Netcode for GameObjects (NGO) and Relay.
**Reason**: Custom WebSocket/Node.js relay is being deprecated for a more robust standard framework.
**Migration**: Use `NetworkManager.StartHost()` or `NetworkManager.StartClient()` with `UnityTransport`.

### Requirement: Agent State Synchronization
**REMOVED**: Replaced by `NetworkTransform` and `NetworkObject`.
**Reason**: Manual `ENEMY_SYNC` events are redundant with NGO's automated synchronization.
**Migration**: Attach `NetworkTransform` to enemy prefabs.

### Requirement: Robust JSON Message Envelope
**REMOVED**: Replaced by NGO's internal binary serialization.
**Reason**: NGO handles packet formatting and serialization automatically, which is more efficient than JSON.
**Migration**: Define RPC parameters and `NetworkVariable` types; NGO will handle the underlying binary transport.

### Requirement: Connection Resilience
The system SHALL handle unexpected disconnections by notifying the user and returning them to the main menu.

#### Scenario: Server Disconnection
- **WHEN** the NGO connection is lost or the Host shuts down
- **THEN** the `NetworkManager` SHALL trigger the `OnClientDisconnectCallback`, show a "Connection Lost" message, and load the Menu scene
