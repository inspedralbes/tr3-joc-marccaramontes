## 1. Cleanup & Logic Pruning

- [x] 1.1 Remove `bestTime`, `isNewRecord`, and `BestTimeKey` from `GameManager.cs`.
- [x] 1.2 Delete `LoadHighScore()` and `SaveHighScore()` methods in `GameManager.cs`.
- [x] 1.3 Remove record-related references from `ResultsUIRegisterer.cs` (`bestTimeText`, `newRecordBadge`, `bestTimeHUDText`).

## 2. UI Structure Refinement

- [x] 2.1 Update `SceneSetupHelper.cs` to remove creation logic for record-related HUD and Results elements.
- [x] 2.2 Ensure `ResultsUIRegisterer.cs` has a clear reference for a `titleText` (can reuse or rename `winnerText`).
- [x] 2.3 Update `SceneSetupHelper.cs` to create/configure a header text with "PARTIDA FINALIZADA" in the Results Panel.
- [x] 2.4 Update `SceneSetupHelper.cs` to identify and deactivate/remove any `TimerHUD` GameObject found outside of the `HUDGroup`.

## 3. Results Sequence & Robustness

- [x] 3.1 Update `GameManager.ShowResultsSequence` to assign stat values (`p1Time`, `currentKills`) to text components immediately upon activation.
- [x] 3.2 Refactor `ShowResultsSequence` to use parallel animations (e.g., PulseScale) for stats instead of just sequential counting.
- [x] 3.3 Update `GameManager.FindResultsPanelExhaustive` to remove search logic for records and add support for the new title text.

## 4. Scene Validation

- [ ] 4.1 Run the `Tools/Setup HUD and Results UI` menu item in `SampleScene` to apply the new UI structure.
- [ ] 4.2 Manually verify the `CanvasResultados` hierarchy to ensure no duplicate `TimerHUD` remains at the root.
- [ ] 4.3 Test death in Solo mode to verify that the Game Over screen displays the new title and correct (non-zero) match statistics.
