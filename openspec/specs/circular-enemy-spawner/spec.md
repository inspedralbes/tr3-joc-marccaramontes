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
El sistema SHALL detener la generación de enemigos cuando el juego haya terminado o cuando se haya alcanzado el límite de enemigos de la oleada actual.

#### Scenario: Parada por Límite de Oleada
- **WHEN** El número de enemigos instanciados en la oleada actual alcanza el máximo definido
- **THEN** el sistema SHALL entrar en estado `WaitingForClear` y dejar de instanciar nuevos enemigos hasta que la oleada finalice

### Requirement: Estandarización de Eventos de Red
Todos los eventos de spawneo de enemigos y acciones de combate (disparos) SHALL usar nombres de eventos en MAYÚSCULAS para garantizar la compatibilidad entre Unity (C#) y el Servidor (Node.js).

#### Scenario: Spawneo sincronizado
- **WHEN** El Host genera un enemigo
- **THEN** Emite el evento `SPAWN_ENEMY` (exactamente en mayúsculas) al servidor.

#### Scenario: Disparo sincronizado
- **WHEN** El jugador dispara
- **THEN** Emite el evento `SHOOT` al servidor para que sea retransmitido.

### Requirement: Sincronización de Enemigos en Clientes
Los jugadores que no son Host SHALL ver los enemigos generados por el Host en las mismas posiciones.

#### Scenario: Recepción de enemigo remoto
- **WHEN** Un cliente recibe un evento `SPAWN_ENEMY`
- **THEN** Instancia el prefab de enemigo en la posición recibida y le asigna el ID de red correspondiente.
