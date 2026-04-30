## Context

El sistema multijugador utiliza un esquema híbrido: descubrimiento por UDP (puerto 4545) y comunicación persistente por WebSockets (puerto 3000 a través de un Gateway). Actualmente, `LANDiscoveryManager` selecciona la primera dirección IPv4 disponible, lo cual es propenso a errores en máquinas con interfaces virtuales (Docker, WSL, VPN).

## Goals / Non-Goals

**Goals:**
- Garantizar que el Host anuncie una dirección IP accesible para otros dispositivos en la misma subred.
- Asegurar que el servidor Node.js acepte conexiones desde fuera de `localhost`.
- Minimizar la intervención del usuario para configurar la red.

**Non-Goals:**
- Implementar NAT Traversal o juego a través de Internet (fuera de la LAN).
- Soporte para IPv6 (se mantendrá el enfoque en IPv4 por simplicidad en LAN).

## Decisions

### 1. Filtrado Inteligente de Interfaces en Unity
Se cambiará el método `GetLocalIPv4()` en `LANDiscoveryManager.cs` para usar `System.Net.NetworkInformation.NetworkInterface`.
- **Razón:** `Dns.GetHostEntry` no proporciona información sobre el tipo de adaptador. `NetworkInterface` permite filtrar por `NetworkInterfaceType.Ethernet` y `NetworkInterfaceType.Wireless80211`.
- **Lógica:** Se iterará por todas las interfaces, descartando las que no estén operativas (`OperationalStatus.Up`), las de loopback, y priorizando aquellas con IPs en rangos privados estándar (192.168.x.x, 10.x.x.x, 172.16-31.x.x) que NO sean de interfaces virtuales conocidas.

### 2. Escucha Global del Servidor (0.0.0.0)
Se modificará el arranque del servidor en `Server/gateway/index.js` para asegurar que el método `.listen()` no restrinja el host.
- **Razón:** Por defecto, algunos entornos pueden restringir a `127.0.0.1`, impidiendo el acceso externo. Explicitamente usar `0.0.0.0` garantiza la escucha en todas las tarjetas de red.

### 3. Validación de IP en el Paquete de Descubrimiento
El paquete UDP incluirá la IP filtrada en el paso 1. El cliente usará esta IP para actualizar el `NetworkManager.serverHost`.

## Risks / Trade-offs

- **[Riesgo]** El firewall de Windows puede bloquear el puerto UDP 4545 incluso con la IP correcta.
  - **Mitigación:** Documentar la necesidad de permitir la aplicación en el firewall o usar un puerto con menos restricciones si el 4545 falla sistemáticamente.
- **[Riesgo]** Máquinas con múltiples redes activas (Ej: Ethernet y Wi-Fi simultáneos).
  - **Mitigación:** La lógica de filtrado dará prioridad a la interfaz con la métrica más baja o la primera física encontrada.
