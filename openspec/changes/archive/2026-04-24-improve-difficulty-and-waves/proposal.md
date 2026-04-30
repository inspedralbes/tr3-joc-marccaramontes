## Why

El juego actual tiene un ritmo de dificultad muy lineal y predecible. Los enemigos siempre se mueven directamente hacia el jugador y el spawner simplemente reduce el tiempo entre apariciones. Para que el juego sea más emocionante y requiera más habilidad, necesitamos introducir variedad en los tipos de enemigos, patrones de movimiento más complejos y un sistema de progresión por oleadas que rompa la monotonía.

## What Changes

- **Nuevo Enemigo Interceptor**: Un nuevo tipo de enemigo que predice la posición futura del jugador para cortarle el paso, en lugar de seguirlo directamente.
- **Sistema de Oleadas Marcadas**: Sustitución del goteo constante de enemigos por oleadas con periodos de calma y picos de intensidad.
- **Escalado de Velocidad Progresivo**: Incremento dinámico de la velocidad y agilidad de todos los enemigos a medida que avanzan las oleadas.
- **Multiplicador Global de Dificultad**: Un factor centralizado que afecta a las estadísticas de los enemigos según el tiempo de supervivencia.

## Capabilities

### New Capabilities
- `interceptor-enemy`: Lógica de predicción de trayectoria y comportamiento de intercepción para un nuevo tipo de enemigo.
- `wave-system`: Gestión de estados de oleada (Spawn, Calma, Transición) y configuración de hordas.

### Modified Capabilities
- `enemy-behavior`: Añadir soporte para modificadores de velocidad globales y tipos de enemigos.
- `circular-enemy-spawner`: Modificar el spawner para que funcione bajo demanda del sistema de oleadas en lugar de un timer fijo.

## Impact

- **Assets**: Se requiere un nuevo Prefab para el enemigo Interceptor (puede ser un recolor del actual).
- **Scripts**: Cambios significativos en `EnemySpawner.cs`, `Enemy.cs` y `GameManager.cs`.
- **Gameplay**: El jugador deberá cambiar su estrategia de movimiento para evitar ser interceptado y prepararse para los picos de intensidad de las oleadas.
