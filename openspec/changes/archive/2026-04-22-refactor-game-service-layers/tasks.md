## 1. Project Structure Setup

- [x] 1.1 Create `Server/game-service/controllers` directory
- [x] 1.2 Create `Server/game-service/services` directory

## 2. Service Layer Implementation

- [x] 2.1 Implement `GameService.js` to manage the `rooms` Map and socket logic
- [x] 2.2 Migrate broadcasting logic to the service
- [x] 2.3 Implement connection/disconnection logic in the service

## 3. Controller Layer Implementation

- [x] 3.1 Implement `SocketController.js` to handle incoming messages and route to the service

## 4. Game Service Refactoring (Wiring)

- [x] 4.1 Refactor `Server/game-service/index.js` to instantiate Repositories, Services, and Controllers
- [x] 4.2 Update the `connection` event in `index.js` to use the new Controller
- [x] 4.3 Remove legacy business logic and direct repo usage from `index.js`

## 5. Validation and Testing

- [x] 5.1 Run `Server/test-integration.js` to verify WebSocket connectivity via Gateway
- [x] 5.2 Verify player movement and game events in Unity
- [x] 5.3 Verify player disconnection cleanup
