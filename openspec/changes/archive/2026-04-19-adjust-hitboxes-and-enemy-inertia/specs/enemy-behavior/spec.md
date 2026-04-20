## MODIFIED Requirements

### Requirement: Enemy Movement
Active enemies SHALL move towards the player with inertia and turning penalties. They SHALL accelerate when moving in a straight line and decelerate when forced to make sharp turns.

#### Scenario: Follow Player with Inertia
- **WHEN** the `Enemy` script is active and a player object is found
- **THEN** the enemy adjusts its direction towards the player gradually based on its `turnSpeed`
- **THEN** its current velocity increases when the angle to the player is small, and decreases when the angle is large
