## 1. Editor Tooling Enhancement

- [x] 1.1 Implement `SetupAtmosphere(Scene scene)` method in `SceneSetupHelper.cs` to configure Camera (Solid Black, Post-Processing ON).
- [x] 1.2 Implement `SetupLighting(Scene scene)` method in `SceneSetupHelper.cs` to find/create `Global Light 2D` with 0.15 intensity.
- [x] 1.3 Add logic to `SetupAtmosphere` to identify the "Plataforma" object and set its color to #1A1A1A.
- [x] 1.4 Integrate `SetupAtmosphere` into the existing `Tools/Setup HUD and Results UI` menu command.

## 2. Player Character Setup

- [x] 2.1 Add a `Light 2D` (Point) component to the `LocalPlayer` object in `SampleScene`.
- [x] 2.2 Configure the player light: Outer Radius = 8.0, Color = #FFCC88, Intensity = 1.0.
- [x] 2.3 Verify if the `Point Light 2D` component should be added via code in `PlayerMovement.Awake()` as a fallback if missing.

## 3. Scene Automation & Persistence

- [x] 3.1 Run the `Tools/Setup HUD and Results UI` command in `SampleScene.unity`.
- [x] 3.2 Verify that the `Main Camera` now has the `renderPostProcessing` flag checked in the Inspector.
- [x] 3.3 Verify that `Global Light 2D` intensity is 0.15.
- [x] 3.4 Manually save the scene and check that settings persist in the `.unity` file (YAML).

## 4. Verification & Polish

- [x] 4.1 Enter Play Mode and confirm the "Vignette" and "Film Grain" effects are visible.
- [x] 4.2 Confirm the player character acts as the primary light source and the void is appropriately dark.
- [x] 4.3 Run a test build and verify that the build visuals match the Editor exactly.
