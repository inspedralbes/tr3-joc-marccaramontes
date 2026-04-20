## ADDED Requirements

### Requirement: Detección de Colisión Proyectil-Enemigo
El sistema SHALL detectar físicamente el contacto entre el proyectil del jugador y los enemigos.

#### Scenario: Activación de Trigger
- **WHEN** el colisionador de la bala entra en el colisionador del enemigo
- **THEN** el motor de física de Unity dispara el evento `OnTriggerEnter2D` en el script `Bullet.cs`
