## ADDED Requirements

### Requirement: Wave State Management
El sistema SHALL gestionar el ciclo de vida de las oleadas a través de estados (Spawn, Calma, Transición), permitiendo el spawneo de múltiples enemigos simultáneamente (clústers).

#### Scenario: Spawneo de Clúster
- **WHEN** llega el momento de spawnear enemigos
- **THEN** el sistema SHALL instanciar un grupo de 3 a 5 enemigos en la misma posición de origen elegida aleatoriamente

#### Scenario: Transición entre oleadas
- **WHEN** Todos los enemigos de la oleada actual han sido derrotados
- **THEN** El sistema SHALL entrar en estado de descanso (Rest) antes de iniciar la siguiente oleada

### Requirement: Wave Progression
El sistema SHALL incrementar la intensidad de las oleadas aumentando significativamente el número de enemigos por ronda y la proporción de interceptores.

#### Scenario: Aumento de densidad
- **WHEN** se inicia una oleada superior a la 1
- **THEN** el número total de enemigos SHALL ser aproximadamente el doble que en la versión base anterior
