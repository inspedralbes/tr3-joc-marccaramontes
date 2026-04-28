## 1. Setup Automation

- [x] 1.1 Update `SceneSetupHelper.cs` to ensure `SetupLobby` styles all text in the Waiting Panel as `bloodRed`.
- [x] 1.2 Ensure `SetupLobby` applies the `InfernalButton` script to all buttons within the Waiting Panel.
- [x] 1.3 Configure background transparency for the Waiting Panel's image component in `SetupLobby`.

## 2. Dynamic Feedback Implementation

- [x] 2.1 Update `LobbyController.cs` to start the `PulseScale` coroutine on `roomCodeText` when a room is successfully created.
- [x] 2.2 Update `LobbyController.cs` to start the `PulseScale` coroutine on `roomCodeText` when a room is successfully joined.
- [x] 2.3 Ensure the `PulseScale` coroutine uses the correct parameters for a subtle waiting animation.

## 3. Layout and Final Polish

- [x] 3.1 Adjust `SetupLobby` to center the Waiting Panel's `RectTransform` and its children.
- [x] 3.2 Ensure `LobbyBackground` circles are sized appropriately to not clutter the Waiting Panel text.
- [x] 3.3 Verify all visual changes in the Lobby scene after running the setup tool.
