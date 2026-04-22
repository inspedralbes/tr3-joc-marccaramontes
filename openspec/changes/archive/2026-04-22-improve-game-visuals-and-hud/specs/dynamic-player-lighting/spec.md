## ADDED Requirements

### Requirement: Player as Light Source
The system SHALL treat the player as the primary source of illumination within the "Abyss" environment.

#### Scenario: Player Light Setup
- **WHEN** the `SceneSetupHelper` tool is executed
- **THEN** a `Light 2D` (Point) component MUST be added to the player object with an outer radius between 7 and 10 units.

#### Scenario: Light Color and Intensity
- **WHEN** the player light is active
- **THEN** the light color MUST be a warm orange/amber (#FFCC88) to simulate a torch or flame.

### Requirement: Low-Ambient Global Lighting
The system SHALL maintain a low level of global illumination to ensure limited visibility of the platform boundaries.

#### Scenario: Global Light Intensity
- **WHEN** the `SceneSetupHelper` tool is executed
- **THEN** the `Global Light 2D` intensity MUST be set to 0.15.
