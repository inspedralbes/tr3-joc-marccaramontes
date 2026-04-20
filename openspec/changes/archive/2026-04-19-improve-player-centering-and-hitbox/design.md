## Context

El juego carece de un sistema de inicialización posicional robusto para el jugador y de feedback visual sobre su hitbox. Actualmente, la muerte por "caída de plataforma" se calcula basándose en la distancia al origen (0,0), lo que limita la flexibilidad del diseño de niveles.

## Goals / Non-Goals

**Goals:**
- Asegurar que el jugador siempre inicie en el centro visual de la plataforma.
- Proporcionar feedback visual claro (azul) del hitbox del jugador.
- Mejorar la visualización del área de juego en el Editor mediante Gizmos.

**Non-Goals:**
- No se cambiarán las mecánicas de movimiento actuales.
- No se implementarán hitboxes poligonales complejos; se mantiene el círculo.
- No se añadirán animaciones de muerte nuevas en este cambio.

## Decisions

### 1. Referencia a la Plataforma
- **Decisión**: Añadir un campo `public Transform platformCenter` en `PlayerMovement`.
- **Razón**: Permite mover la plataforma en el Editor sin romper la lógica de `CheckBounds`. Si es null, se usará `Vector3.zero` como fallback.

### 2. Visualización del Hitbox
- **Decisión**: Usar un objeto hijo del Player con un `SpriteRenderer` circular azul semitransparente.
- **Razón**: Es más performante y fácil de estilizar que un `LineRenderer` para formas circulares simples en 2D.
- **Alternativa considerada**: `OnDrawGizmos` (Solo Editor). Se descartó como única opción porque el usuario pidió "remarcar el hitbox", lo que implica visibilidad en gameplay.

### 3. Implementación de Gizmos
- **Decisión**: Implementar `OnDrawGizmos` en `PlayerMovement` para dibujar el radio de la plataforma y el hitbox.
- **Razón**: Mejora drásticamente la experiencia de diseño de niveles sin coste de rendimiento en el build final.

## Risks / Trade-offs

- **[Riesgo]** El Sprite del hitbox puede solaparse con otros elementos visuales. 
  - **Mitigación**: Ajustar el `Sorting Order` del Sprite del hitbox para que esté por encima del jugador pero con alta transparencia.
- **[Riesgo]** Si se olvida asignar `platformCenter`, el jugador aparecerá en (0,0).
  - **Mitigación**: Añadir un chequeo en `Start()` que busque un objeto con el Tag "Platform" si la referencia está vacía.
