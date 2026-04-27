## ADDED Requirements

### Requirement: Inicialización en el centro de la plataforma
El sistema DEBE teletransportar al jugador al centro de la plataforma asignada al iniciar el juego.

#### Scenario: Teletransporte inicial
- **WHEN** el método Start de PlayerMovement se ejecuta
- **THEN** la posición del jugador se iguala a la posición del platformCenter asignado

### Requirement: Validación de límites relativa al centro
El sistema DEBE calcular si el jugador se ha salido de la plataforma basándose en la distancia entre el jugador y el centro de la plataforma, no basándose en el origen global (0,0).

#### Scenario: Muerte por caída fuera del centro
- **WHEN** la plataforma está en la posición (5,5) y el jugador se mueve a la posición (10,5) con un radio de plataforma de 2.5
- **THEN** el sistema detecta que el jugador está fuera de los límites y ejecuta la función Die()

### Requirement: Visualización de límites en Editor (Gizmos)
El sistema DEBE dibujar el radio de la plataforma en la vista de Escena para facilitar el ajuste del nivel.

#### Scenario: Visualización del radio de caída
- **WHEN** el script PlayerMovement está activo en el editor
- **THEN** se dibuja un círculo rojo (o de contraste) que representa el platformRadius alrededor del platformCenter

### Requirement: Hitbox Standardization
The system SHALL use a standard circular hitbox for all characters (Player and Enemy) with a diameter of 0.5 units (radius 0.25).

#### Scenario: Player Hitbox Verification
- **WHEN** the `PlayerMovement` script is active
- **THEN** the `CircleCollider2D` component MUST have a radius of 0.25

### Requirement: Local Player Self-Identification
The `PlayerMovement` system SHALL automatically identify as the "Local Player" by default. This ensures that the local instance always has authority to process input and physics, while remote "ghost" instances must be explicitly marked as non-local during their initialization.

#### Scenario: Authority in all modes
- **WHEN** the `PlayerMovement` component initializes
- **THEN** the `NetworkIdentity` component MUST set `isLocalPlayer` to true by default, regardless of whether a network manager is present.

#### Scenario: Remote proxy de-authorization
- **WHEN** the `RemotePlayerManager` instantiates a player prefab for a rival
- **THEN** it MUST immediately set `isLocalPlayer` to false on that instance to prevent input/physics conflicts.
