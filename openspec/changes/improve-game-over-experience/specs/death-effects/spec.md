## ADDED Requirements

### Requirement: Pixel Fragmentation Death
The system SHALL replace the player sprite with a particle explosion composed of square pixels (fragmentation) upon death.

#### Scenario: Player Death Fragmentation
- **WHEN** the player's health reaches zero or a boundary is hit
- **THEN** the player sprite MUST be disabled and a particle system MUST emit square fragments at the player's last position

### Requirement: High-Contrast Death Flash
The system SHALL trigger a brief, full-screen white flash to provide immediate visual feedback of the death event.

#### Scenario: Visual Feedback Flash
- **WHEN** the `ProcessDeath` sequence is initiated
- **THEN** the system MUST display a white overlay on the UI layer for 0.1 seconds before fading it out
