## Context

El sistema de disparo instancia balas, pero estas atraviesan a los enemigos. La investigación reveló la ausencia de un componente `Rigidbody2D` en los objetos involucrados.

## Goals

- Habilitar la detección de colisiones entre la bala y los enemigos.

## Decisions

- **Rigidbody en la Bala**: Se prefiere añadir el `Rigidbody2D` a la bala en lugar de a los enemigos para minimizar el coste de procesamiento (hay muchos más enemigos que balas simultáneas).
- **Body Type: Kinematic**: Permite colisiones basadas en triggers sin requerir fuerzas físicas complejas, ideal para proyectiles lineales.

## Risks

- **Z-Position**: Seguir garantizando que `z = 0` para que los colisionadores se toquen en el espacio 2D.
