## 1. Identity and Authorization

- [x] 1.1 Update `Assets/Networking/LobbyController.cs` to set `NetworkManager.Instance.localPlayerId` when joining or creating a room.
- [x] 1.2 Modify `Assets/PlayerMovement.cs` to assume `isLocalPlayer = true` by default in `Awake`.
- [x] 1.3 Update `Assets/Networking/RemotePlayerManager.cs` to explicitly set `isLocalPlayer = false` on remote "ghost" instances after instantiation.

## 2. Survival Match Flow

- [x] 2.1 Update `Assets/GameManager.cs` to stop the survival timer locally when the local player dies in `GameMode.Online`.
- [x] 2.2 Add visual feedback (HUD text update) in `GameManager.ProcessDeath` when waiting for a rival in online mode.
- [x] 2.3 Ensure the `localPlayerId` is correctly sent in the `DEATH` event from `GameManager.ProcessDeath`.

## 3. Verification and Polish

- [x] 3.1 Verify that the player can move and shoot immediately upon entering a multiplayer match.
- [x] 3.2 Verify that death times are correctly recorded and compared at the end of a match.
- [x] 3.3 Ensure the UI doesn't "freeze" when the local player dies first in 1vs1.
