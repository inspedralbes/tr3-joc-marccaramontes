## Context

El proyecto es un videojuego de Unity con un backend de Node.js. Actualmente, el desarrollo se ha realizado de forma local sin control de versiones Git. Existe un repositorio remoto en GitHub (`tr3-joc-marccaramontes`) que contiene una plantilla obligatoria para el curso. Es necesario integrar el código local con este repositorio remoto sin perder los archivos de la plantilla y asegurando una subida limpia de los archivos de Unity.

## Goals / Non-Goals

**Goals:**
- Inicializar Git localmente.
- Configurar un `.gitignore` robusto para Unity y Node.js.
- Fusionar el historial local con el remoto de GitHub (GitHub Classroom).
- Mantener la estructura de carpetas obligatoria del remoto (`doc/`, `.github/`).
- Realizar el primer commit de código funcional.

**Non-Goals:**
- Refactorizar el código existente.
- Configurar pipelines de CI/CD avanzados (fuera de lo que ya traiga la plantilla).
- Resolver deudas técnicas de Unity en este paso.

## Decisions

- **Uso de `.gitignore` de GitHub para Unity**: Se utilizará la plantilla estándar de GitHub para Unity como base, añadiendo exclusiones para `node_modules` de Node.js en la carpeta `Server/`.
- **Estrategia de Fusión: `--allow-unrelated-histories`**: Dado que el repositorio remoto ya tiene commits y el local será inicializado desde cero, se usará este flag para permitir la fusión de ambos historiales durante el primer `pull`.
- **Flujo de Git**: Se trabajará directamente sobre la rama `main` para esta inicialización, asegurando que el estado "estable" local se convierta en el estado inicial del remoto.

## Risks / Trade-offs

- **[Riesgo] Sobrescritura de README.md local** → **Mitigación**: Revisar si existe un README local antes del pull; si existe, fusionar manualmente los contenidos.
- **[Riesgo] Subida de archivos pesados de Unity** → **Mitigación**: Verificar el estado de `git status` antes del primer commit para asegurar que el `.gitignore` está funcionando correctamente.
- **[Riesgo] Conflictos en archivos de la plantilla remota** → **Mitigación**: Realizar un `git checkout` de los archivos remotos si hay conflictos triviales, priorizando la estructura de la plantilla para `doc/` y `LICENSE`.
