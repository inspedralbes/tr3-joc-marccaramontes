## Context

El sistema actual de enemigos (`Enemy.cs`) y spawneo (`EnemySpawner.cs`) es rudimentario. El enemigo siempre apunta al `player.position`. El spawner usa un temporizador simple que decrementa ligeramente. No hay concepto de "progreso" más allá del tiempo acumulado en el `GameManager`.

## Goals / Non-Goals

**Goals:**
- Implementar una lógica de predicción de trayectoria para el enemigo Interceptor.
- Crear una estructura de oleadas (Waves) con configuración de intensidad.
- Centralizar el escalado de velocidad para que afecte a todos los enemigos activos.
- Mantener la compatibilidad con el modo multijugador (sincronización de spawneo).

**Non-Goals:**
- No se implementarán ataques a distancia para enemigos en esta fase.
- No se cambiará el sistema de detección de colisiones actual.
- No se añadirán efectos visuales complejos (partículas) más allá de lo estrictamente necesario para diferenciar enemigos.

## Decisions

### 1. Lógica de Intercepción
**Decisión:** Usar predicción lineal basada en el vector de movimiento actual del jugador.
**Razón:** Es computacionalmente barata y efectiva en un entorno 2D donde el jugador se mueve con `PlayerMovement`.
**Cálculo:** `TargetPosition = PlayerPos + (PlayerVelocity * distanceFactor)`.

### 2. Estructura de Oleadas
**Decisión:** Implementar un sistema de estados en `EnemySpawner.cs`.
- `WaveState.Idle`: Esperando inicio.
- `WaveState.Spawning`: Generando enemigos según la configuración de la oleada actual.
- `WaveState.WaitingForClear`: No spawnea más, pero espera a que mueran los enemigos actuales para avanzar.
- `WaveState.Rest`: Periodo de calma entre oleadas.

### 3. Escalado de Dificultad
**Decisión:** Añadir un `difficultyMultiplier` en `GameManager.cs`.
**Razón:** Permite que cualquier sistema (balas, enemigos, UI) consulte el nivel de reto actual de forma centralizada.

### 4. Diferenciación de Enemigos
**Decisión:** Usar un `enum EnemyType { Basic, Interceptor }` en `Enemy.cs` y modificar el comportamiento en el `Update` basándose en este tipo.
**Razón:** Evita la sobre-arquitectura de múltiples clases para un prototipo rápido, manteniendo la lógica contenida.

## Diagrama de Estados de Oleadas

```text
[Inicio] ──▶ [Rest (5s)] ──▶ [Spawning (Wave X)]
               ▲               │
               │               ▼
          [Rest (5s)] ◀── [WaitingForClear]
```

## Risks / Trade-offs

- **[Riesgo] Intercepción Perfecta** → Si el enemigo predice demasiado bien, el jugador se siente frustrado. 
  - *Mitigación*: Añadir un factor de error aleatorio o limitar el `leadTime` máximo.
- **[Riesgo] Rendimiento** → Muchas oleadas con muchos enemigos pueden saturar la CPU.
  - *Mitigación*: Implementar límites de población máxima en el Spawner.
- **[Riesgo] Desincronización Multijugador** → Las oleadas deben estar perfectamente sincronizadas entre Host y Cliente.
  - *Mitigación*: Solo el Host gestiona el estado de la oleada y emite eventos de "WaveStarted".
