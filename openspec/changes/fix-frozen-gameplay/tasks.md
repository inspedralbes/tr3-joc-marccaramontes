## 1. Fix GameManager State

- [x] 1.1 Restaurar la lógica de "Safe Start" en `Assets/GameManager.cs` dentro del método `Start()`.
- [x] 1.2 Asegurar que el estado cambie a `GameState.Playing` si se detecta la escena `SampleScene`.

## 2. Fix Player Authority

- [x] 2.1 Actualizar `Assets/PlayerMovement.cs` para auto-asignar `isLocalPlayer = true` si no hay sesión de red activa.
- [x] 2.2 Añadir validación y `Debug.LogWarning` si el componente `PlayerInput` o la acción "Move" no se encuentran.

## 3. Validation

- [x] 3.1 Probar el inicio del juego desde la escena `Menu` (Modo Solo).
- [x] 3.2 Probar el inicio del juego directamente desde la escena `SampleScene` en el Editor.
- [x] 3.3 Verificar que el personaje se mueve y el tiempo corre en ambos casos.
