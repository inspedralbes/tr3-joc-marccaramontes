## ADDED Requirements

### Requirement: Repository Isolation Testing
El sistema SHALL permitir la ejecución de tests unitarios aislados para los repositorios de usuarios y resultados utilizando implementaciones `InMemory`.

#### Scenario: Validation of InMemory Persistence
- **WHEN** se guarda un objeto en el `InMemoryUserRepository` durante un test unitario
- **THEN** la recuperación posterior mediante el método `findByUsername` SHALL devolver exactamente el mismo objeto sin interacción con la red o el disco

### Requirement: Service-Repository Interaction Test
Los servicios de negocio (`RoomService`, `ResultService`) SHALL ser testeados inyectando repositorios `InMemory` para validar su lógica interna sin efectos secundarios.

#### Scenario: Room creation logic validation
- **WHEN** se solicita la creación de una sala a `RoomService` con un mock de repositorio
- **THEN** el servicio SHALL llamar al método `save` del repositorio y devolver un ID de sala válido
