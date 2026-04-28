## Why

El sistema de spawneo actual genera enemigos de forma individual y aislada, lo que hace que las hordas se sientan poco naturales y fáciles de gestionar. Al agrupar a los enemigos en clústers y permitir que colisionen físicamente entre ellos, logramos un comportamiento de masa más orgánico y un aumento real en la presión táctica para el jugador.

## What Changes

- **Spawneo en Clústers (Enemy Clusters)**: El spawner ahora generará grupos de enemigos en un mismo punto de origen de forma simultánea.
- **Físicas de Empuje entre Enemigos**: Se añadirán componentes físicos (`Rigidbody2D` y `CircleCollider2D`) a los enemigos para que se empujen y se distribuyan naturalmente al nacer.
- **Aumento de Densidad de Hordas**: Se incrementará el número total de enemigos y la frecuencia de aparición en cada oleada.
- **Comportamiento Individual en Grupo**: Aunque nazcan juntos, cada enemigo mantendrá su lógica de movimiento independiente hacia el jugador.

## Capabilities

### New Capabilities
- `enemy-physics`: Soporte para colisiones físicas y resolución de solapamientos entre enemigos.

### Modified Capabilities
- `wave-system`: Actualización para manejar la generación de grupos (clústers) y aumentar el recuento de enemigos por ronda.

## Impact

- **Prefabs**: Actualización del Prefab de Enemigo con `Rigidbody2D` dinámico y colisionador no-trigger.
- **Scripts**: Modificaciones en `EnemySpawner.cs` y `Enemy.cs` para manejar la física y el spawneo múltiple.
- **Gameplay**: Mayor dificultad debido a la densidad de enemigos y la imposibilidad de que estos se atraviesen, creando "muros" orgánicos de enemigos.
