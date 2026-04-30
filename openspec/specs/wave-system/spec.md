## ADDED Requirements

### Requirement: Wave State Management
El sistema SHALL gestionar el ciclo de vida de las oleadas a través de estados (Spawn, Calma, Transición), permitiendo el spawneo de múltiples enemigos simultáneamente (clústers).

#### Scenario: Spawneo de Clúster
- **WHEN** llega el momento de spawnear enemigos
- **THEN** el sistema SHALL instanciar un grupo de 3 a 5 enemigos en la misma posición de origen elegida aleatoriamente

#### Scenario: Transición entre oleadas
- **WHEN** Todos los enemigos de la oleada actual han sido derrotados
- **THEN** El sistema SHALL entrar en estado de descanso (Rest) antes de iniciar la siguiente oleada

### Requirement: Enemy Movement
Active enemies SHALL move towards their target with inertia and turning penalties. They SHALL accelerate when moving in a straight line and decelerate when forced to make sharp turns. Their final speed SHALL be influenced by a global difficulty multiplier.

#### Scenario: Follow Target with Multiplier
- **WHEN** the `Enemy` script is active and a target (player or predicted point) is found
- **THEN** the enemy adjusts its direction towards the target gradually based on its `turnSpeed`
- **THEN** its current velocity is multiplied by the global `difficultyMultiplier` from the `GameManager`
- **THEN** si el enemigo es de tipo Stalker, su movimiento SHALL ser de órbita manteniendo la distancia ideal.

### Requirement: Wave Progression
El sistema SHALL incrementar la intensidad de las oleadas aumentando significativamente el número de enemigos por ronda y la proporción de interceptores y stalkers.

#### Scenario: Aumento de densidad
- **WHEN** se inicia una oleada superior a la 1
- **THEN** el número total de enemigos SHALL ser aproximadamente el doble que en la versión base anterior
- **THEN** a partir de la oleada 3, el sistema SHALL incluir Stalkers en el pool de enemigos.
