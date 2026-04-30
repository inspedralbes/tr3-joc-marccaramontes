## Why

The current gameplay visuals are too generic and don't match the "Infernal" theme established in the main menu. The lack of a high-quality HUD and atmospheric effects (abyss, dynamic lighting) makes the game feel unfinished and disconnected from its intended *Devil Daggers* inspiration, reducing player immersion and motivation.

## What Changes

- **Atmospheric Abyss**: Replace the default blue background with a solid black "void" and implement post-processing effects including high-intensity Vignette and Film Grain to enhance the dark atmosphere.
- **Infernal HUD**: Overhaul the in-game UI to include a centralized high-visibility timer, a kill counter with pulse effects, and a live "Best Time" display that highlights when a record is being approached or broken.
- **Dynamic Lighting System**: Transition to a darkness-first approach where a 2D Point Light is attached to the player (acting as a torch) and the platform's color is darkened to simulate a stone/burnt altar in the void.
- **Automated Scene Styling**: Extend the `SceneSetupHelper` editor tool to automatically configure the camera, lighting, and HUD elements, ensuring consistency across scenes.

## Capabilities

### New Capabilities
- `game-visuals-atmosphere`: Defines the requirements for the "Abyss" look, including camera background, post-processing profiles, and environmental color palette.
- `dynamic-player-lighting`: Defines the requirements for the player's light aura, its radius, color, and how it reveals the platform and enemies.
- `extended-hud-tracking`: Defines the requirements for the real-time HUD elements including kill counts and record tracking.

### Modified Capabilities
- `game-timer`: Updates the requirement for the timer display to be centralized and styled with the Infernal theme colors (#FF8000 / #FF0000).

## Impact

- **Assets/GameManager.cs**: Addition of HUD references and logic to update the kill counter and record display in real-time.
- **Assets/Editor/SceneSetupHelper.cs**: Implementation of automated visual configuration (Camera, Lighting, HUD hierarchy).
- **Assets/ResultsUIRegisterer.cs**: Extension to register new HUD components with the GameManager.
- **Assets/Scenes/SampleScene.unity**: Direct modification of the scene's visual parameters (Camera background, Global Light intensity, Platform color).
- **Assets/DefaultVolumeProfile.asset**: Configuration of Vignette and Film Grain values.
