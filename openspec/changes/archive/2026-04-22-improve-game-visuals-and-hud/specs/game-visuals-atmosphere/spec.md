## ADDED Requirements

### Requirement: Atmospheric Abyss Visuals
The system SHALL configure the main camera and environment to simulate an infinite abyss, focusing the player's attention on the central platform.

#### Scenario: Camera Background Configuration
- **WHEN** the `SceneSetupHelper` tool is executed
- **THEN** the `Main Camera` background color MUST be set to solid black (#000000) and the `Clear Flags` MUST be set to `Solid Color`.

#### Scenario: Post-Processing Effects
- **WHEN** the game is running
- **THEN** the `Global Volume` MUST apply a `Vignette` effect with intensity >= 0.4 and a `Film Grain` effect with intensity >= 0.2 to the final render.

### Requirement: Platform Visual Style
The system SHALL ensure the platform has a visual appearance consistent with an ancient/burnt altar.

#### Scenario: Platform Material and Color
- **WHEN** the `SceneSetupHelper` tool is executed
- **THEN** the object tagged as `Platform` MUST have its `SpriteRenderer` color set to a dark grey/charcoal (#1A1A1A).
