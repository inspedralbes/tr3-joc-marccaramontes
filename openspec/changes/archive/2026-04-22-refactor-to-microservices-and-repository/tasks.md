## 1. Infraestructura de Microservicios y Base de Datos

- [x] 1.1 Crear la nueva estructura de carpetas en `Server/` para `gateway`, `api-service`, `game-service` y `common`.
- [x] 1.2 Configurar el `package.json` raíz y los individuales para cada servicio con las nuevas dependencias (`ws`, `express`, `pg`, `bcrypt`).
- [x] 1.3 Implementar el microservicio `Gateway` para redirigir tráfico `/api` y `/ws`.
- [x] 1.4 Definir las interfaces del Patrón Repository en `Server/common/repositories`.
- [x] 1.5 Implementar `InMemoryUserRepository` e `InMemoryResultRepository` para validación inicial.

## 2. Rediseño de la Comunicación de Red (WebSockets Nativos)

- [x] 2.1 Implementar el servidor de WebSockets nativos en `game-service` utilizando la librería `ws`.
- [x] 2.2 Definir el nuevo protocolo de mensajes JSON (ej: `{"type": "MOVE", "data": {...}}`).
- [x] 2.3 Refactorizar `SocketIOClient.cs` en Unity para usar `System.Net.WebSockets.ClientWebSocket` puro.
- [x] 2.4 Actualizar `NetworkManager.cs` en Unity para manejar el nuevo formato de mensajes y la conexión a través del Gateway.

## 3. Implementación de la API y Persistencia Real

- [x] 3.1 Migrar los endpoints REST (`/api/rooms/*`, `/api/results`) al nuevo `api-service`.
- [x] 3.2 Implementar `PostgresUserRepository` y `PostgresResultRepository` utilizando un cliente de PostgreSQL.
- [x] 3.3 Configurar la lógica de encriptación de contraseñas (aunque no haya login obligatorio) para cumplir con los requisitos técnicos.
- [x] 3.4 Validar la integridad de los datos entre el `Game Service` y el `API Service` mediante los Repositorios.

## 4. Integración de ML-Agents

- [x] 4.1 Importar el paquete ML-Agents en el proyecto de Unity.
- [x] 4.2 Crear el script `HunterAgent.cs` heredando de `Agent` para la lógica de persecución.
- [x] 4.3 Configurar el Prefab `HunterAgent` con `RayPerceptionSensor2D` y los parámetros de comportamiento.
- [x] 4.4 Implementar la sincronización del agente en red (solo el Host calcula la IA y envía la posición).

## 5. Validación y Cierre

- [x] 5.1 Realizar pruebas de integración de punta a punta (Unity -> Gateway -> Microservicios).
- [x] 5.2 Verificar que el sistema funciona correctamente con la implementación `InMemory` y con `PostgreSQL`.
- [x] 5.3 Documentar los endpoints y el protocolo de red final.


