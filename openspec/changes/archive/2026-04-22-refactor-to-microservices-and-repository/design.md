## Context

El sistema actual consiste en un servidor monolítico Node.js que utiliza Socket.io para la comunicación en tiempo real y Express para la API REST. El cliente de Unity utiliza un wrapper manual para comunicarse con este servidor. Los requisitos técnicos exigen una evolución hacia microservicios, el uso del patrón Repository para la persistencia y la eliminación de capas intermedias en la comunicación de red (WebSockets nativos).

## Goals / Non-Goals

**Goals:**
- Desacoplar la lógica de API (HTTP) de la lógica de partida (WebSockets).
- Implementar un Gateway centralizado que actúe como proxy inverso.
- Abstraer el acceso a datos mediante el patrón Repository, permitiendo múltiples implementaciones (PostgreSQL e InMemory).
- Estandarizar el protocolo de red con JSON puro sobre WebSockets nativos.
- Integrar un agente inteligente sencillo mediante ML-Agents.

**Non-Goals:**
- Implementar un sistema de autenticación complejo (se usará identificación por username único sin login obligatorio).
- Sincronización de físicas compleja en el servidor (el estado se gestionará principalmente en los clientes con validación básica).
- Escalado dinámico de microservicios (se asume un despliegue estático inicial).

## Decisions

- **Arquitectura de Microservicios:**
  - **Gateway:** Un servicio ligero que redirige el tráfico `/api` al `API Service` y el tráfico `/ws` al `Game Service`. Oculta los puertos internos `3001` y `3002`.
  - **API Service:** Gestión de usuarios y resultados mediante HTTP.
  - **Game Service:** Gestión de salas y sincronización de estados mediante WebSockets nativos.
- **Patrón Repository:**
  - Definición de interfaces en `/common/repositories` (ej: `IUserRepository`).
  - Implementaciones separadas: `PostgresUserRepository` utilizando `pg` y `InMemoryUserRepository` para tests rápidos.
- **WebSockets Nativos:**
  - Backend: Uso de la librería `ws`.
  - Frontend: Uso de `System.Net.WebSockets.ClientWebSocket` en Unity, eliminando el parseo de paquetes de Socket.io (`40`, `42`).
- **ML-Agents:**
  - Se añadirá un Prefab `HunterAgent` con un `Behavior Parameters` y un `RayPerceptionSensor2D`.
  - El entrenamiento se realizará localmente y el modelo `.onnx` se integrará en el proyecto.

## Risks / Trade-offs

- **[Riesgo] Complejidad de gestión de múltiples procesos.** → **[Mitigación]** Creación de un script de arranque unificado y, opcionalmente, un archivo `docker-compose.yml`.
- **[Riesgo] Pérdida de funcionalidades automáticas de Socket.io (reconexión, salas).** → **[Mitigación]** Implementación manual de un sistema de salas simple en el `Game Service` y lógica de reconexión básica en el cliente C#.
- **[Riesgo] Inconsistencia de datos entre servicios.** → **[Mitigación]** Uso de la capa `common/repositories` compartida y una única base de datos PostgreSQL para asegurar la integridad.

## Migration Plan

1. **Fase 1 (Backend):** Crear la estructura de carpetas y el Gateway.
2. **Fase 2 (Data):** Implementar la capa Repository con `InMemory` inicialmente.
3. **Fase 3 (Networking):** Migrar de Socket.io a `ws` en el `Game Service` y refactorizar el cliente Unity.
4. **Fase 4 (Features):** Integrar ML-Agents en la escena de juego.
5. **Fase 5 (Persistencia):** Añadir la implementación de PostgreSQL.
