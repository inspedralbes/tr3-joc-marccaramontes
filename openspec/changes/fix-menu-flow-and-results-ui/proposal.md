## Why

El juego presenta un fallo crítico donde la pantalla de Game Over no bloquea el disparo del jugador, y los botones de la interfaz son inaccesibles por falta de un sistema de eventos en la escena. Además, el aspecto visual de la pantalla de resultados es rudimentario y necesita un rediseño que cumpla con estándares mínimos de estética y usabilidad.

## What Changes

- **Bloqueo de Input**: Modificar `PlayerShooting.cs` para ignorar clics cuando el juego está en estado `GameOver`.
- **Habilitación de Clics UI**: Actualizar la herramienta `SceneSetupHelper.cs` para asegurar la creación de un `EventSystem` y la correcta configuración del `GraphicRaycaster`.
- **Rediseño Estético (UI Polishing)**: 
    - Implementar `VerticalLayoutGroup` para alineación automática de elementos.
    - Mejorar la paleta de colores y el estilo de los botones (bordes, contrastes).
    - Añadir sombras/contornos a los textos para mejorar la legibilidad sobre el fondo.
- **Robustez de Escena**: Asegurar que el `GameManager` sea capaz de resetear el estado de disparo al reiniciar la partida.

## Capabilities

### New Capabilities
- `ui-input-shielding`: Prevención de que las acciones del juego (disparar, mover) interfieran con la interacción de la interfaz de usuario.
- `scene-event-integrity`: Garantía de que todos los sistemas necesarios para la interacción (EventSystem, Raycasters) estén presentes y activos.

### Modified Capabilities
- `session-statistics`: Mejora en la visualización de las bajas y el tiempo con un formato más atractivo.

## Impact

- **PlayerShooting.cs**: Nueva dependencia de estado del GameManager.
- **SceneSetupHelper.cs**: Expansión mayor para generar una estructura de UI más compleja y profesional.
- **EventSystem**: Adición de un nuevo objeto vital en la jerarquía de la escena.
