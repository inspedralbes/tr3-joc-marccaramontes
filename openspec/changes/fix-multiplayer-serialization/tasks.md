## 1. Server-Side Fixes

- [x] 1.1 Update `ROOM_JOINED_CONFIRMED` in `Server/game-service/services/GameService.js` to stringify the payload object.
- [x] 1.2 Update `PLAYER_JOINED` broadcast in `Server/game-service/services/GameService.js` to ensure the payload is sent as a JSON string.
- [x] 1.3 Update `PLAYER_LEFT` broadcast in `Server/game-service/services/GameService.js` to stringify the payload.
- [x] 1.4 Audit and fix any other raw object broadcasts in `GameService.js` or `SocketController.js`.

## 2. Verification

- [x] 2.1 Restart the Node.js server and confirm it is listening on all ports.
- [ ] 2.2 Run the Unity Lobby and verify that creating a room successfully transitions to the Waiting Panel.
- [ ] 2.3 Verify that the room code is correctly displayed in the Unity UI.
- [ ] 2.4 Test joining the room from a second instance to ensure `PLAYER_JOINED` is parsed correctly.
