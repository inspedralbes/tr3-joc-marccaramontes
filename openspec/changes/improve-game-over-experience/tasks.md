## 1. UI Animation & Effects Infrastructure

- [x] 1.1 Create `UIAnimationManager.cs` to handle CanvasGroup fades and numeric tickers.
- [x] 1.2 Create `ButtonHoverEffect.cs` for responsive button scaling (1.1x) and color shifts.
- [x] 1.3 Create the `PixelExplosion` particle prefab (square particles, short lifetime).

## 2. GameManager & State Logic (Unity 6 Optimized)

- [x] 2.1 Update `GameManager.cs` to handle local score persistence (Save/Load Best Time).
- [x] 2.2 Update `GameManager.ProcessDeath` to trigger immediate `Time.timeScale = 0`.
- [x] 2.3 Implement the `DeathTransition` coroutine in `GameManager` to sequence: Flash -> Wait -> UI Fade -> Ticker.
- [x] 2.4 Update `GameManager.RegisterResultsUI` to attach the new hover components to buttons.
- [x] 2.5 Add a null check for `UIAnimationManager.Instance` in `GameManager` and log a warning if missing.

## 3. Player & Scene Integration (Unity 6 Core)

- [x] 3.1 Update `PlayerMovement.cs` to instantiate the `PixelExplosion` and disable its sprite on death.
- [x] 3.2 **REMOVE** legacy `Input.GetAxis` fallbacks in `PlayerMovement.cs` to prevent Unity 6 exceptions.
- [x] 3.3 **REMOVE** legacy `Input.GetButton` and `Input.mousePosition` calls in `PlayerShooting.cs`.
- [x] 3.4 Update `PlayerShooting.Shoot` to use `Mouse.current.position.ReadValue()` for 2D aiming.
- [x] 3.5 Ensure `Bullet.cs` and `Enemy.cs` are cleaned up correctly upon the `DeathTransition` trigger.

## 4. UI Layout & Tooling (Modern EventSystem)

- [x] 4.1 Update `SceneSetupHelper.cs` to configure the new UI hierarchy (CanvasGroup, Flash overlay).
- [x] 4.2 Update `SceneSetupHelper.cs` to use `InputSystemUIInputModule` for the `EventSystem`.
- [x] 4.3 Update `SceneSetupHelper.cs` to ensure an `_Managers` object with `UIAnimationManager` exists in the scene.
- [ ] 4.4 Run the `Tools/Setup HUD and Results UI` menu item to update the scene.
- [x] 4.5 Style the `PanelResultados` with high-contrast, "Devil Daggers" inspired colors (Black, White, Red).

## 5. Verification

- [ ] 5.1 Verify that the player "explodes" into pixels on death.
- [ ] 5.2 Verify that the screen flashes and the simulation freezes instantly.
- [ ] 5.3 Verify that the results panel fades in and the time "counts up" rapidly.
- [ ] 5.4 Verify that "Best Time" persists after restarting the game.
- [ ] 5.5 Verify that buttons respond to hover with a snap scaling effect.
- [ ] 5.6 **Unity 6 Check**: Confirm no `InvalidOperationException` is thrown when moving or shooting.
- [ ] 5.7 **Unity 6 Check**: Confirm UI buttons are clickable with the new `InputSystemUIInputModule`.
