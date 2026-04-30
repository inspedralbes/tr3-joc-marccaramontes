## Context

The game currently uses a default Unity 2D configuration: a blue background color, standard global lighting, and a very minimalist HUD that only shows the timer. To align with the "Infernal" theme and *Devil Daggers* inspiration, we need a technical transition to a high-contrast, atmospheric environment where lighting and UI play a central role in the player's experience.

## Goals / Non-Goals

**Goals:**
- Implement an "Abyss" visual style using black camera backgrounds and URP 2D lighting.
- Create a comprehensive HUD that tracks kills, time, and records in real-time.
- Automate the environment and UI setup via the `SceneSetupHelper` editor tool.
- Ensure the Player acts as the primary light source in the scene.

**Non-Goals:**
- Modifying enemy or player sprites/animations.
- Changing the 2D gameplay to 3D.
- Implementing a full inventory or complex meta-progression UI.

## Decisions

### 1. URP 2D Lighting Strategy
- **Decision**: Use a low-intensity `Global Light 2D` (0.15) combined with a high-intensity `Point Light 2D` (Color: #FFCC88, Outer Radius: 7-10) parented to the Player.
- **Rationale**: This creates the "Circle of Life" effect where the player can only see what is nearby, increasing tension.
- **Alternatives**: Using a custom shader for a "fog of war" effect, but 2D Lights are native to URP and easier to manage with existing assets.

### 2. Post-Processing Configuration
- **Decision**: Force `Vignette` (Intensity: 0.45, Smoothness: 0.3) and `Film Grain` (Intensity: 0.2) in the `DefaultVolumeProfile`.
- **Rationale**: The vignette focuses the player's attention on the center (the platform) and hides the edges of the "void," while the grain adds a retro, gritty texture.

### 3. Unified HUD Hierarchy
- **Decision**: Integrate the HUD into the `CanvasResultados` but as a separate sibling to `PanelResultados`.
- **Structure**:
  - `CanvasResultados`
    - `HUDGroup` (CanvasGroup for fading)
      - `KillsCounter` (Top-Left, with pulse animation logic)
      - `TimerCenter` (Top-Center, large font)
      - `BestRecord` (Top-Right, semi-transparent until beaten)
    - `PanelResultados` (The existing death screen)
- **Rationale**: Reusing the existing Canvas reduces overhead and allows the `ResultsUIRegisterer` to manage all UI references in one place.

### 4. Dynamic HUD Feedback in GameManager
- **Decision**: Update the `GameManager.Update` loop to refresh all HUD elements every frame when in `Playing` state.
- **Record Logic**: If `survivalTime > bestTime`, the `TimerCenter` color should transition from white to orange (#FF8000) to signal a new record in progress.

## Risks / Trade-offs

- **[Risk]** → 2D Lights can be performance-heavy on mobile or very old hardware.
- **Mitigation** → Limit the scene to one primary point light (Player) and ensure all sprites use the `UniversalURP/2D/Sprite-Lit-Default` material.
- **[Risk]** → "Pitch Black" might make it impossible to see the platform boundaries.
- **Mitigation** → Keep `Global Light 2D` at 0.15 intensity so the platform is barely visible even without the player's light.
- **[Risk]** → `SceneSetupHelper` overwriting manual scene changes.
- **Mitigation** → Use `FindObject` checks before instantiating new lights or HUD elements; only update parameters if they differ from the "Infernal" standard.
