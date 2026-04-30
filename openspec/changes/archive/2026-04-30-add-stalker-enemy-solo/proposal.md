## Why

El modo solitario actualmente carece de variedad en los tipos de enemigos y desafíos tácticos. Al introducir el "Stalker", un enemigo que ataca desde la periferia con proyectiles lentos, obligamos al jugador a priorizar objetivos y a moverse constantemente dentro de su plataforma, aumentando la profundidad estratégica y la rejugabilidad.

## What Changes

- **Nuevo Tipo de Enemigo**: Se añade el `Stalker`, un enemigo que mantiene distancia y dispara.
- **Sistema de Proyectiles Enemigos**: Implementación de balas que dañan al jugador, con una estética visual (Neon Naranja) distinta a las del jugador.
- **IA de Órbita con Acoso**: El Stalker no persigue directamente al jugador, sino que intenta mantener una distancia constante y orbitar, con movimientos tangenciales dinámicos.
- **Sincronización de Red via Eventos**: Implementación de un evento `ENEMY_SHOOT` para sincronizar disparos en multijugador sin sobrecargar el tráfico.
- **Refactorización de Enemy.cs**: Limpieza del método `FixedUpdate` separando la lógica de movimiento y disparo para mayor claridad.
- **Sistema de Oleadas Progresivo**: El `EnemySpawner` integrará Stalkers usando un sistema de pesos basado en la dificultad.

## Capabilities

### New Capabilities
- `enemy-projectiles`: Definición del comportamiento y visuales de los proyectiles disparados por enemigos.
- `stalker-enemy-behavior`: Especificación de la IA de mantenimiento de distancia, órbita y acoso.
- `enemy-network-sync`: Protocolo para la sincronización de acciones enemigas (disparos) en el entorno multijugador.

### Modified Capabilities
- `wave-system`: Se actualiza para incluir la probabilidad de aparición de Stalkers basada en pesos de dificultad.
- `enemy-behavior`: Se añade el tipo Stalker al catálogo de comportamientos y se refactoriza la estructura interna.

## Impact

- **Código**: `Enemy.cs` (refactorización y extensión), `EnemySpawner.cs` (pesos de oleada), `NetworkManager.cs` (nuevo evento de disparo), `GameManager.cs` (limpieza de escena).
- **Assets**: Nuevo prefab para `EnemyBullet` con shader neon naranja.
- **Gameplay**: Aumento de la profundidad táctica; el jugador debe esquivar proyectiles mientras se mueve por la plataforma.
- **Balance**: Sincronización precisa de "muerte por proyectil" para asegurar justicia tanto en local como en remoto.
