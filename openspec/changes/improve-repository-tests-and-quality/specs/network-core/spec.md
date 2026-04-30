## MODIFIED Requirements

### Requirement: Robust JSON Message Envelope
All communication between client and server SHALL follow a standard JSON envelope: `{"type": "EVENT_NAME", "payload": "JSON_STRING_PAYLOAD"}`.

#### Scenario: Error response handling in Unity
- **WHEN** el servidor devuelve una respuesta de error (4xx o 5xx) a una petición HTTP
- **THEN** el `NetworkManager` de Unity SHALL parsear el cuerpo del error y activar un evento de UI para informar al jugador de forma comprensible

### Requirement: Server Connection
The Unity client MUST be able to establish and maintain a connection with the Node.js server via Native WebSockets. The connection address MUST be resolvable dynamically from user input or configuration.

#### Scenario: Reconnection attempt on socket failure
- **WHEN** la conexión WebSocket se pierde inesperadamente durante la ejecución
- **THEN** el cliente SHALL intentar reconectar automáticamente hasta 3 veces antes de forzar el retorno al Menú Principal
