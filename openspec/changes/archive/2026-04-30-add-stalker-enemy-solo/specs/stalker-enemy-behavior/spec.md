## ADDED Requirements

### Requirement: Enemigo Stalker
El sistema SHALL incluir un nuevo tipo de enemigo llamado Stalker que utiliza una IA de mantenimiento de distancia en lugar de persecución directa.

#### Scenario: Movimiento de Órbita
- **WHEN** un Stalker se encuentra a la distancia ideal (aprox. 14 unidades) del jugador
- **THEN** SHALL moverse lateralmente (perpendicular a la dirección del jugador) para orbitar alrededor del centro.

#### Scenario: Ajuste de Distancia
- **WHEN** el jugador se acerca a menos de 12 unidades del Stalker
- **THEN** el Stalker SHALL alejarse del jugador hasta recuperar la distancia de seguridad.

### Requirement: Ataque a Distancia (Stalker)
El Stalker SHALL disparar proyectiles dirigidos hacia la posición actual del jugador a intervalos regulares.

#### Scenario: Disparo Periódico
- **WHEN** el temporizador de disparo del Stalker (3-5s) expira
- **THEN** SHALL instanciar un proyectil enemigo en su posición actual dirigido hacia el jugador.
