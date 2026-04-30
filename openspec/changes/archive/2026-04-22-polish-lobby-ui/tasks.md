## 1. Preparación de la Escena y UI

- [x] 1.1 Añadir componentes `CanvasGroup` a `MainPanel` y `WaitingPanel` en la escena `Lobby.unity`.
- [x] 1.2 Añadir el script `ButtonHoverEffect` a los botones: `Create`, `Join`, `Back` y `StartMatch`.
- [x] 1.3 Asegurar que los paneles tengan sus referencias de `alpha` y `interactable` configuradas por defecto (Main: 1, Waiting: 0).

## 2. Refactorización de LobbyController

- [x] 2.1 Añadir referencias para `CanvasGroup mainPanelGroup` y `CanvasGroup waitingPanelGroup` en `LobbyController.cs`.
- [x] 2.2 Actualizar `OnCreateRoom` y `OnJoinRoom` para utilizar `UIAnimationManager.Instance.FadeCanvasGroup` en lugar de `SetActive`.
- [x] 2.3 Implementar lógica de desvanecimiento inverso en el botón `Atrás` del panel de espera.
- [x] 2.4 Añadir llamada a `PulseScale` sobre `roomCodeText` cuando se reciba el `roomId`.

## 3. Pulido de Feedback de Estado

- [x] 3.1 Modificar la lógica de error en los callbacks de `PostRequest` para cambiar el color de `statusText` a rojo.
- [x] 3.2 Añadir un pequeño pulso de escala sobre `statusText` cuando ocurra un error de conexión.
- [x] 3.3 Implementar un efecto visual sutil (ej. Fade In rápido) al aparecer el mensaje "Esperando jugadores...".

## 4. Estética Infernal y Estilo Visual

- [x] 4.1 Sustituir `ButtonHoverEffect` por el script `InfernalButton` en los botones del Lobby.
- [x] 4.2 Asignar el asset `InfernalTheme.asset` a todos los componentes `InfernalButton`.
- [x] 4.3 Aplicar color `Blood Red` a todos los textos de TextMesh Pro en el Lobby.
- [x] 4.4 Configurar fondos de `MainPanel` y `WaitingPanel` con color `Void Black` y transparencia del 80%.
- [x] 4.5 Estilizar los `TMP_InputField` para usar fondo negro y texto de color `Blood Red`.

## 5. Verificación y Ajustes

- [x] 5.1 Validar que los botones siguen siendo interactuables después de las transiciones.
- [x] 5.2 Comprobar la consistencia de los tiempos de animación (0.3s - 0.5s) con el resto del juego.
- [x] 5.3 Verificar que el `UIAnimationManager` se encuentra correctamente en la escena del Lobby.
- [x] 5.4 Confirmar que el efecto de desintegración de `InfernalButton` funciona al hacer clic.
