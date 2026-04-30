## 1. UI Setup in SampleScene

- [x] 1.1 Create a new HUD Canvas or add to existing one a TMP Text for the timer. (Handled by SceneSetupHelper.cs)
- [x] 1.2 Add "Retry" and "Main Menu" buttons to the `PanelResultados` object. (Handled by SceneSetupHelper.cs)
- [x] 1.3 Configure button visuals (labels, sprites) to match existing UI. (Handled by SceneSetupHelper.cs)

## 2. UI Registration Logic

- [x] 2.1 Update `ResultsUIRegisterer.cs` with fields for `timerHUDText`, `retryButton`, and `menuButton`.
- [x] 2.2 In `ResultsUIRegisterer.TryRegister`, pass the new references to the `GameManager`.
- [x] 2.3 Implement automatic listener assignment in `ResultsUIRegisterer` or `GameManager` for the buttons.

## 3. GameManager Core Logic

- [x] 3.1 Update `GameManager.cs` to store references for the HUD timer and navigation buttons.
- [x] 3.2 Update `GameManager.Update` to refresh the HUD timer text when `currentState == GameState.Playing`.
- [x] 3.3 Refactor `GameManager.ProcessDeath` to handle Solo mode by transitioning to `GameState.GameOver` and showing results.
- [x] 3.4 Ensure the results panel correctly displays the final time for Solo mode (using the Player 1 field).

## 4. Navigation & Validation

- [ ] 4.1 Verify HUD timer starts at 0 and increments correctly during gameplay.
- [ ] 4.2 Verify death in Solo mode triggers the Results panel with navigation buttons.
- [ ] 4.3 Verify "Retry" button correctly restarts the match.
- [ ] 4.4 Verify "Main Menu" button correctly returns to the Menu scene.
- [ ] 4.5 Clean up any debug logs and ensure the UI is hidden by default.
