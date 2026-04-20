## 1. Backend Setup (Node.js)

- [x] 1.1 Inicializar proyecto Node.js en una nueva carpeta `Server/`.
- [x] 1.2 Instalar dependencias: `express`, `socket.io`.
- [x] 1.3 Implementar servidor base con soporte para salas (rooms).
- [x] 1.4 Crear eventos básicos: `create_room`, `join_room`, `disconnect`.

## 2. Unity Network Infrastructure

- [x] 2.1 Importar o implementar cliente de Socket.io en Unity.
- [x] 2.2 Crear `NetworkManager.cs` como Singleton persistente.
- [x] 2.3 Implementar métodos de conexión y desconexión en `NetworkManager`.
- [x] 2.4 Crear `NetworkIdentity.cs` para identificar objetos en red.

## 3. World & State Synchronization

- [x] 3.1 Refactorizar `PlayerMovement.cs` para enviar actualizaciones de posición por red.
- [x] 3.2 Implementar lógica de "Ghost Player" para representar a otros jugadores.
- [x] 3.3 Modificar `EnemySpawner.cs` para que solo el Host tenga autoridad de spawn.
- [x] 3.4 Sincronizar disparos y muertes a través de eventos de red.

## 4. UI & Flow Integration

- [x] 4.1 Crear escena de `Lobby` con campos para Room ID y botones de Join/Create.
- [x] 4.2 Actualizar `MenuController.cs` para navegar a la nueva escena de Lobby.
- [x] 4.3 Implementar pantalla de espera mientras se aguarda al segundo jugador.
- [x] 4.4 Sincronizar el inicio de la partida (`SampleScene`) para todos los clientes en la sala.

## 5. Validation & Polishing

- [x] 5.1 Realizar pruebas de conexión local con dos instancias de Unity.
- [x] 5.2 Implementar interpolación simple para suavizar el movimiento de los Ghost Players.
- [x] 5.3 Verificar el envío de resultados finales al servidor.
