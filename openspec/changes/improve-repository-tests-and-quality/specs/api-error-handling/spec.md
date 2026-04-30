## ADDED Requirements

### Requirement: Request Schema Validation
El API Service SHALL validar que todos los campos obligatorios (`playerName`, `roomId`, `survivalTime`) están presentes y tienen el formato correcto antes de procesar cualquier petición.

#### Scenario: Missing playerName on room creation
- **WHEN** se recibe un POST en `/rooms/create` con el campo `playerName` vacío o ausente
- **THEN** el servidor SHALL responder con un código HTTP 400 (Bad Request) y un mensaje de error descriptivo

### Requirement: Standardized Error Responses
Todas las respuestas de error del backend SHALL seguir un formato JSON uniforme: `{"error": "string", "message": "string", "code": number}`.

#### Scenario: Server-side exception handling
- **WHEN** ocurre una excepción no controlada en cualquier microservicio
- **THEN** el Gateway SHALL interceptarla y devolver un error 500 estandarizado para evitar fugas de información técnica
