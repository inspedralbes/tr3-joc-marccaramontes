## Why

The current build of `SampleScene` fails to reflect the intended "Infernal" atmosphere seen in the Unity Editor. This is because critical atmospheric settings—such as Camera post-processing, solid black background, and 2D lighting configurations—are either disabled in the scene file or not automatically configured by existing editor tools. This inconsistency breaks player immersion and deviates from the *Devil Daggers* aesthetic inspiration.

## What Changes

- **Automated Atmosphere Setup**: Extend the `SceneSetupHelper.cs` editor tool with a `SetupAtmosphere` method to automatically configure the Camera (Solid Color, Black, Post-Processing ON), Global Light (0.15 intensity), and Platform color (#1A1A1A).
- **Player Torch Component**: Ensure the `LocalPlayer` object (and its prefab) has a `Point Light 2D` component with a warm amber color (#FFCC88) and an 8-unit radius.
- **Scene Persistence**: Update `SampleScene.unity` to persist these visual settings, ensuring they are correctly included in the build.
- **Visual Standardization**: Ensure all game scenes follow the same high-contrast, dark-ambient requirements.

## Capabilities

### New Capabilities
- None.

### Modified Capabilities
- `game-visuals-atmosphere`: Update requirements to explicitly include Camera `renderPostProcessing` flag and solid black background settings.
- `dynamic-player-lighting`: Explicitly require a `Point Light 2D` on the player object as a "torch" effect.

## Impact

- **Assets/Editor/SceneSetupHelper.cs**: Implementation of atmospheric automation logic.
- **Assets/Scenes/SampleScene.unity**: Direct configuration of camera, lighting, and object materials.
- **Assets/PlayerMovement.cs**: Potential check to ensure light is active on start.
- **Assets/Settings/UniversalRP.asset**: Verification of Volume Profile link.
- **Build Quality**: Ensures "What You See Is What You Get" (WYSIWYG) between Editor and Build.
