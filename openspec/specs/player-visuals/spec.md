## ADDED Requirements

### Requirement: Visualización del Hitbox en Gameplay
El sistema DEBE mostrar un círculo azul semitransparente sobre el jugador para representar su hitbox durante el juego.

#### Scenario: Hitbox visible al iniciar
- **WHEN** el juego comienza y el jugador está en escena
- **THEN** un círculo azul semitransparente es visible centrado en la posición del jugador

### Requirement: Visualización de Hitbox en Editor (Gizmos)
El sistema DEBE dibujar el hitbox del jugador en la vista de Escena del Editor usando Gizmos de color azul.

#### Scenario: Visualización en Scene View
- **WHEN** el objeto Jugador es seleccionado o visible en el Editor
- **THEN** se dibuja un círculo alámbrico azul que coincide con el radio del collider del jugador
