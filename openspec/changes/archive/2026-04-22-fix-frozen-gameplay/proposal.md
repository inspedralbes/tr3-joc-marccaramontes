## Why

Después de integrar el sistema multijugador online, el juego se queda "congelado" (el personaje no se mueve y el tiempo no corre) al iniciar la escena de juego. Esto sucede porque el `PlayerMovement` no se identifica como jugador local por defecto y el `GameManager` no activa el estado de juego automáticamente al cargar la escena fuera del flujo del menú.

## What Changes

- **Auto-Identificación de Jugador Local**: El script `PlayerMovement` ahora detectará si está en una partida local (Solo) y se asignará automáticamente como `isLocalPlayer = true`.
- **Restauración de "Safe Start" en GameManager**: El `GameManager` volverá a incluir la lógica que detecta si el juego se ha iniciado directamente desde la escena `SampleScene` en el editor de Unity, forzando el estado a `Playing`.
- **Validación de Input**: Se añade un mensaje de aviso si el componente `PlayerInput` no tiene configurada la acción "Move".

## Capabilities

### Modified Capabilities
- `game-state-management`: Se ajusta para permitir la transición automática al estado `Playing` cuando se carga la escena de juego directamente en el editor.
- `player-positioning`: El jugador ahora debe auto-identificarse como local en modos no-multijugador.

## Impact

- **Assets**: Ninguno.
- **Scripts**: 
  - `Assets/GameManager.cs`: Restauración de lógica de inicio en `Start()`.
  - `Assets/PlayerMovement.cs`: Mejora en la inicialización de `NetworkIdentity`.
- **Gameplay**: Permite volver a testear la escena de juego individualmente y restaura la movilidad del personaje.
