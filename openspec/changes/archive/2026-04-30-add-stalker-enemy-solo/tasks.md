## 1. Infraestructura y Visuales

- [x] 1.1 Crear el script `EnemyBullet.cs` con lógica de movimiento, tiempo de vida y colisión (`Die()`) con el jugador.
- [x] 1.2 Crear el prefab `EnemyBullet` en Unity con `SpriteRenderer`, `CircleCollider2D` (Trigger) y el shader `Custom/SpriteOutline` configurado en Naranja Neon.
- [x] 1.3 Actualizar `NetworkManager.cs` para incluir el evento `ENEMY_SHOOT` y su correspondiente DTO `EnemyShootData`.
- [x] 1.4 Modificar `GameManager.CleanupScene()` para asegurar la destrucción de proyectiles enemigos al reiniciar la partida.

## 2. Refactorización e IA del Stalker

- [x] 2.1 Actualizar el enum `EnemyType` en `Enemy.cs` para incluir `Stalker`.
- [x] 2.2 Refactorizar `Enemy.cs`: Extraer lógica visual a `InitializeVisuals()` y lógica de movimiento a `UpdateMovement()`.
- [x] 2.3 Implementar la lógica de órbita y acoso en `Enemy.UpdateMovement()` para el tipo `Stalker`.
- [x] 2.4 Implementar `HandleShooting()` en `Enemy.cs`: Solo el Host/Solo dispara, emite el evento de red y spawnea la bala localmente.
- [x] 2.5 Configurar el color Naranja Neon (`_OutlineColor` x 15) para el Stalker en `InitializeVisuals()`.

## 3. Sistema de Oleadas por Pesos

- [x] 3.1 Refactorizar `EnemySpawner.SpawnIndividualInCluster` para seleccionar el tipo de enemigo basado en probabilidades (pesos) en lugar de un simple if/else.
- [x] 3.2 Actualizar `WaveConfig` en `EnemySpawner.cs` para incluir un campo opcional de probabilidad para Stalkers.
- [x] 3.3 Ajustar la configuración de oleadas para que los Stalkers aparezcan gradualmente a partir de la oleada 3.

## 4. Validación y Pulido

- [x] 4.1 Realizar prueba de juego en modo Solo para validar el balance de velocidad de bala y fire rate del Stalker.
- [x] 4.2 Probar la sincronización en Multijugador: Verificar que los clientes instancien las balas correctamente cuando el Host dispara.
- [x] 4.3 Asegurar que el Stalker deje de disparar cuando el jugador muere o entra en modo espectador.
