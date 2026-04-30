## ADDED Requirements

### Requirement: Hitbox Standardization
The system SHALL use a standard circular hitbox for all characters (Player and Enemy) with a diameter of 0.5 units (radius 0.25).

#### Scenario: Player Hitbox Verification
- **WHEN** the `PlayerMovement` script is active
- **THEN** the `CircleCollider2D` component MUST have a radius of 0.25
