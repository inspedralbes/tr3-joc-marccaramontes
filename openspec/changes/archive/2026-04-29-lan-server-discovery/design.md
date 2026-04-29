## Context

The current multiplayer implementation requires players to manually enter the host's IP address. This is a common pain point for LAN gaming. The game architecture consists of a standalone Node.js server and Unity clients communicating via WebSockets and HTTP.

## Goals / Non-Goals

**Goals:**
- Implement a UDP broadcast/listener system to discover the server IP automatically.
- Integrate discovery status into the existing Lobby UI.
- Maintain the current Room ID and Player Name workflow.

**Non-Goals:**
- Automatic server process launching (the user still starts Node.js manually).
- NAT traversal or WAN discovery.
- Complex server lists (only the first discovered server will be auto-filled for now).

## Decisions

### 1. UDP Discovery Mechanism
We will use a specialized `LANDiscoveryManager` MonoBehaviour.
- **Host Role**: When a player creates a room, they become the "de facto" host (even if the server is standalone). The Host's Unity instance will broadcast its local IP periodically.
- **Client Role**: On entering the Lobby, the client starts listening for these broadcasts.
- **Rationale**: Simple, low-overhead, and works reliably on standard local networks.

### 2. Discovery Payload
- **Protocol**: UDP.
- **Port**: 4545 (configurable).
- **Message Format**: `AEA_SERVER_DISCOVERY|[IP]|3000`
- **Rationale**: Using a unique prefix (`AEA_SERVER_DISCOVERY`) ensures we don't pick up traffic from other applications.

### 3. UI Integration Logic
The `LobbyController` will subscribe to the discovery events.
- **Discovery State**: A "Buscando servidor..." message will appear near the server address field.
- **Auto-fill**: Once a packet is received, the `serverAddressInputField` will be updated automatically, and `NetworkManager.UpdateServerAddress` will be called.

## Risks / Trade-offs

- **[Risk] Firewall Blocking** → Some Windows firewalls block UDP broadcast by default.
  - *Mitigation*: The manual input field remains functional as a fallback.
- **[Risk] Multiple Servers** → If two people host on the same LAN, the client might jump between IPs.
  - *Mitigation*: For the prototype, we will auto-fill the first one found. In the future, a "Server List" could be implemented.

## UI Flow State Machine

```
[Main Panel] 
     │
     ▼
[Start Discovery] ──▶ [Listen Mode] ──(Timeout?)──▶ [Status: Manual Entry Required]
                          │
                   (Packet Received)
                          │
                          ▼
                  [Auto-fill IP] ──▶ [Status: Servidor Detectado]
```
