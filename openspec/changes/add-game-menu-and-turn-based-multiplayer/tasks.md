## 1. Refactorización del GameManager

- [x] 1.1 Implementar `DontDestroyOnLoad` y la protección del Singleton en `GameManager.cs`.
- [x] 1.2 Añadir los enumerados `GameMode` y `TurnState`.
- [x] 1.3 Añadir variables para almacenar `p1Time` y `p2Time`.
- [x] 1.4 Crear el método `ProcessDeath()` que decida el siguiente estado basándose en el modo y turno actual.

## 2. Creación del Menú Principal

- [x] 2.1 Crear una nueva escena llamada `Menu.unity`.
- [ ] 2.2 Configurar un `Canvas` con dos botones: "Jugar Solo" y "Multijugador".
- [x] 2.3 Crear el script `MenuController.cs` para asignar las funciones a los botones.
- [ ] 2.4 Configurar las Build Settings de Unity para incluir la escena de Menú en el índice 0.

## 3. Lógica de Turnos y Resultados

- [x] 3.1 Modificar `PlayerMovement.cs` para que llame a `GameManager.Instance.ProcessDeath()` en lugar de recargar la escena directamente.
- [ ] 3.2 Crear una interfaz de UI sencilla para mostrar el ganador tras el turno del Jugador 2.
- [ ] 3.3 Añadir un botón de "Volver al Menú" en la pantalla de resultados.

## 4. Verificación

- [ ] 4.1 Verificar que al elegir "Solo", el juego funciona como antes.
- [ ] 4.2 Verificar que en "Multijugador", la escena se reinicia tras la muerte del Jugador 1 y pasa al turno del Jugador 2.
- [ ] 4.3 Confirmar que al morir el Jugador 2, se visualizan ambos tiempos y el ganador.
