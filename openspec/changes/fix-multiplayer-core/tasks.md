## 1. Server-Side Infrastructure Fixes

- [x] 1.1 Update `GameService.js` to parse nested JSON in `handleJoinRoom` before accessing `roomId`
- [x] 1.2 Implement `playerId` injection in `GameService.js`'s `broadcastToRoom` function
- [x] 1.3 Update `SocketController.js` to support standardized uppercase event names (`MOVE`, `SHOOT`, etc.)

## 2. Client-Side Network Manager Updates

- [x] 2.1 Standardize `NetworkManager.cs` DTOs (MoveData, JoinData) to align with server-injected identity format
- [x] 2.2 Update `NetworkManager.HandleMessage` to correctly pass `playerId` to the corresponding C# events
- [x] 2.3 Refactor `NetworkManager.Emit` to ensure consistent serialization of payloads

## 3. Gameplay Synchronization Alignment

- [x] 3.1 Rename `update_position` event to `MOVE` in `PlayerMovement.cs`
- [x] 3.2 Update `PlayerMovement.PositionUpdate` DTO to remove redundant fields and match server expectations
- [x] 3.3 Verify `HunterAgentNetworkSync.cs` authority logic to ensure only the Host emits `ENEMY_SYNC`

## 4. Ghost Player and Identity Logic

- [x] 4.1 Update `RemotePlayerManager.cs` to use the server-provided `playerId` for ghost dictionary mapping
- [x] 4.2 Implement "Lazy Spawn" in `RemotePlayerManager.cs` for robust handling of late-joining players
- [x] 4.3 Fix `LobbyController.cs` to ensure `roomId` is consistently passed from the REST response to the Socket connection

## 5. Integration and Validation

- [x] 5.1 Run `Server/start-all.js` and verify connectivity via Gateway (localhost:3000)
- [x] 5.2 Test 1vs1 synchronization: verify movement, rotation, and player presence in both clients
- [x] 5.3 Confirm enemy synchronization: verify the Hunter Agent is controlled by Host but visible to Client
