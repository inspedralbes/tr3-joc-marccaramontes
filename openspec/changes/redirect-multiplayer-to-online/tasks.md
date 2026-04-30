## 1. Modificaciones de Código

- [x] 1.1 Actualizar `MenuController.cs`: Redirigir el método `PlayMultiplayer()` a la escena `Lobby`.
- [x] 1.2 Actualizar `MenuController.cs`: (Opcional) Unificar `PlayOnline()` y `PlayMultiplayer()` para que compartan la misma lógica de carga de escena.
- [x] 1.3 Limpiar referencias a `GameMode.Multiplayer` en el `MenuController.cs` que ya no sean necesarias.

## 2. Validación de Flujo

- [ ] 2.1 Verificar en Unity que el botón de Multijugador carga correctamente la escena de Lobby.
- [ ] 2.2 Confirmar que al iniciar desde el Lobby, el `NetworkManager` y el `GameManager` entran correctamente en el modo Online.
