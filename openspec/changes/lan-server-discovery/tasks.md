## 1. Core Discovery Logic

- [x] 1.1 Create `Assets/Networking/LANDiscoveryManager.cs` to handle UDP socket operations.
- [x] 1.2 Implement the `BroadcastServer(string ip)` method to send periodic UDP packets on port 4545.
- [x] 1.3 Implement the `ListenForServers()` method using an asynchronous UDP receive loop.
- [x] 1.4 Implement a helper method to retrieve the local IPv4 address of the machine.

## 2. UI Integration

- [x] 2.1 Modify `LobbyController.cs` to include a reference to `LANDiscoveryManager`.
- [x] 2.2 Implement `HandleServerDiscovered(string ip)` in `LobbyController` to update the UI and `NetworkManager`.
- [x] 2.3 Add visual feedback in the Lobby UI (e.g., "Buscando servidor..." / "Servidor detectado").
- [x] 2.4 Implement logic to detect manual input in `serverAddressInputField` and disable auto-fill temporarily.

## 3. Workflow Implementation

- [x] 3.1 Update `LobbyController.OnCreateRoom` to start the discovery broadcast once the room is created.
- [x] 3.2 Ensure discovery listening starts automatically when the Lobby scene is loaded.
- [x] 3.3 Ensure discovery stops correctly when leaving the Lobby or starting a match to free the port.

## 4. Validation

- [x] 4.1 Test discovery in a single machine using two instances (Editor and Standalone build).
- [x] 4.2 Verify that manual IP entry still works and takes precedence over auto-discovery.
- [x] 4.3 Verify that the Room ID system remains functional and independent of IP discovery.
