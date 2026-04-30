## 1. Server-Side REST API Implementation

- [x] 1.1 Add Express body-parser middleware to `Server/index.js` to handle JSON requests
- [x] 1.2 Implement `POST /api/rooms/create` endpoint to generate a room and return the ID
- [x] 1.3 Implement `POST /api/rooms/join` endpoint to validate room existence and player capacity
- [x] 1.4 Implement `POST /api/results` endpoint to receive and log match survival times
- [x] 1.5 Modify the `rooms` data structure to support pre-registration of players via HTTP before they connect via WebSocket

## 2. Unity Network Layer Updates

- [x] 2.1 Add `SendRequest<T>` generic method to `NetworkManager.cs` using `UnityWebRequest`
- [x] 2.2 Create Data Transfer Objects (DTOs) for Room requests, Room responses, and Result reporting
- [x] 2.3 Refactor `NetworkManager.Connect()` to accept a `roomId` and only initiate connection after HTTP success
- [x] 2.4 Update `NetworkManager.HandleEvent` to support receiving the player's display name from the server

## 3. Lobby UI and Player Configuration

- [x] 3.1 Add a Name Input Field (TextMeshPro) to the Lobby scene UI
- [x] 3.2 Implement logic in `LobbyController.cs` to enable/disable Create/Join buttons based on Name input
- [x] 3.3 Store the player's name locally in a `Static` variable or `PlayerPrefs` for the session

## 4. Feature Integration and Refactoring

- [x] 4.1 Update `LobbyController.OnCreateRoom` to use `UnityWebRequest` instead of direct socket emission
- [x] 4.2 Update `LobbyController.OnJoinRoom` to use `UnityWebRequest` instead of direct socket emission
- [x] 4.3 Update `GameManager.cs` to call a new `NetworkManager.ReportResults()` method upon player death
- [x] 4.4 Ensure the game transition to `SampleScene` only occurs after the WebSocket connection is confirmed

## 5. Testing and Validation

- [ ] 5.1 Test room creation via HTTP and verify the `roomId` is returned correctly
- [ ] 5.2 Test joining a room with a specific name and verify it appears in the server logs
- [ ] 5.3 Verify that survival time is correctly sent to `/api/results` when the game ends
