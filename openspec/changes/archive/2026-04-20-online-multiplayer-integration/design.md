## Context

El juego es actualmente un arcade de supervivencia para un solo jugador o cooperativo local por turnos. Se requiere una evolución hacia una arquitectura de red donde múltiples clientes puedan compartir una misma sesión de juego. La base técnica es Unity para el cliente y Node.js para el servidor.

## Goals / Non-Goals

**Goals:**
- Implementar un servidor Node.js que gestione salas de juego.
- Sincronizar la posición y disparos de los jugadores en tiempo real.
- Sincronizar la aparición (spawn) y movimiento de enemigos.
- Permitir que un jugador sea el "Host" de la lógica de juego para simplificar la autoridad del servidor.
- Mantener una latencia baja para una experiencia de juego fluida.

**Non-Goals:**
- Servidor autoritativo completo (la lógica de físicas y colisiones seguirá en el cliente por simplicidad).
- Sistema de cuentas persistentes con base de datos (se usará memoria volátil en el servidor para esta fase).
- Chat de voz o texto integrado.

## Decisions

### 1. Arquitectura de Red: Host-Relay via Node.js
- **Decisión**: El servidor Node.js actuará como un relay de mensajes y gestor de salas, mientras que el primer jugador en entrar a una sala será designado como "Host".
- **Razón**: Implementar un servidor autoritativo que corra físicas de Unity en Node.js es complejo. El modelo Host-Relay es ideal para juegos de ritmo rápido con pocos jugadores donde la confianza en el cliente es aceptable para un prototipo.
- **Alternativas**: Servidor autoritativo (demasiado costoso en desarrollo), P2P puro (problemas de NAT/Firewall).

### 2. Protocolo: Socket.io (WebSockets)
- **Decisión**: Usar Socket.io para la comunicación en tiempo real.
- **Razón**: Proporciona reconexión automática, gestión de salas (rooms) nativa y es fácil de integrar tanto en Node.js como en Unity.
- **Alternativas**: UDP crudo (más rápido pero complejo de implementar en Web), WebRTC (buena alternativa pero con configuración de señalización más pesada).

### 3. Sincronización de Enemigos
- **Decisión**: Solo el Host ejecuta el `EnemySpawner`. Cuando un enemigo aparece, el Host envía un evento `spawn_enemy` con una ID única y posición. Los clientes instancian el enemigo basándose en ese evento.
- **Razón**: Evita que los clientes tengan hordas de enemigos diferentes. Mantiene la consistencia del mundo.

### 4. Diagrama de Flujo de Conexión
```
[Cliente] --(REST: /create-room)--> [Servidor] (Crea ID de sala)
[Cliente] --(WS: join_room)-------> [Servidor] (Añade a Socket Room)
[Servidor] --(WS: player_joined)--> [Otros Clientes] (Notifica presencia)
```

## Risks / Trade-offs

- **[Riesgo] Latencia / Lag** → **Mitigación**: Implementar interpolación lineal (Lerp) en los "Ghost Players" para suavizar el movimiento recibido por red.
- **[Riesgo] Desconexión del Host** → **Mitigación**: Si el Host se desconecta, la partida termina para todos en esta fase (en el futuro se podría implementar migración de Host).
- **[Riesgo] Trampas (Cheating)** → **Mitigación**: Aceptado como trade-off por la simplicidad del modelo Host-Relay. No es crítico para un proyecto educativo/prototipo.

## Jerarquía de Prefabs Necesaria
- **PlayerOnline**: Prefab que contiene el script `NetworkIdentity`.
- **EnemyOnline**: Prefab con una ID de red para sincronizar su muerte.
