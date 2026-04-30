## 1. Core Network Updates

- [x] 1.1 Add `OnRemotePlayerLeft` event to `NetworkManager.cs`.
- [x] 1.2 Implement `HandleEvent` case for "player_left" in `NetworkManager.cs`.
- [x] 1.3 Add `LeaveRoom()` method to `NetworkManager.cs` that emits "leave_room" (or similar) to the server.
- [x] 1.4 Standardize internal naming for shooting events (ensure `OnRemotePlayerShot` matches the data received).

## 2. Remote Player Management

- [x] 2.1 Refactor `RemotePlayerManager.cs` to use `OnEnable` and `OnDisable` for event registration.
- [x] 2.2 Add `RemoveRemotePlayer(string playerId)` to `RemotePlayerManager.cs` to destroy the GameObject and remove from dictionary.
- [x] 2.3 Subscribe to `OnRemotePlayerLeft` in `RemotePlayerManager.cs`.

## 3. Shooting System Cleanup

- [x] 3.1 Refactor `PlayerShooting.cs` to use `OnEnable` and `OnDisable` for event registration.
- [x] 3.2 Ensure `OnRemotePlayerShot` is correctly unsubscribed to prevent `MissingReferenceException`.

## 4. Scene Transition Management

- [x] 4.1 Update `GameManager.ReturnToMenu()` to call `NetworkManager.Instance.LeaveRoom()` before loading the scene.
- [x] 4.2 Add a small delay or check to ensure the "leave_room" message is sent before the socket is potentially affected by the scene load.

## 5. Validation

- [ ] 5.1 Verify that returning to the menu no longer triggers console errors when other players move or shoot.
- [ ] 5.2 Verify that when a player closes their client, their ghost disappears from other clients.
