## 1. Refactor de NetworkManager

- [x] 1.1 Modificar `NetworkManager.cs` para añadir el método `UpdateServerAddress(string host)` que reconstruya las URLs dinámicamente.
- [x] 1.2 Actualizar las URLs por defecto para que apunten al puerto 3000 (Gateway) tanto para HTTP (`/api`) como para WS (`/ws`).
- [x] 1.3 Implementar lógica en `NetworkManager.cs` para leer la dirección desde `PlayerPrefs` en `Awake`.

## 2. Actualización de Interfaz (UI)

- [x] 2.1 En la escena `Lobby`, añadir un `TMP_InputField` para la dirección del servidor (Server Address).
- [x] 2.2 Vincular el nuevo InputField en `LobbyController.cs`.
- [x] 2.3 Implementar en `LobbyController.cs` la carga inicial del valor guardado y la actualización del `NetworkManager` al cambiar el texto.

## 3. Validación y Persistencia

- [x] 3.1 Asegurar que el valor del InputField se guarde en `PlayerPrefs` cuando el usuario inicie una acción (Crear o Unirse).
- [x] 3.2 Verificar que el WebSocket conecte correctamente a la ruta `/ws` del Gateway (Puerto 3000).
- [x] 3.3 Realizar una prueba de conexión manual usando una IP local en lugar de localhost.
