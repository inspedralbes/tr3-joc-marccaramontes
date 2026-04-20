## 1. Game State Management

- [x] 1.1 Add `GameState` enum to `GameTypes.cs` (Menu, Playing, GameOver).
- [x] 1.2 Update `GameManager` to include a `CurrentState` property and initialize it to `GameState.Menu`.
- [x] 1.3 Update `GameManager.StartGame` to set `CurrentState = GameState.Playing`.
- [x] 1.4 Update `GameManager.ReturnToMenu` to set `CurrentState = GameState.Menu`.
- [x] 1.5 Wrap logic in `GameManager.Update` (survival time) and `GameManager.ProcessDeath` with a check for `CurrentState == GameState.Playing`.
- [x] 1.6 Update `GameManager.ProcessDeath` to transition to `GameState.GameOver` when the second player dies in multiplayer.

## 2. UI Registration System

- [x] 2.1 Create `ResultsUIRegisterer.cs` to hold references to the results panel and TMP texts.
- [x] 2.2 Implement `RegisterResultsUI(ResultsUIRegisterer ui)` in `GameManager`.
- [x] 2.3 Refactor `GameManager.ShowResults` to use the registered `ResultsUIRegisterer` instead of `GameObject.Find`.
- [ ] 2.4 Add `ResultsUIRegisterer` to the `PanelResultados` object in `SampleScene` and assign its child references.

## 3. Validation

- [ ] 3.1 Verify the Menu scene no longer triggers accidental restarts.
- [ ] 3.2 Verify Multiplayer results display correctly without "PanelResultados not found" warnings.
- [ ] 3.3 Add a safety check in `GameManager.ResetSession` to clear UI references if needed when changing scenes.
