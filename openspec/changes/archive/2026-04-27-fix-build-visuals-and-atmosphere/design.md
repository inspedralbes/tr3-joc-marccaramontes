## Context

The "Abyss" visual style requires specific URP 2D configurations that are easily lost or misconfigured during manual scene editing. Currently, `SampleScene` in builds appears flat and bright because:
1. The `Main Camera` has `renderPostProcessing` disabled.
2. The `Global Light 2D` is at default high intensity.
3. The player lacks a local light source (Point Light 2D).
4. The platform color does not match the dark charcoal requirement.

## Goals / Non-Goals

**Goals:**
- Automate the configuration of camera, lighting, and environmental colors via `SceneSetupHelper.cs`.
- Ensure consistency between the Unity Editor view and the final Build.
- Implement the "Torch" effect for the player character.

**Non-Goals:**
- Modifying the underlying Volume Profiles (Bloom, Grain, etc.) which are already defined in `DefaultVolumeProfile.asset`.
- Changing enemy lighting or specialized shaders.

## Decisions

### 1. Atmospheric Automation in `SceneSetupHelper`
- **Decision**: Implement a unified `SetupAtmosphere` method in the editor script.
- **Rationale**: Manual configuration of URP components (like `UniversalAdditionalCameraData`) is error-prone. A script ensures every scene follows the exact same visual spec.
- **Implementation**: The script will find the "Main Camera", "Global Light 2D", and objects tagged as "Platform" and "Player".

### 2. Camera URP Flags
- **Decision**: Explicitly set `cam.backgroundColor = Color.black`, `cam.clearFlags = CameraClearFlags.SolidColor`, and `additionalCamData.renderPostProcessing = true`.
- **Rationale**: Unity's default 2D template often leaves post-processing off by default. Forcing this via script ensures the `DefaultVolumeProfile` (Vignette/Grain) is actually rendered.

### 3. Player-Centric Lighting (The Torch)
- **Decision**: Add or update a `Light 2D` (Point) component on the player object.
- **Configuration**: Outer Radius: 8.0, Color: #FFCC88 (Amber), Intensity: 1.0.
- **Rationale**: This is the primary gameplay mechanic for visibility in the "Abyss".

### 4. Platform Visual Integration
- **Decision**: Search for the "Plataforma" object and force its `SpriteRenderer` color to `#1A1A1A`.
- **Rationale**: Ensures the platform is barely visible against the black background, emphasizing the player's torch.

## Risks / Trade-offs

- **[Risk]** → `SceneSetupHelper` might overwrite intentional manual lighting tweaks.
  - *Mitigation*: The script will check if a component already exists and only update its values if they significantly deviate from the "Infernal" standard.
- **[Risk]** → Performance impact of 2D lights on lower-end devices.
  - *Mitigation*: Keep the number of active lights low (1 Global, 1 Player Point Light).

## Hierarchy & Prefabs

### LocalPlayer Structure
```text
LocalPlayer (GameObject, Tag: "Player")
├── SpriteRenderer (Material: SpriteOutline)
├── Point Light 2D (New: Radius 8, Amber)
└── ... (Scripts)
```
