## ADDED Requirements

### Requirement: Transiciones de Panel mediante Desvanecimiento
El sistema SHALL utilizar transiciones de opacidad (fades) al cambiar entre el panel principal del lobby y la sala de espera.

#### Scenario: Transición exitosa a sala de espera
- **WHEN** el usuario hace clic en "Crear Sala" o "Unirse" y la solicitud HTTP es exitosa
- **THEN** el panel principal debe desvanecerse (alpha de 1 a 0) y el panel de espera debe aparecer (alpha de 0 a 1) en un periodo de tiempo definido (ej. 0.3s)

### Requirement: Feedback Interactivo de Botones
Todos los botones interactivos en el Lobby SHALL proporcionar feedback visual inmediato cuando el puntero del ratón se sitúa sobre ellos.

#### Scenario: Hover sobre el botón de retroceso
- **WHEN** el puntero del ratón entra en el área del botón "Atrás"
- **THEN** el botón debe disparar una animación de escalado o cambio de color consistente con el sistema de `ButtonHoverEffect`.

### Requirement: Énfasis Visual en el Código de Sala
El sistema SHALL resaltar visualmente el código de la sala de espera cuando este sea asignado dinámicamente.

#### Scenario: Visualización del código tras unirse
- **WHEN** el código de la sala se actualiza en el `roomCodeText`
- **THEN** el componente de texto debe realizar una animación de pulso de escala (`PulseScale`) para atraer la atención del usuario.

### Requirement: Feedback Animado de Estado de Conexión
El texto de estado SHALL animar su aparición o comportamiento para indicar que el sistema está procesando una solicitud.

#### Scenario: Error de conexión HTTP
- **WHEN** la solicitud de creación o unión a sala devuelve un error
- **THEN** el texto de estado debe cambiar su color a rojo y realizar un pulso de escala para señalar el fallo.
