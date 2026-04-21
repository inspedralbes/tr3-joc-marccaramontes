## 1. Core Logic and State

- [x] 1.1 Add `DeathTransition` to `GameState` enum in `Assets/GameTypes.cs`.
- [x] 1.2 Implement `LoadHighScore` and `SaveHighScore` using `PlayerPrefs` in `Assets/GameManager.cs`.
- [x] 1.3 Implement `CleanupScene()` in `Assets/GameManager.cs` to find and destroy objects with "Enemy" and "Bullet" tags.

## 2. Death Sequence Implementation

- [x] 2.1 Create `DeathSequenceCoroutine` in `Assets/GameManager.cs` to handle the transition (TimeScale 0.3f, delay 1.5s).
- [x] 2.2 Update `ProcessDeath()` to trigger the coroutine and set state to `GameState.DeathTransition`.
- [x] 2.3 Ensure `Time.timeScale` is reset to 1.0f in `ResetSession()` or `StartGame()`.

## 3. UI and Prefab Updates

- [x] 3.1 Add a new `bestTimeText` (TMP) and `newRecordBadge` (GameObject) field to `Assets/ResultsUIRegisterer.cs`.
- [x] 3.2 Modify the `PanelResultados` prefab to include the new UI elements for Best Time and New Record badge.
- [x] 3.3 Update `GameManager.RegisterResultsUI` and `ShowResults` to handle the new UI references and display persistence data.

## 4. Testing and Verification

- [x] 4.1 Verify the slow-motion effect and 1.5s delay upon death.
- [x] 4.2 Verify that High Scores persist after closing and reopening the game.
- [x] 4.3 Verify that no enemies or bullets remain in the scene when the results panel is shown.
