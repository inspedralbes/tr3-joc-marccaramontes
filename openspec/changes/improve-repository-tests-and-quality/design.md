## Context

El sistema cuenta con una arquitectura de microservicios y el patrón Repository implementado en `Server/common/repositories`. Sin embargo, no hay una forma automatizada de verificar que un cambio en los repositorios no rompa la lógica de los servicios. Además, el API Service no valida estrictamente los payloads recibidos de Unity, lo que puede causar errores silenciosos en la base de datos SQLite.

## Goals / Non-Goals

**Goals:**
- Implementar una suite de tests unitarios robusta para los Repositories.
- Estandarizar el manejo de errores en el Gateway y el API Service.
- Mejorar el feedback visual en Unity ante errores de red.

**Non-Goals:**
- Migrar la base de datos de SQLite a un sistema más complejo.
- Refactorizar completamente el sistema de red de Unity.
- Implementar tests de integración end-to-end complejos en esta fase.

## Decisions

- **Test Framework: Jest**: Se elige Jest por su facilidad de configuración en proyectos Node.js y su excelente soporte para mocks e informes de cobertura.
- **Inyección de Dependencias**: Se mantendrá el patrón actual donde los Services reciben el Repository. Los tests instanciarán el Service con la versión `InMemory` del Repository correspondiente.
- **Validation Middleware**: Se implementará un sistema de validación simple en los controladores de Express para asegurar que `survivalTime` es un número y `playerName` no está vacío.
- **HTTP Error Mapping**: El Gateway traducirá los errores internos de los microservicios a códigos HTTP estándar (400 Bad Request, 500 Internal Server Error) para que Unity pueda reaccionar adecuadamente.

## Risks / Trade-offs

- **[Riesgo] Diversidad de entornos de ejecución** → **[Mitigación]** Los tests se ejecutarán en un entorno controlado dentro de la carpeta `Server` para asegurar la consistencia.
- **[Riesgo] Falsos positivos en tests InMemory** → **[Mitigación]** Se asegurará que la interfaz `IUserRepository` y `IResultRepository` sea idéntica en ambas implementaciones (InMemory y Sqlite).
- **[Riesgo] Aumento del tiempo de compilación/test** → **[Mitigación]** Los tests se mantendrán ligeros al no requerir servicios externos (DB, red).
