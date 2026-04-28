## ADDED Requirements

### Requirement: Lobby Player Tracking
El sistema SHALL realizar un seguimiento de todos los jugadores conectados a una sala y sincronizar sus nombres con todos los clientes.

#### Scenario: Sincronización al unirse
- **WHEN** Un nuevo jugador se une a la sala mediante socket
- **THEN** El servidor SHALL emitir un evento `PLAYER_JOINED` incluyendo la lista completa de jugadores actuales a todos los miembros de la sala.

### Requirement: Visualización de Lista de Jugadores
El Lobby SHALL mostrar una lista legible de los nombres de los jugadores que están actualmente en la sala de espera.

#### Scenario: Actualización de UI
- **WHEN** El cliente recibe un evento `PLAYER_JOINED` o `PLAYER_LEFT`
- **THEN** El `LobbyController` SHALL actualizar el componente de texto o lista en el `WaitingPanel` para reflejar el estado actual de la sala.
