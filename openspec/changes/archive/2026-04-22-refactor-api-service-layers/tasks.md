## 1. Project Structure Setup

- [x] 1.1 Create `Server/api-service/controllers` directory
- [x] 1.2 Create `Server/api-service/services` directory

## 2. Service Layer Implementation

- [x] 2.1 Implement `RoomService.js` to manage the `rooms` object and room logic (create, join)
- [x] 2.2 Implement `ResultService.js` to handle results saving and user stats updates
- [x] 2.3 Move password hashing logic into `RoomService` (or a security helper if needed)

## 3. Controller Layer Implementation

- [x] 3.1 Implement `RoomController.js` to handle `/api/rooms/create` and `/api/rooms/join`
- [x] 3.2 Implement `ResultController.js` to handle `/api/results`

## 4. API Service Refactoring (Wiring)

- [x] 4.1 Refactor `Server/api-service/index.js` to instantiate Repositories, Services, and Controllers
- [x] 4.2 Update routes in `index.js` to delegate to the corresponding Controllers
- [x] 4.3 Remove legacy business logic and direct repo usage from `index.js`

## 5. Validation and Testing

- [x] 5.1 Verify Room Creation from Unity (check logs in Gateway and API Service)
- [x] 5.2 Verify Room Joining from Unity
- [x] 5.3 Verify Result Reporting from Unity (check stats update in the repository)
- [x] 5.4 Run existing integration tests (if any) to ensure no regressions
