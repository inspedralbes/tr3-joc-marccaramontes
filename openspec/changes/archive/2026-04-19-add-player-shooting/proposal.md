## Why

Actualmente el jugador no tiene forma de defenderse de los enemigos, lo que limita la jugabilidad a la evasión pura. Añadir una mecánica de disparo permite una progresión de supervivencia más dinámica y gratificante, permitiendo al jugador limpiar amenazas de forma proactiva.

## What Changes

- **Nuevo Sistema de Disparo**: Implementación de un componente `PlayerShooting` que permite disparar proyectiles.
- **Apuntado Preciso**: El jugador apuntará usando la posición del ratón en un rango de 360 grados.
- **Nuevo Objeto: Bala**: Creación de un Prefab de proyectil con lógica de movimiento y colisión.
- **Mecánica de Combate**: Los enemigos ahora podrán ser destruidos al entrar en contacto con las balas.
- **Configuración de Tags**: Adición del Tag "Enemy" para identificar los objetivos de disparo.

## Capabilities

### New Capabilities
- `shooting-mechanics`: Define la capacidad del jugador de generar proyectiles dirigidos y el comportamiento de dichos proyectiles (velocidad, vida útil, detección de impacto).

### Modified Capabilities
- `enemy-behavior`: Los enemigos deben ser capaces de reaccionar a los impactos de proyectiles (actualmente muriendo instantáneamente).

## Impact

- **Assets**: Creación de `Assets/Bullet.prefab`.
- **Scripts**: Creación de `Assets/Bullet.cs` y `Assets/PlayerShooting.cs`.
- **Escenas**: `SampleScene.unity` requiere que el jugador tenga el nuevo componente de disparo y los enemigos el Tag adecuado.
- **Input**: Uso de la acción `Attack` ya definida en `InputSystem_Actions`.
