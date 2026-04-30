## ADDED Requirements

### Requirement: Relay Session Creation
The system SHALL integrate with the **Unity Relay Service** to allow players to host matches over the internet. The Host MUST be able to request a Relay allocation and receive a unique 6-character **Join Code**.

#### Scenario: Hosting with Relay
- **WHEN** a player clicks "Create Online Room"
- **THEN** the system SHALL request a Relay allocation, configure the `UnityTransport` with the allocation data, and display the Join Code to the player

### Requirement: Joining via Relay Code
The system SHALL allow players to join an existing session by entering a valid Relay Join Code. The system MUST resolve the Join Code into connection parameters using the Unity Relay API.

#### Scenario: Joining with Code
- **WHEN** a player enters a 6-digit Join Code and clicks "Join Room"
- **THEN** the system SHALL attempt to join the Relay session and establish a connection to the Host's `UnityTransport`

### Requirement: Connection Feedback
The system SHALL provide visual feedback to the user during the Relay connection process, including success, failure, and timeout states.

#### Scenario: Connection Timeout
- **WHEN** a Join Code is invalid or the Host is unreachable
- **THEN** the UI MUST display a "Failed to connect: Invalid Code or Timeout" message and return the player to the lobby screen
