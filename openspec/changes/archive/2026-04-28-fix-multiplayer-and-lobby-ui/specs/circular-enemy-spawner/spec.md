## ADDED Requirements

### Requirement: Estandarización de Eventos de Red
Todos los eventos de spawneo de enemigos y acciones de combate (disparos) SHALL usar nombres de eventos en MAYÚSCULAS para garantizar la compatibilidad entre Unity (C#) y el Servidor (Node.js).

#### Scenario: Spawneo sincronizado
- **WHEN** El Host genera un enemigo
- **THEN** Emite el evento `SPAWN_ENEMY` (exactamente en mayúsculas) al servidor.

#### Scenario: Disparo sincronizado
- **WHEN** El jugador dispara
- **THEN** Emite el evento `SHOOT` al servidor para que sea retransmitido.

### Requirement: Sincronización de Enemigos en Clientes
Los jugadores que no son Host SHALL ver los enemigos generados por el Host en las mismas posiciones.

#### Scenario: Recepción de enemigo remoto
- **WHEN** Un cliente recibe un evento `SPAWN_ENEMY`
- **THEN** Instancia el prefab de enemigo en la posición recibida y le asigna el ID de red correspondiente.
