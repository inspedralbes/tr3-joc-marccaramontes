## ADDED Requirements

### Requirement: Introducción de Dirección de Servidor
El sistema SHALL permitir al usuario introducir una dirección de servidor (IP o Hostname) mediante un campo de texto en la interfaz del Lobby.

#### Scenario: Usuario introduce IP válida
- **WHEN** el usuario escribe "192.168.1.15" en el campo de dirección del servidor
- **THEN** el sistema debe almacenar este valor internamente para su uso en las conexiones

### Requirement: Persistencia de Configuración de Servidor
El sistema SHALL guardar la última dirección de servidor utilizada en el almacenamiento local (`PlayerPrefs`) y cargarla automáticamente al iniciar la escena del Lobby.

#### Scenario: Carga automática al iniciar el Lobby
- **WHEN** la escena del Lobby se carga
- **THEN** el campo de dirección del servidor debe mostrar el valor guardado previamente (o "localhost" si no existe ninguno)

### Requirement: Construcción Dinámica de URLs
El sistema SHALL construir las URLs de conexión (HTTP y WebSocket) dinámicamente utilizando la dirección proporcionada por el usuario y el puerto del Gateway (3000).

#### Scenario: Generación de URLs para IP específica
- **WHEN** la dirección del servidor es "mi-servidor.com"
- **THEN** la URL HTTP debe ser "http://mi-servidor.com:3000/api" y la URL WS debe ser "ws://mi-servidor.com:3000/ws"
