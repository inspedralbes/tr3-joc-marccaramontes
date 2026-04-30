## 1. Core Logic & State Management

- [x] 1.1 Update `GameManager.cs` to track individual player death times in a dictionary.
- [x] 1.2 Modify `PlayerMovement.Die()` to only trigger `GameManager.GameOver` if the rival is already dead.
- [x] 1.3 Implement a `DEATH` event handler in `NetworkManager.cs` to update the rival's status and time.
- [x] 1.4 Add a check in `GameManager.Update()` to transition to `GameOver` when all players have death times recorded.

## 2. Spectator Mode & HUD

- [x] 2.1 Update `PlayerMovement.Die()` to disable the player sprite and input instead of destroying the object.
- [x] 2.2 Implement camera focus switching in `PlayerMovement` to target the rival's `Ghost` object upon local death.
- [x] 2.3 Add a "Duel HUD" to the `SampleScene` with two timer displays (Local and Rival).
- [x] 2.4 Update the HUD during the match via `NetworkManager` events.

## 3. Results UI Comparison

- [x] 3.1 Update `ResultsUIRegisterer.cs` to include a new reference for `RivalTimeText`.
- [x] 3.2 Modify the `ResultsUIRegisterer.ShowResults()` method to compare the two times and display "VICTORIA!" or "DERROTA!".
- [x] 3.3 Create a new task in `SceneSetupHelper.cs` to automatically link the new UI fields in the results panel.

## 4. Final Polish & Testing

- [ ] 4.1 Test the transition from death to spectating with two local instances.
- [ ] 4.2 Verify that the winner is correctly identified in the Results screen.
- [ ] 4.3 Ensure the match ends correctly even if the rival disconnects abruptly.
