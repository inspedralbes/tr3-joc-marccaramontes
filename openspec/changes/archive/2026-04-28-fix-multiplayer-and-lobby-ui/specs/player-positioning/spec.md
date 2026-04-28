## ADDED Requirements

### Requirement: Global Remote Player Management
La escena de juego SHALL contener una instancia de `RemotePlayerManager` para gestionar la visualización de otros jugadores en modo multijugador.

#### Scenario: Visualización de rivales
- **WHEN** Se carga `SampleScene` en modo multijugador
- **THEN** El `RemotePlayerManager` SHALL estar presente y suscrito a los eventos de red para instanciar los prefabs de los otros jugadores.

## MODIFIED Requirements

### Requirement: Remote proxy de-authorization
- **WHEN** the `RemotePlayerManager` instantiates a player prefab for a rival
- **THEN** it MUST immediately set `isLocalPlayer` to false on that instance to prevent input/physics conflicts AND it SHALL disable local movement scripts to ensure only network updates control the ghost.
