## Context

La implementación del sistema multijugador introdujo el componente `NetworkIdentity` y estados de red que, por defecto, están desactivados o en "espera" de una conexión. Esto ha roto la experiencia de juego en solitario y las pruebas rápidas en el editor, ya que el código ahora depende de flags externos que no se inicializan correctamente fuera del flujo de red.

## Goals / Non-Goals

**Goals:**
- Restaurar la funcionalidad del modo Solo.
- Permitir el testeo de la escena `SampleScene` directamente desde el editor de Unity.
- Asegurar que el jugador local siempre tenga autoridad en modos no-multijugador.

**Non-Goals:**
- Cambiar la lógica de red ya establecida para el modo online.
- Rediseñar el sistema de Input.

## Decisions

### 1. Inicialización Condicional de NetworkIdentity
- **Decisión**: En `PlayerMovement.Awake()`, se añadirá una comprobación: si `NetworkManager.Instance` es nulo o si `currentRoomId` está vacío, se forzará `networkIdentity.isLocalPlayer = true`.
- **Razón**: En modo Solo o durante pruebas locales en el editor sin servidor, el jugador siempre debe ser considerado "local" para que el script de movimiento y físicas funcione.

### 2. Recuperación del "Safe Start" en GameManager
- **Decisión**: Re-implementar en `GameManager.Start()` una detección de escena actual. Si la escena es `SampleScene` y el estado es `Menu`, cambiar automáticamente a `Playing`.
- **Razón**: Mejora la productividad del desarrollo al permitir pulsar "Play" en cualquier escena.

### 3. Validación de Acciones de Input
- **Decisión**: Añadir un log de advertencia en `PlayerMovement` si `moveAction` es nulo tras el `Awake`.
- **Razón**: Ayuda a diagnosticar rápidamente si el problema es de configuración en el componente `PlayerInput` del Inspector.

## Risks / Trade-offs

- **[Riesgo] Conflictos en el inicio de partida Online** → **Mitigación**: Asegurarse de que el "Safe Start" solo ocurra si no hay una conexión de red activa o si el `GameMode` no es `Online`.
- **[Riesgo] Falsos positivos de isLocalPlayer** → **Mitigación**: La lógica de red de `NetworkManager` sobrescribirá estos valores cuando se establezca una conexión real.
