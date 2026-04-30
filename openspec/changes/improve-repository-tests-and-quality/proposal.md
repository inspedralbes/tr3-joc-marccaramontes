## Why

El proyecto requiere demostrar una calidad técnica superior, incluyendo la validación de la lógica de negocio mediante tests unitarios. Actualmente, el patrón Repository está implementado pero carece de una suite de tests automatizada que verifique la integridad de las implementaciones `InMemory` y `Sqlite`, asegurando que el desacoplamiento entre persistencia y lógica es efectivo y robusto.

## What Changes

- **Testing Suite**: Implementación de una suite de tests unitarios en el backend (Node.js) utilizando Jest.
- **Repository Validation**: Creación de tests específicos para `InMemoryUserRepository` e `InMemoryResultRepository`.
- **Global Error Handling**: Mejora de la robustez del Gateway para capturar errores inesperados y formatearlos adecuadamente para el cliente Unity.
- **Data Validation**: Refuerzo de la validación de datos en los controladores del API Service.

## Capabilities

### New Capabilities
- `repository-unit-testing`: Capacidad para ejecutar y validar la lógica de acceso a datos de forma aislada mediante implementaciones en memoria.
- `api-error-handling`: Estandarización del manejo de errores y validación de esquemas en el API Service.

### Modified Capabilities
- `network-core`: Se añadirán requisitos de validación de respuesta y manejo de códigos de error HTTP específicos.

## Impact

- **Backend**: Adición de la dependencia `jest` en `Server/package.json`. Modificación de los controladores para incluir validaciones más estrictas.
- **Unity**: El `NetworkManager.cs` deberá manejar códigos de error HTTP específicos (400, 404, 500) de forma más granular.
- **Calidad**: Mejora directa en el cumplimiento de los criterios de evaluación del proyecto.
