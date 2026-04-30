## Why

The current main menu is a functional placeholder that lacks the atmospheric identity of the game. To align with the "Devil Daggers" aesthetic, we need a high-impact, "juicy" visual experience that establishes a dark, oppressive tone from the first interaction.

## What Changes

- **Visual Overhaul**: Complete replacement of the menu's visual style with a high-contrast, "horror gótico" aesthetic (deep blacks, glowing reds).
- **Interactive Player Circle**: The player's circle model will be integrated into the menu as a central, reactive element with breathing animations and pulse effects.
- **Particle-Based UI Feedback**: Integration of `PixelExplosion` effects for menu button interactions (hover/click), creating a "burning" or "disintegrating" feel.
- **Atmospheric Post-Processing**: Aggressive use of Film Grain, Bloom, Vignette, and Chromatic Aberration specifically tuned for the menu scene.
- **Dynamic Transitions**: Implementation of a "Pixel Ash" transition between the menu and the game scene.

## Capabilities

### New Capabilities
- `infernal-menu-ui`: Covers the visual requirements, interactive elements, and feedback systems of the new main menu.
- `retro-post-processing-config`: Defines the specific visual filters and post-processing profiles required for the 90s gothic aesthetic.

### Modified Capabilities
- `game-modes`: Update requirements to include specific visual feedback or "descent" animations when selecting modes.

## Impact

- **Assets**: New UI Sprites (low-res/dithered), Particle Materials, and Sound Effects (UI hover/select).
- **Scripts**: `MenuController` will be updated for interactive logic; `PixelExplosion` will be adapted for UI; new scripts for circle animation and post-processing control.
- **Scenes**: Significant modification to the `Menu` scene layout and rendering settings.
