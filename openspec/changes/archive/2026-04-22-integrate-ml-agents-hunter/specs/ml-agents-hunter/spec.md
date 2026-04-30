## ADDED Requirements

### Requirement: Dynamic Target Acquisition
The hunter agent MUST continuously evaluate the positions of all active players and select the closest one as its current pursuit target.

#### Scenario: Switching targets
- **WHEN** a player moves closer to the agent than the current target
- **THEN** the agent SHALL switch its focus to the closer player

### Requirement: Host-Driven AI
The decision-making and training logic of the hunter agent SHALL execute exclusively on the Host instance of the game.

#### Scenario: Client synchronization
- **WHEN** the game is running in a multiplayer session
- **THEN** only the Host's agent instance SHALL compute actions, while remote clients SHALL receive position updates via network events

### Requirement: Reward-Based Performance
The agent SHALL be trained using a reward system that incentivizes staying close to and eventually colliding with the closest player.

#### Scenario: Successful hunt
- **WHEN** the agent collides with a player
- **THEN** it SHALL receive a positive reward and reset its state for the next episode
