## MODIFIED Requirements

### Requirement: Enemy Movement
Active enemies SHALL move towards their target with inertia and turning penalties. They SHALL accelerate when moving in a straight line and decelerate when forced to make sharp turns. Their final speed SHALL be influenced by a global difficulty multiplier.

#### Scenario: Follow Target with Multiplier
- **WHEN** the `Enemy` script is active and a target (player or predicted point) is found
- **THEN** the enemy adjusts its direction towards the target gradually based on its `turnSpeed`
- **THEN** its current velocity is multiplied by the global `difficultyMultiplier` from the `GameManager`
