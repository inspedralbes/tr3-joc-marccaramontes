# Memòria del Projecte: Videojoc Multijugador 2D

## Introducció i Objectius
Aquest projecte consisteix en el disseny i desenvolupament d'un videojoc multijugador en 2D que connecta un client realitzat en Unity amb un backend robust en Node.js. L'objectiu principal ha estat crear una experiència de joc fluida, on la comunicació entre jugadors sigui ràpida i, sobretot, que el sistema sigui capaç de gestionar errors i dades de manera professional.

## L'Arquitectura del Backend
Per a la construcció del servidor, hem optat per una estructura de **microserveis**. Això ens permet separar clarament les responsabilitats i facilitar el manteniment:

1.  **Gateway**: Actua com la "porta d'entrada". És l'encarregat de rebre totes les peticions i redirigir-les al servei corresponent, ja sigui una petició d'API o una connexió per WebSockets.
2.  **API Service**: Aquí es gestiona tota la part lògica de les sales de joc i el guardat de resultats. Utilitzem **SQLite** perquè ens dóna una persistència real i fiable sense complicar excessivament la instal·lació.
3.  **Game Service**: Aquest servei es dedica exclusivament al temps real. Gràcies als **WebSockets**, aconseguim que el moviment dels jugadors i les accions a la partida se sincronitzin pràcticament a l'instant.

### Aplicació del Patró Repository
Un dels punts clau del disseny ha estat el **desacoblament**. Hem implementat el patró Repository per separar la base de dades de la lògica de negoci. Això ens ha permès crear implementacions "InMemory" que fem servir per als tests unitaris, garantint que el sistema funciona correctament sense necessitat d'escriure fitxers reals cada vegada.

## Qualitat Tècnica i Robustesa
Ens hem pres molt seriosament que el projecte no només funcioni, sinó que sigui resistent a errors:

*   **Validació de dades**: Hem implementat middlewares que revisen cada petició. Si un jugador intenta crear una sala sense nom o falten dades, el servidor li respon amb un error clar i estructurat en lloc de fallar.
*   **Tests Unitaris amb Jest**: Hem creat una suite de proves que valida els components crítics del backend. Això ens assegura que qualsevol canvi futur no trencarà el que ja funciona.
*   **Estabilitat a Unity**: Al client de Unity, el `NetworkManager` ara sap interpretar els codis d'error del servidor. Si hi ha un problema, el jugador rep un missatge informatiu a la pantalla. També hem afegit un sistema de **reconnexió automàtica** per si la xarxa té algun petit tall.

## Funcionament de l'Entorn
Per posar-ho en marxa, només cal seguir aquests passos:
1.  **Backend**: Dins de la carpeta `Server`, instal·lem les dependències amb `npm run install-all` i arranquem amb `npm start`.
2.  **Tests**: Podem comprovar que tot està correcte executant `npm test`.
3.  **Unity**: Només cal obrir el projecte i configurar la IP del servidor (per defecte `localhost`) des del menú del Lobby.

---
*Aquesta memòria reflecteix el compromís amb les bones pràctiques de programació, posant el focus en l'arquitectura neta i la fiabilitat del sistema.*
