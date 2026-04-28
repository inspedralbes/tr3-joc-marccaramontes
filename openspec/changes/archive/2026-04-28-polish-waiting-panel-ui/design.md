## Context

The current Waiting Panel in the Lobby scene lacks thematic consistency with the rest of the game's UI. It is a simple panel that appears when a room is created or joined, but it doesn't provide the "Infernal" aesthetic present in the Main Menu.

## Goals / Non-Goals

**Goals:**
- Apply the `InfernalTheme` to the Waiting Panel.
- Use `SceneSetupHelper.cs` to automate the styling of the Waiting Panel.
- Add a pulsating animation to the Room Code text using `UIAnimationManager`.
- Ensure the background transparency allows background circles to be visible.

**Non-Goals:**
- Redesigning the underlying networking logic of the Lobby.
- Creating new textures or sprites (only existing assets and code-based styling will be used).

## Decisions

### Decision: Reuse `SceneSetupHelper` for Automation
We will update the `SetupLobby` method in `SceneSetupHelper.cs` to specifically target the Waiting Panel elements for styling.

**Rationale:**
This ensures that the "Infernal" style can be reapplied or updated across the entire scene with a single editor command, maintaining consistency and reducing manual work.

### Decision: Script-based Animation Trigger
We will update `LobbyController.cs` to trigger a pulse animation on `roomCodeText` when the room is successfully created or joined.

**Rationale:**
Visual feedback is crucial during waiting states. Using the existing `UIAnimationManager` keeps the code clean and consistent with other UI animations.

## Risks / Trade-offs

- **Risk**: Overlap of UI elements if the background circles are too large.
- **Mitigation**: Adjust circle sizes in `SetupLobby` if they interfere with text readability.

## UI Structure

```
Canvas
└── LobbyBackground
    ├── Circle_Left (Blue-ish)
    └── Circle_Right (Red-ish)
└── WaitingPanel (CanvasGroup)
    ├── Panel Background (Semi-transparent)
    ├── StatusText (Blood Red)
    ├── RoomCodeText (Pulsating, Blood Red)
    └── StartMatchBtn (Infernal Style)
```
