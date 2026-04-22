## ADDED Requirements

### Requirement: Gateway Proxy
The system SHALL include a Gateway service that acts as a single point of entry for all client requests, hiding internal service ports.

#### Scenario: Route API request
- **WHEN** a client sends an HTTP request to `/api/*`
- **THEN** the Gateway SHALL forward the request to the internal API Service.

#### Scenario: Route WebSocket request
- **WHEN** a client initiates a WebSocket connection to `/ws`
- **THEN** the Gateway SHALL upgrade the connection and forward it to the internal Game Service.
