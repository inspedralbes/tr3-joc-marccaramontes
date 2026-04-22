## Why

El proyecto actual utiliza una arquitectura monolítica basada en Socket.io que no cumple con los requisitos técnicos de modularidad, integridad de datos y control de bajo nivel. Esta transición es necesaria para implementar una arquitectura de microservicios robusta, el patrón Repository para la persistencia y una comunicación por WebSockets nativos más eficiente, además de integrar capacidades de IA mediante ML-Agents.

## What Changes

- **BREAKING**: Sustitución de Socket.io por WebSockets nativos (librería `ws` en Node.js y `ClientWebSocket` en Unity).
- **BREAKING**: División del servidor monolítico en tres microservicios: Gateway (Proxy Inverso), API Service (HTTP/Express) y Game Service (WebSockets).
- **BREAKING**: Implementación del Patrón Repository para las entidades `User`, `Game` y `Result`.
- Introducción de PostgreSQL como motor de persistencia para producción y una implementación `InMemory` para testing.
- Integración de un agente "Hunter" utilizando ML-Agents en la escena de juego.
- Implementación de un Gateway que centraliza el acceso externo y oculta los puertos internos de los microservicios.
- Eliminación del sistema de "fake" Socket.io en Unity a favor de un protocolo JSON puro.

## Capabilities

### New Capabilities
- `microservice-infrastructure`: Implementación del Gateway y la comunicación entre servicios.
- `repository-pattern-persistence`: Capa de acceso a datos desacoplada con soporte para PostgreSQL e InMemory.
- `native-websocket-protocol`: Protocolo de comunicación de tiempo real de bajo nivel para sincronización de partida.
- `ml-agents-hunter`: Agente de IA entrenado para perseguir a los jugadores en la escena.

### Modified Capabilities
- `network-core`: Migración del transporte de Socket.io a WebSockets nativos.
- `game-state-management`: Adaptación de la gestión de estados al nuevo flujo distribuido y persistencia mediante repositorios.
- `matchmaking`: Refactorización de la creación y unión a salas para usar el nuevo API Service.

## Impact

- **Backend**: Rediseño completo de la arquitectura del servidor y la capa de datos.
- **Frontend**: Refactorización profunda de `SocketIOClient.cs` y los managers de red en Unity.
- **Seguridad**: Mejora en la protección de puertos y encriptación de datos sensibles.
- **Dependencias**: Se añaden `ws`, `pg` (o `sequelize`/`typeorm`), `bcrypt` en el backend; y el paquete ML-Agents en Unity.
