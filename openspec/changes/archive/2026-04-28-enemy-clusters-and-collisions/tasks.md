## 1. Físicas de Enemigos

- [x] 1.1 Actualizar `Enemy.cs` con `RequireComponent(typeof(Rigidbody2D))` y `RequireComponent(typeof(CircleCollider2D))`.
- [x] 1.2 Configurar el `Rigidbody2D` en `Awake` (Gravity=0, Drag=2, Interpolation).
- [x] 1.3 Modificar el movimiento en `FixedUpdate` usando `rb.MovePosition` para permitir colisiones físicas.

## 2. Sistema de Clústers y Oleadas

- [x] 2.1 Implementar `SpawnCluster(int size)` en `EnemySpawner.cs`.
- [x] 2.2 Actualizar `HandleWaveStates` para llamar a `SpawnCluster` en lugar de `SpawnEnemy` individual.
- [x] 2.3 Incrementar el recuento de enemigos en `WaveConfig` para todas las oleadas.
- [x] 2.4 Ajustar el tiempo entre clústers para mantener un ritmo desafiante.

## 3. Validación y Ajuste

- [x] 3.1 Verificar que los enemigos se empujan al nacer y no se solapan durante el trayecto.
- [x] 3.2 Comprobar que el número de enemigos por ronda se siente correcto.
- [x] 3.3 Validar que cada enemigo sigue manteniendo su comportamiento individual (seguimiento/intercepción).
