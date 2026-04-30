## Why

Unity (C#) expects the `payload` field in WebSocket messages to be a string, but the Server (Node.js) currently sends it as a raw object. This mismatch causes a `NullReferenceException` in Unity's `NativeWebSocketClient` and `NetworkManager`, preventing players from successfully creating or joining rooms.

## What Changes

- **Server-side Serialization**: Modify the `GameService` in the `game-service` to ensure all outgoing WebSocket messages have a stringified `payload`.
- **Message Standardization**: Specifically update `ROOM_JOINED_CONFIRMED`, `PLAYER_JOINED`, and `PLAYER_LEFT` events to follow the `type` (string) and `payload` (stringified JSON) protocol.
- **Client-side Verification**: Ensure Unity correctly parses these stringified payloads without throwing exceptions.

## Capabilities

### New Capabilities
- `multiplayer-serialization-fix`: Standardizes the WebSocket communication protocol by ensuring all payloads are sent as JSON strings, matching the client-side `NetworkPacket` definition.

### Modified Capabilities
- `multiplayer-lobby`: The requirement for room joining confirmation now includes a strictly typed string payload.

## Impact

- **Affected Systems**: `game-service` (WebSocket broadcasting), Unity Client (`NetworkManager` and `NativeWebSocketClient`).
- **APIs**: WebSocket protocol (internal structure change of the `payload` field).
- **Assets**: None.
