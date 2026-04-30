## Why

El juego actualmente tiene las direcciones del servidor (IP/localhost) grabadas directamente en el código de `NetworkManager.cs`. Esto impide que jugadores externos se conecten a un servidor específico sin recompilar el juego, causando errores de "cannot connect to destination host" en builds distribuidas.

## What Changes

- **Configuración Dinámica de Servidor**: Añadir la capacidad de especificar la dirección IP o el Host del servidor desde la interfaz de usuario (Lobby).
- **Persistencia de Dirección**: Guardar la última dirección utilizada en `PlayerPrefs` para que no sea necesario escribirla cada vez.
- **Unificación de Puerto vía Gateway**: Configurar el cliente para que todas las comunicaciones (HTTP y WebSockets) se realicen a través del Gateway (puerto 3000), simplificando la configuración de red y firewalls.

## Capabilities

### New Capabilities
- `server-configuration`: Permite al usuario definir y persistir la ubicación del servidor de juego.

### Modified Capabilities
- `network-core`: Se modifica el flujo de inicialización de conexión para aceptar parámetros de dirección dinámicos en lugar de valores estáticos.

## Impact

- **Client (Unity)**: `NetworkManager.cs` (gestión de URLs), `LobbyController.cs` (interfaz de usuario), `NativeWebSocketClient.cs` (conexión dinámica).
- **UI**: Nueva escena o campo de texto en `Lobby` para introducir la IP.
- **Assets**: Se requiere un nuevo `TMP_InputField` en la escena de Lobby.
