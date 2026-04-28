## Context

Actualmente, el `NetworkManager` tiene URLs de servidor estáticas:
- HTTP: `http://localhost:3000/api`
- WS: `ws://localhost:3002`

Esto causa errores de conexión en builds compartidas si el servidor no está en la misma máquina. Además, el uso de puertos diferentes (3000 y 3002) complica la configuración de red para usuarios externos.

## Goals / Non-Goals

**Goals:**
- Permitir configurar la dirección del servidor (IP o Host) dinámicamente.
- Centralizar la comunicación a través del Gateway (puerto 3000).
- Persistir la configuración localmente.

**Non-Goals:**
- Implementar un sistema de búsqueda automática de servidores (Discovery).
- Cambiar la lógica de los servicios del servidor.

## Decisions

### 1. Campo de Entrada en el Lobby
Se añadirá un `TMP_InputField` en el panel principal de la escena `Lobby` para que el usuario introduzca la dirección del servidor.

**Rationale**: Es el lugar más lógico donde el jugador decide a qué sala unirse o crear, por lo que también debe poder decidir a qué servidor conectarse.

### 2. Unificación vía Gateway (/ws)
El cliente conectará al WebSocket a través de `http://{host}:3000/ws`.

**Rationale**: El Gateway ya tiene implementado el proxy de WebSockets en la ruta `/ws`. Esto permite que solo se necesite abrir un puerto (3000) en el firewall del host para que el juego funcione completamente.

### 3. Persistencia con PlayerPrefs
La dirección del servidor se guardará bajo la clave `"ServerAddress"`.

**Rationale**: Es el método estándar de Unity para guardar pequeñas configuraciones persistentes entre sesiones sin necesidad de archivos externos complejos.

### 4. Flujo de Inicialización de Red

```
[UI Lobby] -> [Escribir IP] -> [Guardar en PlayerPrefs]
      │
      ├──> [Botón Crear/Unirse]
      │           │
      │           └──> [NetworkManager actualiza URLs]
      │           └──> [Llamada HTTP via Gateway (Port 3000)]
      │           └──> [Conexión WS via Gateway (Port 3000 /ws)]
```

## Risks / Trade-offs

- **[Risk]** → El usuario introduce una dirección mal formateada.
- **[Mitigation]** → Validar mínimamente la cadena y añadir valores por defecto (localhost) si está vacío.
- **[Risk]** → El Gateway no está configurado para aceptar conexiones externas en algunas redes.
- **[Mitigation]** → El uso del puerto 3000 es más estándar y fácil de redirigir que usar múltiples puertos.
