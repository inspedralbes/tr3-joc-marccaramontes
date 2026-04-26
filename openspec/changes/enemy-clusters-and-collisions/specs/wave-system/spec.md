## MODIFIED Requirements

### Requirement: Wave State Management
El sistema SHALL gestionar el ciclo de vida de las oleadas, permitiendo el spawneo de múltiples enemigos simultáneamente (clústers).

#### Scenario: Spawneo de Clúster
- **WHEN** llega el momento de spawnear enemigos
- **THEN** el sistema SHALL instanciar un grupo de 3 a 5 enemigos en la misma posición de origen elegida aleatoriamente

### Requirement: Wave Progression
El sistema SHALL incrementar la intensidad de las oleadas aumentando significativamente el número de enemigos por ronda.

#### Scenario: Aumento de densidad
- **WHEN** se inicia una oleada superior a la 1
- **THEN** el número total de enemigos SHALL ser aproximadamente el doble que en la versión base anterior
