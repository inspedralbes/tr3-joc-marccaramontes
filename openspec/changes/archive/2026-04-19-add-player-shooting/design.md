## Context

El juego carece de mecánicas ofensivas. Se implementará un sistema de disparo basado en proyectiles donde el jugador apunta con el ratón. Se usará el Input System existente para capturar la intención de disparo.

## Goals / Non-Goals

**Goals:**
- Implementar un sistema de disparo funcional y responsivo.
- Crear un Prefab de bala con colisiones 2D.
- Asegurar que los enemigos puedan ser eliminados.

**Non-Goals:**
- Implementar armas múltiples o Power-ups en esta fase.
- Añadir efectos de partículas complejos o sonidos.

## Decisions

- **Referencia a Cámara**: Se usará `Camera.main` para convertir la posición del ratón de coordenadas de pantalla a coordenadas del mundo.
- **Lógica de Cooldown**: Se utilizará una variable `float lastFireTime` comparada con `Time.time` para gestionar la cadencia de 2 disparos por segundo.
- **Componente PlayerShooting**: Este nuevo script se añadirá al objeto Jugador y tendrá una referencia al `BulletPrefab`.
- **Componente Bullet**: Este script gestionará la velocidad (`speed * transform.up`) y la destrucción por tiempo (`Destroy(gameObject, lifeTime)`).
- **Colisiones**: Se usará `OnTriggerEnter2D` en el script de la bala para detectar enemigos basándose en su Tag.
- **Tags**: Se creará el Tag "Enemy" y se asignará al Prefab del enemigo.

## Risks / Trade-offs

- **Z-Axis Lock**: Al convertir de pantalla a mundo, es crítico forzar `z = 0` para evitar que las balas se instancien detrás de la cámara o de la escena 2D.
- **Performance**: La destrucción constante de balas es aceptable para 2 disparos/seg, pero si la cadencia aumenta, se debería considerar Object Pooling.
- **Colisión Perdida**: Si la bala es muy rápida, podría atravesar enemigos pequeños. Se usará `CollisionDetectionMode2D.Continuous` en la bala si esto ocurre.
