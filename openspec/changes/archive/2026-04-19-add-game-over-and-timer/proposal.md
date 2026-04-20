## Why

Currently, the game lacks a real-time survival timer and a proper Game Over screen for the Solo mode. This makes the experience abrupt, as the scene restarts immediately upon death without giving the player feedback on their performance or a choice to navigate back to the menu.

## What Changes

- **HUD Timer**: Implement a real-time survival timer display in the game scene.
- **Solo Game Over Flow**: Modify the death logic in Solo mode to transition to a "Game Over" state instead of an immediate restart.
- **Results Navigation**: Add "Retry" and "Main Menu" buttons to the results panel to allow players to choose their next action.
- **UI Registration**: Extend the UI registration system to handle the new HUD timer and navigation buttons.

## Capabilities

### New Capabilities
- `game-timer`: Real-time tracking and display of survival time on a HUD element.
- `game-over-ui`: Enhanced results panel with navigation buttons (Retry, Menu) and support for Solo mode.

### Modified Capabilities
- `game-modes`: Solo mode requirements updated to include a death transition to the results screen instead of immediate reload.

## Impact

- **GameManager**: Logic updates to handle the HUD timer and Solo mode death transition.
- **ResultsUIRegisterer**: New fields for HUD timer and navigation buttons.
- **SampleScene**: UI hierarchy updates to include the HUD timer and buttons in the Results Panel.
- **Assets**: Sprites and fonts for the new UI elements.
