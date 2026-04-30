## 1. Backend: Testing Infrastructure

- [ ] 1.1 Instalar `jest` y `supertest` como dependencias de desarrollo en el directorio `Server`.
- [ ] 1.2 Configurar el script `test` en el `package.json` de la raíz del servidor.
- [ ] 1.3 Crear el directorio `Server/common/tests` para alojar los tests unitarios de los repositorios.

## 2. Backend: Repository Unit Tests

- [ ] 2.1 Implementar `UserRepository.test.js` para validar `InMemoryUserRepository`.
- [ ] 2.2 Implementar `ResultRepository.test.js` para validar `InMemoryResultRepository`.
- [ ] 2.3 Crear un test unitario para `RoomService.js` inyectando el repositorio en memoria.

## 3. Backend: Robustness and Validation

- [ ] 3.1 Implementar un middleware de validación en `Server/api-service/index.js` para los endpoints POST.
- [ ] 3.2 Actualizar `Server/gateway/index.js` para capturar errores de los microservicios y devolver JSON estandarizado.
- [ ] 3.3 Refactorizar `SqliteUserRepository.js` para asegurar que las excepciones de base de datos se propaguen correctamente.

## 4. Unity: Network Error Handling

- [ ] 4.1 Actualizar `NetworkManager.cs` para capturar códigos de error HTTP en `PostRequestRoutine`.
- [ ] 4.2 Implementar un sistema de notificaciones de error simple en el `LobbyController` para mostrar mensajes del servidor.
- [ ] 4.3 Añadir lógica de reintento básica en `NativeWebSocketClient.cs` para mejorar la estabilidad de la conexión.

## 5. Final Validation

- [ ] 5.1 Ejecutar la suite de tests unitarios completa (`npm test`) y verificar que todos pasan.
- [ ] 5.2 Realizar un test de integración manual en Unity provocando errores (ej: nombre vacío) y verificando el feedback.
- [ ] 5.3 Asegurar que toda la documentación en `doc/` refleja estos cambios de calidad.
