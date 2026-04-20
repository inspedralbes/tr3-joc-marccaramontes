## Why

Las balas no están impactando a los enemigos porque ninguno de los dos objetos posee un componente `Rigidbody2D`, el cual es indispensable en Unity para que los eventos de colisión `OnTriggerEnter2D` se activen.

## What Changes

- **Componente Físico**: Añadir un `Rigidbody2D` al Prefab `Bullet.prefab`.
- **Configuración**: Establecer el `Body Type` en `Kinematic` para evitar que la gravedad afecte a la bala, manteniendo la detección de colisiones activa.

## Capabilities

### Modified Capabilities
- `shooting-mechanics`: Asegurar que los proyectiles detecten correctamente los impactos.

## Impact

- **Assets**: `Assets/Bullet.prefab` (modificado con Rigidbody2D).
