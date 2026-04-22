## Why

The `SampleScene` UI is currently cluttered with non-functional duplicate elements (a ghost timer stuck at 0) and obsolete record-tracking features that are no longer required. Additionally, broken references in the `GameManager` cause match statistics (kills and survival time) to erroneously display as zero on the Game Over screen, significantly degrading the player's experience.

## What Changes

- **Removal**: Deleted the non-functional `TimerHUD` GameObject that sits directly under `CanvasResultados`.
- **Removal**: Removed all record-tracking UI components, including `BestTimeHUD`, `BestTimeText`, and the `NewRecordBadge`.
- **Logic Removal**: Pruned high-score persistence logic (`bestTime` variables, `PlayerPrefs` loading/saving) from the `GameManager`.
- **UI Refinement**: Redesigned the `PanelResultados` to include a prominent "GAME OVER" title and a more thematic, high-contrast visual style.
- **Logic Modification**: Updated the Game Over sequence to ensure stats are assigned immediately and animated smoothly, preventing "stuck at zero" visual glitches.

## Capabilities

### New Capabilities
- `game-over-ui-v2`: Defines the requirements for the updated Results Panel, including the new title header, improved layout, and parallel animation sequences for stats.

### Modified Capabilities
- `game-timer`: Modified to remove record-tracking requirements and enforce a single-source-of-truth HUD timer.

## Impact

- **Assets/GameManager.cs**: Significant pruning of high-score logic and refinement of the death/results sequence.
- **Assets/ResultsUIRegisterer.cs**: Removal of pruned UI references (BestTime, Badge).
- **Assets/Editor/SceneSetupHelper.cs**: Updated automation logic to reflect the simplified HUD and the new Results Panel structure.
- **Assets/Scenes/SampleScene.unity**: Direct hierarchy cleanup to remove duplicates.
- **Player Experience**: Improved clarity and visual impact upon match completion.
