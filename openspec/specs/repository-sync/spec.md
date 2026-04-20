## ADDED Requirements

### Requirement: Repositorio Git Inicializado
El sistema SHALL inicializar un repositorio Git en la raíz del proyecto para el control de versiones local.

#### Scenario: Inicialización de Git
- **WHEN** el comando `git init` se ejecuta en la raíz del proyecto.
- **THEN** se crea una carpeta `.git` y el proyecto está listo para el seguimiento de archivos.

### Requirement: Exclusión de Archivos Temporales
El sistema SHALL configurar un archivo `.gitignore` que excluya archivos temporales de Unity, compilaciones, archivos de usuario y dependencias de Node.js.

#### Scenario: Configuración de .gitignore
- **WHEN** el archivo `.gitignore` se crea con las reglas estándar de Unity y Node.js.
- **THEN** los archivos en `Library/`, `Temp/`, `Logs/` y `Server/node_modules/` no son rastreados por Git.

### Requirement: Sincronización con Repositorio Remoto
El sistema SHALL vincular el repositorio local con el remoto oficial del curso y fusionar la estructura base obligatoria.

#### Scenario: Vinculación y Pull del Remoto
- **WHEN** se añade el origen remoto y se realiza un `git pull origin main --allow-unrelated-histories`.
- **THEN** los archivos `doc/`, `LICENSE`, `README.md` y la carpeta `.github/` aparecen en el directorio raíz local.

### Requirement: Primer Commit y Push a Main
El sistema SHALL realizar un commit inicial de todo el código fuente actual y subirlo a la rama `main` del repositorio remoto.

#### Scenario: Subida Inicial
- **WHEN** se añaden todos los archivos (`git add .`), se hace commit y se ejecuta `git push origin main`.
- **THEN** el código del juego y del servidor es visible en el repositorio de GitHub.
