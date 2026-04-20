## ADDED Requirements

### Requirement: Spawneo en Corona Circular
El sistema SHALL calcular una posición aleatoria dentro de una corona circular definida por un radio mínimo y un radio máximo, centrada en un Transform de referencia.

#### Scenario: Generación de posición válida
- **WHEN** el sistema solicita un nuevo enemigo
- **THEN** se elige un ángulo aleatorio [0, 2π] y un radio aleatorio [minRadius, maxRadius], y la posición resultante es `center + (cos(a), sin(a)) * r`

### Requirement: Visualización en Editor
El sistema SHALL dibujar Gizmos en la vista de Escena de Unity que representen visualmente los límites del área de spawneo.

#### Scenario: Visualización de límites
- **WHEN** el componente EnemySpawner está seleccionado en el editor
- **THEN** se dibujan dos círculos de alambre (wire spheres) correspondientes a `minRadius` y `maxRadius`

### Requirement: Control de Estado de Juego
El sistema SHALL detener la generación de enemigos cuando el juego haya terminado.

#### Scenario: Parada por Game Over
- **WHEN** `GameManager.Instance.isGameOver` es verdadero
- **THEN** el método `Update` del spawner no debe ejecutar la lógica de instanciación ni descontar tiempo para el siguiente spawn
