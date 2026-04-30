## ADDED Requirements

### Requirement: Selección de Modo de Juego
El sistema SHALL permitir al usuario elegir entre el modo "Solo" y el modo "Multiplayer" desde el menú principal.

#### Scenario: Elección de modo solo
- **WHEN** el usuario pulsa el botón "Solo"
- **THEN** se carga la escena de juego y se inicia el contador de tiempo normal

#### Scenario: Elección de modo multijugador
- **WHEN** el usuario pulsa el botón "Multiplayer"
- **THEN** se carga la escena de juego y se activa la secuencia de turnos para el Jugador 1

### Requirement: Gestión de Turnos Hotseat
El sistema SHALL alternar el control entre el Jugador 1 y el Jugador 2 en el modo multijugador tras la muerte del primero.

#### Scenario: Fin del turno del Jugador 1
- **WHEN** el Jugador 1 muere en modo multijugador
- **THEN** el sistema guarda el tiempo de supervivencia del Jugador 1 y reinicia la escena informando que es el turno del Jugador 2

### Requirement: Comparación de Resultados
El sistema SHALL comparar los tiempos de ambos jugadores al finalizar el turno del Jugador 2 y declarar un ganador.

#### Scenario: Declaración de ganador
- **WHEN** el Jugador 2 muere en modo multijugador
- **THEN** el sistema muestra una pantalla con los tiempos de ambos jugadores y resalta al que sobrevivió más tiempo
