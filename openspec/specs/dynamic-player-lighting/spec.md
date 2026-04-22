## ADDED Requirements

### Requirement: Player-Centric Torch Aura
The system SHALL ensure the player character acts as a mobile light source in the environment.

#### Scenario: Torch Configuration
- **WHEN** the player is instantiated in `SampleScene`
- **THEN** a `Point Light 2D` MUST be attached to the player with an outer radius of 8 units and color #FFCC88.

### Requirement: Darkness-First Lighting
The system SHALL configure ambient light levels to emphasize player lighting.

#### Scenario: Ambient Lighting Configuration
- **WHEN** the scene is initialized via the setup tool
- **THEN** the `Global Light 2D` intensity MUST be set to 0.15.
