## Why

La interfaz del Lobby multijugador es actualmente funcional pero carece del pulido visual y la fluidez presentes en el Menú principal y la SampleScene. Para proporcionar una experiencia de usuario profesional y consistente, es necesario integrar el sistema de animaciones (`UIAnimationManager`) y los efectos interactivos en la sala de espera.

## What Changes

- **Transiciones Fluidas**: Sustitución de activaciones binarias (`SetActive`) por desvanecimientos (`Fade`) mediante `CanvasGroup`.
- **Estética Infernal**: Aplicación del `UIThemeSO` a todos los elementos del Lobby.
- **Botones Dinámicos**: Implementación de `InfernalButton` en lugar de simples hovers, permitiendo efectos de escala y desintegración de píxeles al hacer clic.
- **Identidad Visual**: Unificación de colores (Blood Red, Void Black) y fuentes en paneles, textos e inputs.
- **Énfasis en Datos Críticos**: Animación de escala (`PulseScale`) en el código de la sala.

## Capabilities

### New Capabilities
- `lobby-visual-polish`: Requisitos para la fluidez de transiciones y feedback interactivo específicos de la sala multijugador.

### Modified Capabilities
- `ui-animation-system`: Extensión menor para asegurar compatibilidad con elementos de UI dinámicos en el Lobby.

## Impact

- **Assets/Networking/LobbyController.cs**: Refactorización para gestionar `CanvasGroup` y llamar al `UIAnimationManager`.
- **Assets/Scenes/Lobby.unity**: Modificación de la jerarquía para añadir `CanvasGroup` a los paneles y componentes de efectos a los botones.
- **Assets/UIAnimationManager.cs**: Verificación de robustez para llamadas rápidas desde el Lobby.
