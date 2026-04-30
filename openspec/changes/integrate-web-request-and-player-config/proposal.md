## Why

The project currently uses WebSockets (Socket.io) for all communication, including room management and result reporting. To strictly comply with the academic project requirements, we need to integrate `UnityWebRequest` for punctual/non-continuous actions (creating/joining rooms and sending results). Additionally, the project lacks a way for players to configure basic parameters (like their name), which is also a requirement.

## What Changes

- **Add Player Name Configuration**: Introduce a text input field in the Lobby UI to allow players to set their display name.
- **Node.js REST API**: Implement Express.js endpoints for room creation, room joining, and result submission.
- **UnityWebRequest Integration**: Transition the Unity client's room management (Create/Join) and Game Over reporting from WebSockets to HTTP requests.
- **Socket.io Connection Flow**: Update the connection logic so the client only connects to the WebSocket server AFTER a room has been successfully created or joined via HTTP.
- **Remove Login Complexity**: As confirmed, any previous requirements for a login/authentication system are explicitly removed to focus on the core communication and configuration goals.

## Capabilities

### New Capabilities
- `http-communication-infrastructure`: Infrastructure for handling RESTful API calls between Unity and the Node.js server using `UnityWebRequest`.
- `player-configuration`: System to manage and sync basic player-defined parameters like display names.

### Modified Capabilities
- `matchmaking`: Transition the room creation and joining process from WebSocket events to HTTP requests.
- `game-over-ui`: Add the ability to report final match results (survival time) to the server via an HTTP POST request.
- `network-core`: Redefine the responsibility of the network layer to handle both HTTP (punctual actions) and WebSockets (real-time gameplay).

## Impact

- **Client**: `NetworkManager.cs` and `LobbyController.cs` will be significantly modified to use `UnityWebRequest`. New UI elements will be added to the Lobby scene.
- **Server**: `Server/index.js` will be expanded with Express routes to handle `POST /api/rooms/create`, `POST /api/rooms/join`, and `POST /api/results`.
- **API**: The communication protocol for room management will change from WebSocket events to RESTful endpoints.
