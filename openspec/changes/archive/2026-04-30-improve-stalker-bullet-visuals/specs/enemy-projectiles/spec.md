## MODIFIED Requirements

### Requirement: Proyectiles Enemigos
El sistema SHALL permitir que los enemigos disparen proyectiles que dañen al jugador al contacto.

#### Scenario: Visuales de Amenaza
- **WHEN** se instancia un proyectil enemigo
- **THEN** SHALL tener un color Neon Naranja intenso (HDR) que supere el umbral del Bloom (threshold 0.9) para generar un resplandor visible, y ser visualmente distinto a los proyectiles del jugador.

#### Scenario: Daño al Jugador
- **WHEN** un proyectil enemigo colisiona con el jugador
- **THEN** SHALL llamar al método `Die()` del jugador, finalizando la partida.

#### Scenario: Velocidad y Tiempo de Vida
- **WHEN** un proyectil enemigo se desplaza
- **THEN** SHALL tener una velocidad constante (ej. 5 uds/s) y destruirse automáticamente tras un tiempo de vida (ej. 5s) para evitar sobrecarga de memoria.
