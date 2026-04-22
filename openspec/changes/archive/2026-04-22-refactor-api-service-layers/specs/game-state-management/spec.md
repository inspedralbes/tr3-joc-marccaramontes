## ADDED Requirements

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
