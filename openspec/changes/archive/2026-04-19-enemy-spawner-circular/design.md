## Context

El `EnemySpawner` actual usa un radio fijo y no tiene en cuenta el estado de la partida. Esto causa que sigan apareciendo enemigos después de que el jugador pierda, y no permite ajustar el área de spawneo fácilmente en el editor.

## Goals / Non-Goals

**Goals:**
- Implementar un rango de spawneo (min/max radius).
- Hacer que el spawner dependa de un `Transform` de referencia para el centro.
- Detener el spawneo automáticamente al morir el jugador.
- Añadir visualización en el editor (Gizmos).

**Non-Goals:**
- No se cambiará la lógica de movimiento de los enemigos (`Enemy.cs`).
- No se implementarán diferentes tipos de oleadas (waves) en este cambio.

## Decisions

- **Modificación de GameManager**: Cambiar `isGameOver` de `private` a `public` (o añadir una propiedad `IsGameOver { get; }`).
  - *Rationale*: Es la forma más directa de que el spawner sepa cuándo parar en un sistema basado en Singletons.
- **Uso de OnDrawGizmos**: Utilizar `OnDrawGizmos` en `EnemySpawner`.
  - *Rationale*: Permite ver el área de spawneo sin necesidad de darle al Play, facilitando el diseño de niveles.
- **Referencia de Centro**: Añadir un campo `public Transform spawnCenter`. Si es nulo en el Start, se usará el transform del propio objeto.
  - *Rationale*: Flexibilidad para mover el punto de origen de los enemigos.

## Risks / Trade-offs

- **[Risk]** Spawneo dentro de la cámara → **Mitigation**: Ajustar el `minRadius` para que sea siempre mayor que el alcance de la cámara (aprox 8-10 unidades en este proyecto).
- **[Risk]** NRE si el GameManager no existe → **Mitigation**: Usar `if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)`.
