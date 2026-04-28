## 1. Servidor: Estandarización y Lista de Jugadores

- [x] 1.1 Modificar `Server/game-service/services/GameService.js` para incluir la lista completa de jugadores en el payload de `ROOM_JOINED_CONFIRMED`.
- [x] 1.2 Asegurar que `handleJoinRoom` devuelva un array de strings con los `playerName` de la sala actual.
- [x] 1.3 Revisar `Server/game-service/controllers/SocketController.js` para asegurar que todos los casos del switch usen eventos en MAYÚSCULAS (`SPAWN_ENEMY`, `SHOOT`, `MOVE`, etc.).

## 2. Unity: Protocolo y Lobby UI

- [x] 2.1 Actualizar los DTOs en `Assets/Networking/NetworkManager.cs` para manejar el nuevo formato de `RoomConfirmedData` (con array de jugadores).
- [x] 2.2 Modificar `Assets/Networking/LobbyController.cs` para recibir la lista de jugadores y actualizar un campo de texto en el `WaitingPanel`.
- [x] 2.3 Estandarizar los nombres de eventos en `Assets/EnemySpawner.cs` (cambiar `spawn_enemy` por `SPAWN_ENEMY`).
- [x] 2.4 Estandarizar los nombres de eventos en `Assets/PlayerShooting.cs` (cambiar `player_shoot` por `SHOOT`).

## 3. Unity: Visibilidad y Escena

- [x] 3.1 Actualizar `Assets/Editor/SceneSetupHelper.cs` para incluir la creación del objeto `RemotePlayerManager` en la escena `SampleScene` si no existe.
- [x] 3.2 Modificar `Assets/Networking/RemotePlayerManager.cs` para desactivar el script `PlayerMovement` (y otros si aplica) en los prefabs instanciados para otros jugadores, evitando conflictos de autoridad local.
- [ ] 3.3 Ejecutar el `SceneSetupHelper` desde el menú de Unity para aplicar los cambios a la escena actual.

## 4. Validación

- [ ] 4.1 Iniciar el servidor y verificar que el Lobby muestra correctamente "Jugadores: [Nombre1, Nombre2]".
- [ ] 4.2 Probar con dos instancias de Unity y verificar que los jugadores se ven y se mueven correctamente.
- [ ] 4.3 Confirmar que el segundo jugador recibe los enemigos generados por el Host y ve sus disparos.
