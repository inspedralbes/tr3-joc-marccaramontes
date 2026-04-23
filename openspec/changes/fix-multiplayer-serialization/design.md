## Context

The current communication bridge between the Node.js `game-service` and the Unity client uses a `NetworkPacket` structure:
- `type`: String (event name)
- `payload`: String (event data)

The Unity client's `NativeWebSocketClient` receives this packet and immediately tries to deserialize the `payload` using `JsonUtility.FromJson<T>(payload)`. However, the server is currently sending the `payload` as a raw JavaScript object in several events (like `ROOM_JOINED_CONFIRMED`), causing the client-side deserialization to fail and return `null`.

## Goals / Non-Goals

**Goals:**
- Ensure all WebSocket messages sent from the server have a stringified `payload`.
- Fix the `NullReferenceException` in the Unity Lobby when creating or joining a room.
- Maintain the existing `NetworkPacket` protocol to avoid refactoring the entire client-side networking stack.

**Non-Goals:**
- Refactoring the `NativeWebSocketClient` to handle both object and string payloads.
- Changing the REST API serialization (which already works correctly via JSON).

## Decisions

### Decision: Server-Side Serialization Audit
We will audit `Server/game-service/services/GameService.js` and wrap every payload in `JSON.stringify()`.

**Rationale:**
Unity's `JsonUtility` is very strict. It cannot automatically parse a raw object that has been nested inside another JSON object as a string. By stringifying the payload on the server, we provide the literal string that `JsonUtility.FromJson` expects.

**Alternatives Considered:**
- Changing Unity's `NetworkPacket` to use `object` for payload: Rejected because `JsonUtility` does not support `object` or `dynamic` types well without third-party libraries like Newtonsoft.Json (which is not currently used/configured for the core WS client).

## Risks / Trade-offs

- **Risk**: Double-serialization if an already stringified payload is stringified again.
- **Mitigation**: Carefully review `GameService.js` to ensure only raw objects are passed to `JSON.stringify`.

## Flow Diagram

```
[Unity Client]                     [Node.js Server]
      |                                   |
      | ---- JOIN_ROOM (String) ------>   |
      |                                   | (1) Process Join
      |                                   | (2) Create Payload { roomId: "XYZ" }
      |                                   | (3) JSON.stringify(payload)
      | <--- CONFIRMED (String Payload) - |
      |                                   |
 (4) JsonUtility.FromJson(payload)
 (5) SUCCESS: No Exception
```
