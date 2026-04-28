## MODIFIED Requirements

### Requirement: Player-Centric Torch Aura
The system SHALL ensure the player character acts as a mobile light source in the environment, persisted in the scene or prefab.

#### Scenario: Torch Configuration
- **WHEN** the player is instantiated or the scene is setup via the editor tool
- **THEN** a `Point Light 2D` MUST be attached to the player with an outer radius of 8 units and color #FFCC88, and its `Intensity` MUST be set to 1.0.

### Requirement: Darkness-First Lighting
The system SHALL configure ambient light levels to emphasize player lighting.

#### Scenario: Ambient Lighting Configuration
- **WHEN** the scene is initialized via the setup tool
- **THEN** the `Global Light 2D` intensity MUST be set to 0.15.
