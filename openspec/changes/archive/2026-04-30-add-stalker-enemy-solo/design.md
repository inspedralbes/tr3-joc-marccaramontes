## Context

El sistema de enemigos actual en `Enemy.cs` maneja persecución directa (`Basic`) e interceptación (`Interceptor`). El juego es un arcade frenético donde la muerte es instantánea. No existe actualmente un sistema de ataque a distancia para los enemigos ni una forma de sincronizar acciones puntuales (como disparos) en red sin saturar la posición.

## Goals / Non-Goals

**Goals:**
- Implementar el enemigo `Stalker` con una IA de órbita y acoso dinámico.
- Crear un sistema de proyectiles enemigos (`EnemyBullet`) sincronizado en red.
- Refactorizar `Enemy.cs` para manejar múltiples comportamientos de forma limpia.
- Mantener la coherencia visual con el estilo Neon (Naranja para amenazas externas).

**Non-Goals:**
- No implementar sistemas de salud para el jugador (mantiene One-Hit-Kill).
- No añadir sistemas de armas complejos o inventarios para los enemigos.
- No modificar el sistema de red base, solo extenderlo con nuevos eventos.

## Decisions

### 1. IA de Órbita y Acoso (Vector-Based)
- **Decisión:** Calcular la trayectoria del Stalker usando la perpendicular del vector al jugador combinada con una fuerza radial de corrección.
- **Razón:** Permite un movimiento fluido y orgánico que rodea al jugador sin las limitaciones de un sistema de nodos o navegación compleja.
- **Alternativas:** `NavMesh` (demasiado pesado para este estilo de juego) o `A*` (innecesario en espacio abierto).

### 2. Sincronización de Disparos (Host Authority)
- **Decisión:** El Host calcula el timer de disparo. Al disparar, emite un evento `ENEMY_SHOOT` vía `NetworkManager`. Los clientes instancian la bala localmente con la trayectoria recibida.
- **Razón:** Minimiza el tráfico de red comparado con sincronizar la posición de cada bala cada frame. La detección de colisión local asegura que el jugador sienta que la muerte es justa.

### 3. Refactorización de `Enemy.cs`
- **Decisión:** Separar el `FixedUpdate` en `UpdateMovement()` y `HandleShooting()`. Usar un método `InitializeVisuals()` para configurar los colores neon dinámicamente.
- **Razón:** Mejora la legibilidad y facilita la adición de futuros tipos de enemigos sin crear un "God Script".

### 4. Jerarquía de Prefabs y Visuales
- **Stalker:** Variante del prefab `Enemy` con `type = EnemyType.Stalker` y color naranja neon (`_OutlineColor` x 15).
- **EnemyBullet:** Prefab con `SpriteRenderer`, `CircleCollider2D` (Trigger) y el script `EnemyBullet.cs`. Usa el shader `Custom/SpriteOutline` con color naranja.

## Máquina de Estados del Stalker

```ascii
    [ ACERCÁNDOSE ] ──── (Distancia > 10) ───▶ [ ORBITANDO ]
           ▲                                       │
           │                                       ▼
    [ ALEJÁNDOSE ] ◄──── (Distancia < 7) ──── [ DISPARANDO ]
           ▲                                       │
           └───────────────────────────────────────┘
```

## Risks / Trade-offs

- **[Riesgo] Balas fuera de pantalla** → El Stalker dispara desde la periferia.
    - **Mitigación:** Velocidad de proyectil moderada (6-8 units/s) y un pequeño "flash" naranja en el enemigo antes de disparar.
- **[Riesgo] Latencia en Multijugador** → El jugador puede morir por una bala que "no vio" venir por lag.
    - **Mitigación:** La detección de colisión se realiza en el cliente del jugador afectado para garantizar respuesta inmediata.
