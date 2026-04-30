## 1. UI Infrastructure & Registration

- [x] 1.1 Update `ResultsUIRegisterer.cs` to add fields for the new HUD: `hudGroup` (CanvasGroup), `killsHUDText`, and `bestTimeHUDText`.
- [x] 1.2 Update `GameManager.cs` to include internal references for the new HUD components and a public `killsHUDText` for updates.
- [x] 1.3 Update `GameManager.RegisterResultsUI` to correctly bind the new references from the registerer.

## 2. HUD Logic & Feedback

- [x] 2.1 Update `GameManager.Update` to refresh the kill counter and best record display during the `Playing` state.
- [x] 2.2 Implement the record-breaking color transition (White to Orange) for the timer in `GameManager.Update`.
- [x] 2.3 Add a `PulseScale` Coroutine in `UIAnimationManager.cs` to handle UI element feedback.
- [x] 2.4 Trigger the `PulseScale` animation on `killsHUDText` within the `GameManager.AddKill` method.

## 3. Atmospheric & Lighting Setup

- [x] 3.1 Update `DefaultVolumeProfile.asset` (or create a delta configuration) to set Vignette intensity to 0.45 and Film Grain to 0.2.
- [x] 3.2 Update `SceneSetupHelper.cs` with a `SetupAtmosphere` method to force black camera background and 0.15 Global Light intensity.
- [x] 3.3 Extend `SceneSetupHelper.cs` to ensure the object with the "Player" tag has a `Point Light 2D` with the Infernal Amber color (#FFCC88).

## 4. Automated HUD Construction

- [x] 4.1 Update `SceneSetupHelper.cs` to automatically build the HUDGroup hierarchy (Kills, Timer, Record) within `CanvasResultados`.
- [x] 4.2 Configure all HUD element anchors (Top-Left, Top-Center, Top-Right) and initial styling via the setup tool.
- [x] 4.3 Execute the `Tools/Setup HUD and Results UI` command in `SampleScene` and verify the visual hierarchy in the Inspector.
- [x] 4.4 Final verification: Play the game and confirm the \"Abyss\" feel, dynamic lighting, and real-time HUD updates.
