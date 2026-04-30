## 1. UI Registration Reliability

- [x] 1.1 Move `ResultsUIRegisterer.TryRegister` call from `Start` to `Awake`.
- [x] 1.2 Implement a recursive retry logic in `ResultsUIRegisterer` to handle `GameManager` singleton initialization order.

## 2. Self-Healing Discovery in GameManager

- [x] 2.1 Update `GameManager.ShowResults` to trigger an emergency UI discovery if `resultsPanel` is null.
- [x] 2.2 Implement `FindResultsPanelExhaustive` method in `GameManager` using `Resources.FindObjectsOfTypeAll`.
- [x] 2.3 Ensure the emergency discovery also re-links buttons and text components.

## 3. Fail-safe Coroutine Execution

- [x] 3.1 Update `GameManager.DeathSequenceCoroutine` to check for `UIAnimationManager.Instance` and skip the flash/wait if missing.
- [x] 3.2 Update `GameManager.ShowResultsSequence` to display the panel instantly if `UIAnimationManager.Instance` is null.

## 4. Singleton Stability

- [x] 4.1 Update `UIAnimationManager.Awake` to handle singleton persistence correctly for Unity 6.
- [x] 4.2 Add debug logging to track the exact moment of registration and discovery for better troubleshooting.

## 5. Verification

- [ ] 5.1 Verify that the Game Over panel appears even if the object was disabled in the hierarchy.
- [ ] 5.2 Verify that the game loop continues normally if the `UIAnimationManager` is missing from the scene.
- [ ] 5.3 Verify that the survival time and kills are displayed correctly on the re-discovered UI.
