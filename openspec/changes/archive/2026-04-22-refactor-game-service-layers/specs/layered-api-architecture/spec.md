## MODIFIED Requirements

### Requirement: Controller-Service-Repository Layered Structure
All microservices SHALL be structured into three distinct layers: Controller, Service, and Repository. This applies to both HTTP-based and WebSocket-based services.

#### Scenario: Request handling flow
- **WHEN** an HTTP request OR a WebSocket message is received by the Gateway
- **THEN** it is routed to the corresponding Controller in the respective Service, which validates the input and delegates business logic to a Service, which in turn uses a Repository for data persistence.
