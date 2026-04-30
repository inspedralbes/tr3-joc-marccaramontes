## Why

Actualmente, el proyecto local de Unity no está bajo control de versiones Git y carece de una sincronización con el repositorio remoto de la asignatura (`tr3-joc-marccaramontes`). Es fundamental establecer una base sólida de control de versiones que respete la estructura obligatoria del repositorio remoto (plantilla de GitHub Classroom) y que ignore correctamente los archivos pesados y temporales de Unity.

## What Changes

- **Inicialización de Git**: Configuración del repositorio local como un repositorio Git.
- **Sincronización con Remoto**: Vinculación con el repositorio `https://github.com/inspedralbes/tr3-joc-marccaramontes.git`.
- **Integración de Estructura Obligatoria**: Descarga y fusión de los archivos de la plantilla remota (`doc/`, `LICENSE`, `README.md`, `.github/`).
- **Configuración de .gitignore**: Creación de un archivo `.gitignore` optimizado para Unity (incluyendo `Library/`, `Temp/`, etc.) y Node.js (ignorar `node_modules/` en el servidor).
- **Primer Commit y Push**: Subida de la estructura base y el código actual al repositorio remoto.

## Capabilities

### New Capabilities
- `repository-sync`: Proceso de sincronización y mantenimiento de la estructura obligatoria del repositorio del curso.

### Modified Capabilities
- Ninguna.

## Impact

- **Directorio Raíz**: Se añadirán carpetas `doc/` y `.github/`, además de archivos `LICENSE`, `README.md` y `.gitignore`.
- **Flujo de Trabajo**: A partir de ahora, todos los cambios se gestionarán mediante commits de Git.
- **Seguridad**: Se evita la subida accidental de archivos temporales pesados y dependencias de Node.js.
