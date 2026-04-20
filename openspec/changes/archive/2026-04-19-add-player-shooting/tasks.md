## 1. Configuración de Entorno

- [x] 1.1 Crear el Tag "Enemy" en los Project Settings.
- [x] 1.2 Asignar el Tag "Enemy" al Prefab `Enemy.prefab`.

## 2. Implementación de Proyectiles

- [x] 2.1 Crear el script `Assets/Bullet.cs` con lógica de movimiento y colisión.
- [x] 2.2 Crear el Prefab `Assets/Bullet.prefab` con un `SpriteRenderer`, `CircleCollider2D` (Trigger) y el script `Bullet`.
- [x] 2.3 Configurar el Prefab de la bala para que se autodestruya a los 3 segundos.

## 3. Lógica de Disparo del Jugador

- [x] 3.1 Crear el script `Assets/PlayerShooting.cs` que maneje el Input y el Cooldown.
- [x] 3.2 Implementar el cálculo de rotación hacia el ratón en `PlayerShooting.cs`.
- [x] 3.3 Añadir el componente `PlayerShooting` al objeto Jugador en la escena.
- [x] 3.4 Asignar el Prefab de la bala al componente `PlayerShooting` del jugador.

## 4. Verificación

- [x] 4.1 Confirmar que al hacer clic se instancia una bala hacia el ratón.
- [x] 4.2 Confirmar que no se puede disparar más rápido que 2 disparos por segundo.
- [x] 4.3 Confirmar que las balas destruyen a los enemigos al impactar.
