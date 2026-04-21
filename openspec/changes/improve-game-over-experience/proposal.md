## Why

The current Game Over state is abrupt and lacks a polished transition, which negatively impacts player immersion. Additionally, the absence of high score persistence reduces the incentive for players to improve their performance and replay the game.

## What Changes

- **Death Sequence**: Introduce a brief period of slow-motion and a delay before showing the results panel.
- **High Score Persistence**: Implement a system to save and display the player's best survival time using PlayerPrefs.
- **Scene Cleanup**: Automatically destroy or disable active enemies and bullets when the Game Over state is triggered to clear the visual field.
- **Enhanced Results UI**: Update the results panel to include a "Best Time" display and visual feedback for new records.
- **Input & Logic Lock**: Ensure all gameplay systems (shooting, spawning, movement) are strictly disabled during the transition and Game Over state.

## Capabilities

### New Capabilities
- `persistence-manager`: Handles saving, loading, and comparing high scores using PlayerPrefs.
- `game-over-transition`: Manages the timed sequence between player death and the display of the results UI, including time-scale effects.

### Modified Capabilities
- `game-over-ui`: Requirements for the results panel to display "Best Time" and "Current Kills" prominently.
- `game-state-management`: Requirements for transitioning through a "DeathTransition" sub-state before reaching "GameOver".

## Impact

- **Assets/GameManager.cs**: Central hub for state transitions and high score management.
- **Assets/ResultsUIRegisterer.cs**: Updated to include new UI references for high scores.
- **Assets/Enemy.cs & Assets/Bullet.cs**: Logic to stop behavior or destroy themselves on Game Over.
- **UI Assets**: Modification of the `PanelResultados` prefab to add new text elements.
- **Audio (Optional)**: Potential addition of a Game Over sound effect.
