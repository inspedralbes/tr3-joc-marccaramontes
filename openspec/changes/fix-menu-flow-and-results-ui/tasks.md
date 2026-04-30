## 1. Game State Management

- [x] 1.1 Add `GameState` enum to `GameTypes.cs` (Menu, Playing, GameOver).
- [x] 1.2 Update `GameManager` to include a `CurrentState` property and initialize it to `GameState.Menu`.
- [x] 1.3 Update `GameManager.StartGame` to set `CurrentState = GameState.Playing`.
- [x] 1.4 Update `GameManager.ReturnToMenu` to set `CurrentState = GameState.Menu`.
- [x] 1.5 Wrap logic in `GameManager.Update` (survival time) and `GameManager.ProcessDeath` with a check for `CurrentState == GameState.Playing`.
- [x] 1.6 Update `GameManager.ProcessDeath` to transition to `GameState.GameOver` when the second player dies in multiplayer.

## 2. UI Registration & Visibility Correction

- [x] 2.1 Create `ResultsUIRegisterer.cs` to hold references to the results panel and TMP texts.
- [x] 2.2 Implement `RegisterResultsUI(ResultsUIRegisterer ui)` in `GameManager`.
- [x] 2.3 Refactor `GameManager.ShowResults` to use the registered `ResultsUIRegisterer` instead of `GameObject.Find`.
- [x] 2.4 Update `ResultsUIRegisterer.cs` to use `RemoveAllListeners()` on buttons during registration.
- [x] 2.5 Add `Debug.LogError` in `GameManager.ShowResults` if `resultsPanel` is null to detect failures.
- [x] 2.6 Mover el script `ResultsUIRegisterer` de `PanelResultados` a `CanvasResultados` en `SampleScene` y reasignar referencias. (Automatizado vía Tools)

## 3. Session Statistics & Input Control

- [x] 3.1 Añadir `int currentKills` al `GameManager` y el método `AddKill()`.
- [x] 3.2 Modificar `GameManager.ResetSession` para poner a cero el contador de bajas.
- [ ] 3.3 Bloquear disparo: Añadir check de `IsGameOver` en `PlayerShooting.Update`.
- [ ] 3.4 Actualizar la UI de resultados para incluir el texto de "Bajas: X" con sombra y mejor formato.

## 4. UI Aesthetics & Scene Integrity

- [ ] 4.1 Actualizar `SceneSetupHelper` para crear un `EventSystem` si falta en la escena.
- [ ] 4.2 Implementar `VerticalLayoutGroup` en el Panel de Resultados para auto-alineación.
- [ ] 4.3 Mejorar visuales: Colores, sombreado de textos y espaciado proporcional.
- [ ] 4.4 Asegurar que el `GraphicRaycaster` del Canvas esté activo.

## 5. Validation

- [ ] 5.1 Verificar que al morir el ratón ya no dispara proyectiles.
- [ ] 5.2 Confirmar que los botones se pueden pulsar sin que el disparo "traspase" la UI.
- [ ] 5.3 Validar que el aspecto del menú es limpio y profesional.
