## 1. Preparación del Sistema de Juego

- [x] 1.1 Modificar `GameManager.cs` para permitir el acceso público a `isGameOver` mediante una propiedad.

## 2. Refactorización del EnemySpawner

- [x] 2.1 Actualizar las variables de `EnemySpawner.cs`: añadir `minSpawnRadius`, `maxSpawnRadius`, y `spawnCenter`.
- [x] 2.2 Modificar el método `Update` para añadir la comprobación de `GameManager.Instance.IsGameOver`.
- [x] 2.3 Reimplementar `SpawnEnemy` para calcular la posición usando el rango de radios y el centro de referencia.
- [x] 2.4 Implementar `OnDrawGizmos` para dibujar los círculos de previsualización en el editor.

## 3. Validación

- [x] 3.1 Verificar en el editor de Unity que los Gizmos se dibujan correctamente.
- [x] 3.2 Probar en Play Mode que los enemigos aparecen aleatoriamente en la corona circular.
- [x] 3.3 Confirmar que el spawneo se detiene cuando el jugador muere.
