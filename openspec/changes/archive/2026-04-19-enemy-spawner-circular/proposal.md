## Why

El sistema de spawneo actual es estático y limitado. Los enemigos siempre aparecen en un radio fijo de 10 unidades respecto al origen (0,0), lo que no se adapta si el jugador o la plataforma se mueven. Además, carece de visualización en el editor, dificultando el ajuste de la dificultad y el área de juego.

## What Changes

- **Spawneo Dinámico**: Implementación de un rango de spawneo (radio mínimo y máximo) para evitar patrones predecibles.
- **Referencia Espacial**: El área de spawneo ahora seguirá a un transform de referencia (como la plataforma o el jugador).
- **Visualización (Gizmos)**: Adición de círculos visuales en el editor de Unity para previsualizar el área de aparición.
- **Seguridad**: El spawner dejará de crear enemigos automáticamente cuando el juego termine (GameManager.isGameOver).

## Capabilities

### New Capabilities
- `circular-enemy-spawner`: Define las reglas de aparición de enemigos en una corona circular aleatoria con soporte para visualización en el editor.

### Modified Capabilities
- Ninguna.

## Impact

- **Assets/EnemySpawner.cs**: Reescritura total o parcial de la lógica de posicionamiento y adición de variables de configuración.
- **Assets/GameManager.cs**: Requiere acceso público a la variable `isGameOver` para que el spawner pueda consultarla.
- **Gameplay**: Los enemigos aparecerán de forma más orgánica y siempre fuera de la zona segura de la plataforma.
