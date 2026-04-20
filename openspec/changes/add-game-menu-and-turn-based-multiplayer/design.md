## Context

El juego carece de un sistema de navegación inicial y soporte para múltiples jugadores. Se requiere una arquitectura que permita la persistencia de datos (como el modo de juego seleccionado y los tiempos de los jugadores) a través de cargas de escenas para gestionar los turnos del modo multijugador local.

## Goals / Non-Goals

**Goals:**
- Implementar una interfaz de menú principal clara y funcional.
- Refactorizar el `GameManager` para que sea persistente (`DontDestroyOnLoad`).
- Crear un flujo de juego que maneje turnos consecutivos para dos jugadores.
- Visualizar comparativa de resultados al final de la sesión multijugador.

**Non-Goals:**
- Implementar multijugador en red (online) o simultáneo (pantalla dividida).
- Guardar récords históricos (High Scores) en disco en esta fase.

## Decisions

- **Singleton Persistente**: El `GameManager` usará `DontDestroyOnLoad` para mantener el estado del modo de juego y los resultados parciales entre la carga de la escena de Menú y la escena de Juego.
- **Enumerados de Estado**:
  - `GameMode { Solo, Multiplayer }`: Para definir la lógica de reinicio.
  - `TurnState { Player1, Player2 }`: Para identificar quién está jugando actualmente.
- **Flujo de Escenas**:
  - Menú -> Juego (Turno 1) -> (Si Multi) Juego (Turno 2) -> Resultados (UI Overlay) -> Menú.
- **Interfaz de Usuario**:
  - `MenuController`: Nuevo script para gestionar los eventos de los botones del menú.
  - El Canvas de resultados será un objeto desactivado por defecto en la escena de juego o una escena separada, gestionado por el `GameManager`.

## Risks / Trade-offs

- **Z-Ordering de UI**: Asegurar que los botones sean clickeables y no estén bloqueados por otros elementos del Canvas.
- **Detección de Muerte**: El script `PlayerMovement.cs` deberá llamar a un método del `GameManager` que decida si reiniciar la escena o pasar al siguiente turno, en lugar de cargar la escena directamente.
- **Escalabilidad**: El diseño actual soporta 2 jugadores; añadir más requeriría una lista dinámica de tiempos en el `GameManager`.
