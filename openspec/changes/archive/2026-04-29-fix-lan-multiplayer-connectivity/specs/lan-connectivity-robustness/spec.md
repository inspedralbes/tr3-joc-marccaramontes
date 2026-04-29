## ADDED Requirements

### Requirement: Filtrado de Interfaces de Red Físicas
El sistema MUST ser capaz de identificar y seleccionar una dirección IPv4 que pertenezca a un adaptador de red físico (Ethernet o Wi-Fi) activo.

#### Scenario: Selección de IP en presencia de Docker/VMs
- **WHEN** el host tiene interfaces de Docker (172.x.x.x) y Wi-Fi (192.168.x.x)
- **THEN** el sistema debe seleccionar la dirección 192.168.x.x para el broadcast de descubrimiento

### Requirement: Escucha Global del Servidor
El servidor de backend (Gateway) MUST estar configurado para aceptar conexiones entrantes desde cualquier dirección IP en la red local.

#### Scenario: Conexión desde equipo externo
- **WHEN** un cliente en un PC distinto intenta conectar a la IP del servidor en el puerto 3000
- **THEN** el Gateway debe recibir y procesar la solicitud correctamente

### Requirement: Sincronización Automática de Host
El cliente MUST actualizar su configuración de conexión (`serverHost`) automáticamente al recibir un paquete de descubrimiento válido.

#### Scenario: Recepción de paquete de descubrimiento
- **WHEN** el cliente recibe un paquete UDP con el prefijo correcto y una IP válida
- **THEN** el NetworkManager debe actualizar sus URLs de HTTP y WebSocket para apuntar a esa IP
