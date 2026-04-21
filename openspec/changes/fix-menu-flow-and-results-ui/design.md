## Context

La implementación actual de la UI es funcional pero incompleta. Los clics "traspasan" el panel de resultados porque no hay un `EventSystem` que los detenga y el script de disparo no consulta el estado del juego. Además, la jerarquía de la UI es manual y difícil de mantener estéticamente.

## Goals / Non-Goals

**Goals:**
- Bloquear el disparo del jugador cuando el panel de resultados sea visible.
- Hacer que los botones de Reintentar y Menú respondan al primer clic.
- Mejorar el aspecto visual del menú con alineación automática y mejores estilos.

**Non-Goals:**
- Implementar animaciones complejas de UI en este paso.
- Añadir sonidos a la interfaz.

## Decisions

### 1. Bloqueo de Input de Disparo
- **Decisión**: Añadir `if (GameManager.Instance.IsGameOver) return;` al inicio de `PlayerShooting.Update`.
- **Razón**: Es la forma más sencilla y efectiva de evitar que el jugador siga interactuando con el mundo una vez finalizada la partida.

### 2. Generación de EventSystem
- **Decisión**: Modificar la herramienta `SceneSetupHelper` para buscar un objeto con el componente `EventSystem`. Si no existe, crearlo.
- **Razón**: Unity requiere este objeto para que el puntero del ratón interactúe con el Canvas.

### 3. Uso de Layout Groups
- **Decisión**: El `PanelResultados` tendrá un componente `VerticalLayoutGroup` y un `ContentSizeFitter`.
- **Razón**: Permite que los textos y botones se distribuyan solos con espaciado constante, eliminando la necesidad de posicionarlos a mano por coordenadas.

### 4. Estilo Visual (Aesthetics)
- **Decisión**: 
    - Fondo con un tono gris azulado oscuro y borde blanco fino.
    - Botones con colores pasteles/vibrantes pero con esquinas ligeramente redondeadas (vía sprites por defecto de Unity).
    - Textos con el componente `Shadow` de Unity UI para resaltar el contraste.

## Risks / Trade-offs

- **[Riesgo]** El `EventSystem` puede causar conflictos si ya existe uno con una configuración diferente (ej. para VR). → **Mitigación**: Usar `FindObjectOfType<EventSystem>` para no crear duplicados.
- **[Riesgo]** Los Layout Groups pueden ser difíciles de configurar por código. → **Mitigación**: Definir valores de padding y spacing fijos en el helper.

## Migration Plan

1. **Scripts**: Modificar `PlayerShooting.cs` y expandir `SceneSetupHelper.cs`.
2. **Unity**: Ejecutar de nuevo el menú **Tools > Setup HUD and Results UI**.
3. **Validación**: Comprobar que al morir (a) no sale fuego por el ratón y (b) los botones reaccionan al pasar el ratón por encima (cambio de color) y al clicar.
