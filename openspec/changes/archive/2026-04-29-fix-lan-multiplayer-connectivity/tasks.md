## 1. Refactorización de Detección de IP en Unity

- [x] 1.1 Importar `System.Net.NetworkInformation` en `LANDiscoveryManager.cs`.
- [x] 1.2 Implementar nueva lógica en `GetLocalIPv4()` para filtrar por tipo de interfaz y estado operacional.
- [x] 1.3 Asegurar que se descarten las IPs de bucle local (127.0.0.1) e interfaces auto-configuradas (169.254.x.x).
- [x] 1.4 Modificar `SendBroadcastPacket()` para usar la IP filtrada en el mensaje.

## 2. Configuración del Servidor (Gateway)

- [x] 2.1 Modificar `Server/gateway/index.js` para que `server.listen` use `0.0.0.0`.
- [x] 2.2 Verificar que el logging del Gateway muestre la IP correcta al iniciar.

## 3. Validación y Pruebas

- [x] 3.1 Usar el `LANDiagnosticTest` en el editor de Unity para verificar que la IP detectada coincide con la IP real de Wi-Fi/Ethernet.
- [x] 3.2 Verificar que el broadcast UDP se recibe correctamente en la misma máquina (bucle local controlado).
- [x] 3.3 Confirmar que el Gateway es accesible vía HTTP desde el navegador usando la IP local.
