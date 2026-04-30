## Context

El proyecto actualmente tiene un `MenuController` con métodos separados para `PlayMultiplayer()` (que intenta iniciar un modo local) y `PlayOnline()` (que carga la escena `Lobby`). Esta distinción causa confusión en los usuarios que esperan que el botón principal de Multijugador los lleve a la experiencia Online.

## Goals / Non-Goals

**Goals:**
- Unificar el flujo de Multijugador hacia el modo Online.
- Asegurar que el botón de Multijugador en el menú sea funcional y consistente.
- Limpiar el código de `MenuController` para eliminar rutas de navegación redundantes.

**Non-Goals:**
- Implementar nuevas funcionalidades de red.
- Rediseñar visualmente el menú principal.

## Decisions

### 1. Redirección de `PlayMultiplayer`
Modificaremos el método `PlayMultiplayer` en `MenuController` para que realice la misma acción que `PlayOnline`.

**Rationale**: Esto asegura que si el botón en la escena de Unity está vinculado a cualquiera de los dos métodos, el resultado será el mismo (ir al Lobby). Es la forma más segura de corregir el flujo sin necesidad de re-vincular eventos en el Inspector de Unity para cada prefab de botón.

### 2. Eliminación de `PlayOnline` (Opcional)
Se mantendrá `PlayOnline` por compatibilidad si otros botones lo usan, pero ambos métodos tendrán el mismo comportamiento.

### 3. Diagrama de Flujo de Navegación

```
[Menú Principal]
       │
       ├─── [Botón Solo] ─────────▶ SceneManager.LoadScene("SampleScene") (GameMode.Solo)
       │
       └─── [Botón Multijugador] ─▶ SceneManager.LoadScene("Lobby")
```

## Risks / Trade-offs

- **[Risk]** → Si algún sistema dependía críticamente del `GameMode.Multiplayer` (local), este dejará de ser accesible.
- **[Mitigation]** → Dado que el usuario ha indicado que el modo local "no funciona" o causa problemas, la eliminación de su punto de entrada es el comportamiento deseado.
