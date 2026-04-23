## ADDED Requirements

### Requirement: Standardized WebSocket Serialization
The server SHALL stringify all outgoing WebSocket payloads before sending them to the client. This ensures compatibility with the Unity-side `NativeWebSocketClient` which expects the `payload` field to be a string.

#### Scenario: Server sends room join confirmation
- **WHEN** the `GameService` emits `ROOM_JOINED_CONFIRMED`
- **THEN** the `payload` MUST be a JSON-formatted string, such as `{"roomId": "ABCD"}`.

#### Scenario: Server notifies about new player
- **WHEN** a new player joins a room and `PLAYER_JOINED` is emitted
- **THEN** the `payload` MUST be a JSON-formatted string, such as `{"playerName": "Player1", "roomId": "ABCD"}`.

#### Scenario: Client receives and parses payload
- **WHEN** the Unity client receives a WebSocket packet
- **THEN** it SHALL successfully deserialize the `payload` string into the appropriate C# DTO (e.g., `RoomConfirmedData`) without throwing a `NullReferenceException`.
