## Why

El modo multijugador LAN actualmente no funciona de manera fiable entre diferentes máquinas de la misma red local. El problema principal es que el sistema de descubrimiento automático (UDP Broadcast) a menudo identifica interfaces de red incorrectas (como las de Docker, WSL o VirtualBox) que no son accesibles para otros dispositivos, lo que impide que los clientes se conecten al servidor.

Arreglar esto ahora es crítico para que el prototipo sea funcional en entornos de red reales y no solo en una única máquina de desarrollo.

## What Changes

- **Mejora en la Detección de IP**: Refactorización del sistema de obtención de IP local en Unity para filtrar interfaces virtuales y priorizar redes locales reales (192.168.x.x, 10.x.x.x).
- **Robusto LAN Discovery**: Actualización del sistema de broadcast para que sea más resistente a múltiples interfaces de red.
- **Configuración del Servidor**: Asegurar que el servidor Node.js (Gateway) escuche en todas las interfaces de red (`0.0.0.0`) y no solo en `localhost`.

## Capabilities

### New Capabilities
- `lan-connectivity-robustness`: Capacidad de detectar y anunciar la dirección IP correcta en entornos con múltiples adaptadores de red (virtuales y físicos).

### Modified Capabilities
- `multiplayer-lobby`: Se modifica el requisito de conexión para que sea automático y fiable en LAN, eliminando la necesidad de entrada manual de IP y asegurando la visibilidad del servidor.

## Impact

- **Assets/Networking/LANDiscoveryManager.cs**: Cambios en la lógica de obtención de IP y envío de paquetes.
- **Server/gateway/index.js**: Cambio en la configuración de escucha del servidor.
- **Experiencia de Usuario**: El flujo de "Crear sala" y "Unirse" será totalmente automático en la misma red local.
