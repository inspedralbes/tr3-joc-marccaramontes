## MODIFIED Requirements

### Requirement: Control de Estado de Juego
El sistema SHALL detener la generación de enemigos cuando el juego haya terminado o cuando se haya alcanzado el límite de enemigos de la oleada actual.

#### Scenario: Parada por Límite de Oleada
- **WHEN** El número de enemigos instanciados en la oleada actual alcanza el máximo definido
- **THEN** el sistema SHALL entrar en estado `WaitingForClear` y dejar de instanciar nuevos enemigos hasta que la oleada finalice
