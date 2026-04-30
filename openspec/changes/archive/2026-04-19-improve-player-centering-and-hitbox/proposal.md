## Why

El jugador no aparece centrado en la plataforma al iniciar el juego, lo que genera confusión visual y una experiencia de juego inconsistente. Además, el jugador no tiene una referencia visual de su propia área de colisión (hitbox), lo que dificulta la esquiva precisa de enemigos.

## What Changes

- **Posicionamiento Inicial**: El script `PlayerMovement` ahora forzará la posición del jugador al centro de la plataforma (o al origen (0,0) si no se especifica una plataforma) al iniciar.
- **Visualización de Hitbox**: Se añadirá una representación visual del hitbox del jugador (un círculo azul) que será visible durante el gameplay para ayudar a la precisión.
- **Sistema de Gizmos**: Se añadirán Gizmos en el editor para que el desarrollador vea el radio de muerte/caída y el hitbox sin necesidad de ejecutar el juego.

## Capabilities

### New Capabilities
- `player-visuals`: Mejora de la representación visual del jugador, incluyendo la visualización del hitbox.
- `player-positioning`: Gestión precisa de la ubicación inicial y límites del jugador en la plataforma circular.

### Modified Capabilities
<!-- Dejar vacío si no hay cambios en requerimientos de capacidades existentes -->

## Impact

- `PlayerMovement.cs`: Modificación de `Start` y `CheckBounds`.
- `Assets/Player.prefab` (o el objeto en escena): Adición de un componente visual para el hitbox (Sprite o LineRenderer).
- `GameManager.cs`: Podría requerir una referencia al objeto plataforma para centralizar al jugador dinámicamente.
