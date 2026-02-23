# OAuth.Mono

Service OAuth complet compatible Mono : authentification par tokens, stockage en memoire ou MongoDB, logging et conteneurisation Docker.

## Structure

| Fichier / Dossier | Role |
|-|-|
| `Identity/Business/` | Logique metier d'authentification |
| `Identity/Controllers/` | Controleurs de l'API OAuth |
| `Identity/IdentityProviders/` | Fournisseurs d'identite (InMemory, MongoDB) |
| `Identity/Logging/` | Couche de logging |
| `Dockerfile` | Conteneurisation du service |

## Stack

[![Stack](https://skillicons.dev/icons?i=cs,dotnet,mongodb,docker&theme=dark)](https://skillicons.dev)