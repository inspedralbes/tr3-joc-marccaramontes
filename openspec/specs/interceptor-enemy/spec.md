## ADDED Requirements

### Requirement: Interceptor Movement Logic
El sistema SHALL calcular una posición de destino que prediga el movimiento del jugador basándose en su velocidad actual.

#### Scenario: Predicción básica
- **WHEN** El jugador se mueve hacia la derecha a una velocidad constante
- **THEN** El enemigo interceptor SHALL apuntar a un punto por delante del jugador proporcional a su velocidad y distancia

### Requirement: Interceptor Visual Differentiation
El enemigo interceptor SHALL ser visualmente distinto del enemigo básico para que el jugador pueda identificar su comportamiento.

#### Scenario: Cambio de color
- **WHEN** Se instancia un enemigo de tipo Interceptor
- **THEN** El sistema SHALL cambiar el color del SpriteRenderer a uno predefinido (ej. Púrpura)
