## Context

The current menu uses standard UGUI components with a basic `MenuController`. The visual style is inconsistent with the intended "horror gótico" direction. This design outlines the technical integration of particle effects, reactive animations, and post-processing to achieve the "Devil Daggers" look.

## Goals / Non-Goals

**Goals:**
- Implement a high-contrast, dark aesthetic for the Main Menu.
- Integrate the `PixelExplosion` system into UI button interactions.
- Create a reactive central player element (The Circle).
- Configure a dedicated Post-Processing Volume for the Menu scene.

**Non-Goals:**
- Overhauling the in-game HUD (this change is focused on the Main Menu only).
- Implementing new game mechanics or modes.
- Modifying the underlying `GameManager` logic for scene loading.

## Decisions

### 1. UI Feedback: Pixel-Based Disintegration
- **Decision**: Modify `PixelExplosion.cs` to support UI Space (Screen Space Overlay/Camera).
- **Rationale**: Reusing existing code is efficient, and `ParticleSystem` can render in UI using a `Canvas` with `RenderMode.ScreenSpaceCamera` or by using a custom sorting layer.
- **Alternative**: Creating separate UI-only shaders. Considered too complex for the current scope.

### 2. Reactive Central Element (The Circle)
- **Decision**: Create an `InfernalMenuCircle` script to handle breathing and pulse animations using sine waves.
- **Rationale**: Simple math-based animation is performant and gives the "alive" feeling characteristic of Devil Daggers without needing complex Animation clips.

### 3. Post-Processing: Retro-Gothic Profile
- **Decision**: Use a dedicated `VolumeProfile` for the Menu scene with aggressive Bloom (Intensity > 2), Film Grain (Large, high intensity), and Vignette.
- **Rationale**: These effects are the "glue" that creates the low-res gothic atmosphere.
- **Alternative**: Using a shader-based "downscaler". Rejected for now to stay within URP's standard feature set for stability.

### 4. UI Architecture: ScriptableObject Theme
- **Decision**: Create a `UIThemeSO` to store colors (Blood Red, Void Black) and font settings.
- **Rationale**: Allows for quick iterative visual tweaks without modifying prefabs.

## Risks / Trade-offs

- **[Risk]** → Particle systems in UI can be tricky to sort. 
  - *Mitigation*: Use a dedicated `Camera` for the UI or set the Particle System's `Sorting Layer` to match the Canvas.
- **[Risk]** → High Bloom intensity might make text unreadable.
  - *Mitigation*: Use TextMesh Pro with high-contrast outlines to maintain legibility.

## State Machine: Menu Flow

```text
[Idle] ──(Hover)──▶ [Focused/Pulse]
  │                     │
  └──────(Click)────────┴──▶ [Disintegrating] ──▶ [Load Scene]
```

## Prefab Hierarchy

- `MenuRoot` (Canvas)
  - `Background` (Image: Solid Black)
  - `CentralCircle` (Image + `InfernalMenuCircle`)
  - `MenuOptions` (Vertical Layout Group)
    - `Button_Ascend` (Button + `ButtonHoverEffect` + `PixelExplosion`)
    - `Button_Coven` (...)
    - `Button_Abandon` (...)
  - `PostProcessingVolume` (Global Volume)
