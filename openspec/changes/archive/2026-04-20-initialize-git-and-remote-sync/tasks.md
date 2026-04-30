## 1. Inicialización Local

- [x] 1.1 Ejecutar `git init` en la raíz del proyecto.
- [x] 1.2 Crear el archivo `.gitignore` con las reglas de Unity (GitHub template) y Node.js (Server/node_modules).
- [x] 1.3 Verificar con `git status` que las carpetas `Library/`, `Temp/`, `Logs/` y `node_modules/` están correctamente ignoradas.

## 2. Configuración del Remoto

- [x] 2.1 Añadir el origen remoto: `git remote add origin https://github.com/inspedralbes/tr3-joc-marccaramontes.git`.
- [x] 2.2 Configurar la rama principal: `git branch -M main`.
- [x] 2.3 Realizar un `git pull origin main --allow-unrelated-histories` para descargar la estructura obligatoria.
- [x] 2.4 Resolver posibles conflictos menores en archivos como `README.md` (fusionando el contenido si fuera necesario).

## 3. Primer Commit y Sincronización

- [x] 3.1 Añadir todos los archivos locales al staging: `git add .`.
- [x] 3.2 Crear el primer commit: `git commit -m "chore: initial commit with Unity project and server"`.
- [x] 3.3 Subir el repositorio local al remoto: `git push -u origin main`.
- [x] 3.4 Verificar en GitHub que el código y la estructura de carpetas (`Assets/`, `Server/`, `doc/`) son correctos.
