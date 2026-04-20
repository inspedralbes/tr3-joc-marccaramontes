## Why

El juego actualmente carece de una estructura de navegación y modos de juego competitivos. Añadir un menú principal y un modo multijugador por turnos (hotseat) permitirá una experiencia de juego más completa y fomentará la competitividad entre usuarios locales, dándole un propósito claro a la mecánica de supervivencia.

## What Changes

- **Nueva Escena de Menú**: Creación de una escena inicial con botones para seleccionar el modo de juego.
- **Modo Solo**: El modo de juego actual, donde el jugador intenta sobrevivir el mayor tiempo posible.
- **Modo Multijugador (Turnos)**: Un nuevo modo donde el Jugador 1 juega su turno, seguido del Jugador 2. Al final, el sistema compara los tiempos y declara un ganador.
- **Refactor de GameManager**: Actualización del manager para persistir datos entre cambios de escena y gestionar la lógica de turnos.
- **Pantalla de Resultados**: Visualización de los tiempos finales y el ganador al concluir ambos turnos en el modo multijugador.

## Capabilities

### New Capabilities
- `game-modes`: Define la lógica para alternar entre el modo de un solo jugador y el modo competitivo por turnos, incluyendo la gestión de estados globales.

### Modified Capabilities
<!-- No requirement changes for existing capabilities -->

## Impact

- **Escenas**: `Assets/Scenes/Menu.unity` (nueva) y actualización de la carga de `Assets/Scenes/SampleScene.unity`.
- **Scripts**: `Assets/GameManager.cs` (refactor mayor para persistencia y turnos), creación de `Assets/MenuController.cs`.
- **UI**: Uso de `Canvas`, `Button` y `TextMeshPro` para el menú y los resultados.
