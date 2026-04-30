## MODIFIED Requirements

### Requirement: Editor Safe Start
The `GameManager` SHALL automatically transition to `GameState.Playing` if it initializes (Start) within a designated gameplay scene (e.g., "SampleScene") while currently in `GameState.Menu`.

#### Scenario: Direct scene entry in Editor
- **WHEN** the "SampleScene" is played directly from the Unity Editor
- **THEN** the `GameManager` MUST set its state to `GameState.Playing` to enable gameplay logic and death processing

### Requirement: Room Lifecycle via Service
The creation and management of room states SHALL be handled by the `RoomService` class, isolating these operations from HTTP concerns.

#### Scenario: Room Creation
- **WHEN** a room creation request is received by the `RoomController`
- **THEN** it MUST call the `RoomService.createRoom` method which returns a unique room ID and stores the initial host state.

### Requirement: Unified Room State
The system SHALL ensure that any change to the room state is synchronized through the service layer to prevent race conditions and ensure data consistency.

#### Scenario: Joining a room
- **WHEN** a join request is received
- **THEN** the `RoomService` checks the room's current capacity and user status before allowing the join and updating the room state.

## ADDED Requirements

### Requirement: Competitive Survival State Transition
In Online mode, the match SHALL NOT transition to the Results screen until both players have reached the `Death` state.

#### Scenario: Full game over
- **WHEN** all players in a room have died or disconnected
- **THEN** the `GameManager` SHALL transition to `GameState.GameOver` and show the results screen.
