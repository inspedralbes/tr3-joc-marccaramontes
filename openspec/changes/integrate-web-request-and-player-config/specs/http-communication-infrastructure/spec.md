## ADDED Requirements

### Requirement: REST Communication Channel
The system MUST provide HTTP endpoints on the Node.js server for transactional game operations.

#### Scenario: Create Room via HTTP
- **WHEN** the Unity client sends a POST request to `/api/rooms/create`
- **THEN** the server MUST respond with a 200 OK and a JSON body containing a new `roomId`

### Requirement: UnityWebRequest Wrapper
The Unity client MUST implement a centralized way to handle `UnityWebRequest` calls with proper error handling and JSON parsing.

#### Scenario: Handle API Error
- **WHEN** a UnityWebRequest returns a 404 or 500 error
- **THEN** the client MUST catch the error and display a descriptive message to the user in the UI
