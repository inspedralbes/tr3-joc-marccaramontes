## ADDED Requirements

### Requirement: Wave State Management
El sistema SHALL gestionar el ciclo de vida de las oleadas a través de estados (Spawn, Calma, Transición).

#### Scenario: Transición entre oleadas
- **WHEN** Todos los enemigos de la oleada actual han sido derrotados
- **THEN** El sistema SHALL entrar en estado de descanso (Rest) antes de iniciar la siguiente oleada

### Requirement: Wave Progression
El sistema SHALL incrementar la intensidad de las oleadas aumentando el número de enemigos y la proporción de interceptores.

#### Scenario: Aumento de enemigos
- **WHEN** Se inicia la oleada 2
- **THEN** El número total de enemigos SHALL ser superior al de la oleada 1
