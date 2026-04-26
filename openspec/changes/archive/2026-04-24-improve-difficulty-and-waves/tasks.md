## 1. Sistema Global de Dificultad

- [x] 1.1 Añadir propiedad `difficultyMultiplier` en `GameManager.cs`.
- [x] 1.2 Implementar lógica en `GameManager.Update` para incrementar el multiplicador con el tiempo.

## 2. Refactorización de Enemigos (Interceptor)

- [x] 2.1 Añadir `EnemyType` (Basic, Interceptor) en `Enemy.cs`.
- [x] 2.2 Implementar lógica de intercepción en `Enemy.Update` (predicción de trayectoria).
- [x] 2.3 Aplicar el `difficultyMultiplier` al cálculo de velocidad final en `Enemy.cs`.
- [x] 2.4 Crear una variante del Prefab de Enemigo llamada `InterceptorEnemy` (Lógica visual implementada en Enemy.cs para este tipo).

## 3. Sistema de Oleadas (Wave System)

- [x] 3.1 Añadir `WaveState` y estructuras de configuración de oleada en `EnemySpawner.cs`.
- [x] 3.2 Refactorizar `EnemySpawner.Update` para manejar el flujo de estados (Spawn -> WaitingForClear -> Rest).
- [x] 3.3 Implementar lógica de selección de Prefab (Basic vs Interceptor) según la oleada actual.
- [x] 3.4 Asegurar que la sincronización de red (`spawn_enemy`) incluya el tipo de enemigo si es necesario.

## 4. Validación y Ajustes

- [x] 4.1 Verificar que los enemigos aumentan su velocidad progresivamente.
- [x] 4.2 Probar que los interceptores efectivamente cortan el paso al jugador.
- [x] 4.3 Ajustar los tiempos de calma (Rest) y la intensidad de las oleadas para una curva de dificultad equilibrada.
