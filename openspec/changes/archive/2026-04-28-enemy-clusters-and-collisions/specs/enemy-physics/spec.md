## ADDED Requirements

### Requirement: Enemy-to-Enemy Collision
Los enemigos SHALL poseer colisionadores físicos que les impidan atravesarse entre sí.

#### Scenario: Colisión lateral
- **WHEN** dos enemigos intentan ocupar la misma posición
- **THEN** el motor de físicas de Unity SHALL aplicar una fuerza de separación proporcional a su movimiento

### Requirement: Momentum and Friction
Los enemigos SHALL tener inercia física pero con una fricción (drag) suficiente para no patinar excesivamente.

#### Scenario: Parada tras empujón
- **WHEN** un enemigo es empujado por otro
- **THEN** su velocidad SHALL reducirse gradualmente hasta detenerse o retomar su ruta gracias al `Linear Drag`
