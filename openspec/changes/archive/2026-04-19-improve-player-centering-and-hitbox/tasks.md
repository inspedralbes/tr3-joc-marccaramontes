## 1. Refactorización de PlayerMovement

- [x] 1.1 Añadir variable `public Transform platformCenter` en `PlayerMovement.cs`.
- [x] 1.2 Implementar lógica en `Start()` para teletransportar al jugador a `platformCenter.position` (o buscar objeto con tag "Platform" si es null).
- [x] 1.3 Modificar `CheckBounds()` para usar la distancia relativa a `platformCenter` en lugar de `sqrMagnitude` desde el origen.
- [x] 1.4 Implementar `OnDrawGizmos()` para visualizar el radio de la plataforma y el hitbox en el editor.

## 2. Visualización del Hitbox (Gameplay)

- [x] 2.1 Crear un objeto hijo en el Prefab del Jugador llamado "HitboxVisual". (Automatizado por script)
- [x] 2.2 Configurar un `SpriteRenderer` en "HitboxVisual" con un sprite circular azul semitransparente. (Cambiado a LineRenderer por script)
- [x] 2.3 Asegurar que el tamaño del sprite visual coincida con el radio del `CircleCollider2D` del jugador. (Automatizado por script)

## 3. Validación y Ajustes Finales

- [x] 3.1 Probar el inicio del juego con la plataforma desplazada del origen (0,0).
- [x] 3.2 Verificar que el jugador muere correctamente al salir de los nuevos límites dinámicos.
- [x] 3.3 Confirmar que el hitbox azul es visible y está correctamente centrado durante el movimiento.
