## ADDED Requirements

### Requirement: ML-Agents Hunter Agent
The game SHALL include an AI agent ("Hunter") trained using ML-Agents to pursue players in the scene.

#### Scenario: Agent Pursuit
- **WHEN** the "Hunter" agent detects a player within its raycast sensor range
- **THEN** the agent MUST move towards the player using its trained neural network model.

### Requirement: Agent State Sync
The position and state of the ML-Agent MUST be synchronized across all clients in a session.

#### Scenario: Sync via Host
- **WHEN** the game is running in multiplayer mode
- **THEN** the "Host" client SHALL calculate the agent's logic and broadcast its position to all "Guest" clients via the Game Service.
