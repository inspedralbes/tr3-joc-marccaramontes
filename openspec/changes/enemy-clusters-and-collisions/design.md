## Context

Actualmente los enemigos se solapan al moverse y el spawner los genera uno a uno. Esto reduce la sensación de amenaza masiva.

## Goals / Non-Goals

**Goals:**
- Generar grupos de 3 a 5 enemigos simultáneamente.
- Impedir que los enemigos ocupen el mismo espacio físico mediante colisiones.
- Aumentar el número total de enemigos por oleada.

**Non-Goals:**
- No se implementarán comportamientos de flanqueo complejos; los enemigos seguirán siendo "individuales" en su lógica de búsqueda.
- No se añadirá un sistema de vida (HP) en esta fase (mantener 1-hit kill).

## Decisions

### 1. Físicas de Enemigos
**Decisión**: Añadir `Rigidbody2D` (Dynamic) y `CircleCollider2D` (No trigger) a los enemigos.
**Razón**: Permite que los enemigos se empujen automáticamente usando el motor de físicas de Unity.
**Configuración**: `Gravity Scale = 0`, `Linear Drag = 2.0` para evitar deslizamientos infinitos.

### 2. Spawneo de Clústers
**Decisión**: Crear una función `SpawnCluster(int size)` en `EnemySpawner.cs`.
**Razón**: Agrupa el código de instanciación para ser llamado en bucle cuando el sistema de oleadas lo requiera.

### 3. Aumento de Cantidad
**Decisión**: Modificar `WaveConfig` para duplicar aproximadamente el número de enemigos por ronda.

## Risks / Trade-offs

- **[Riesgo] Atascos** → Los enemigos podrían bloquearse en pasillos o contra el jugador.
  - *Mitigación*: Asegurar que el radio del collider sea ligeramente inferior al tamaño del sprite.
- **[Riesgo] Rendimiento** → Muchos Rigidbody2D activos pueden afectar los FPS.
  - *Mitigación*: Los enemigos están en una capa de físicas optimizada.
