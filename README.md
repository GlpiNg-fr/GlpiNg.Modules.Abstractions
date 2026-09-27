# GlpiNg.Modules.Abstractions

*[English version](README.en.md)*

Contrats partagés entre l'hôte GlpiNg, ses modules et les plugins. Un module ne dépend que de ces contrats, jamais du projet hôte.

> **Avertissement** — GlpiNg est un projet indépendant. Il n'est ni affilié à, ni approuvé,
> soutenu ou sponsorisé par Teclib' ou le projet GLPI. « GLPI » et « GLPI-Agent » sont des
> marques de leurs propriétaires respectifs ; elles ne sont citées ici que pour décrire la
> compatibilité de GlpiNg avec le protocole GLPI-Agent et l'import depuis une base GLPI.

## Contenu

- `Menu` : `IMenuProvider`, contribution au menu latéral
- `Reports` : `IReportProvider`, contribution aux rapports (`/tools/reports`)
- `Cron` : `ICronTask`, actions automatiques exécutées par le module Cron
- `Directory` : annuaire des acteurs (entités, groupes, profils, utilisateurs) et habilitations de l'utilisateur courant
- `Documents`, `Notes` : documents joints et notes d'un objet
- `Entities` : cloisonnement par entité
- `Localization` : `Tr`, traduction de l'interface (le texte français sert de clé)
- `Preferences` : préférences d'affichage de l'utilisateur (dates, fuseau...)
- `Notifications`, `Storage`, `Import`, `FieldUnicity`, `ExternalLinks` : services de l'hôte exposés aux modules

## Utilisation

Ce dépôt est un sous-module de [GlpiNg](https://github.com/GlpiNg-fr/GlpiNg), sous
`src/GlpiNg.Modules.Abstractions`. Il n'a aucune dépendance de projet.

```bash
git clone --recurse-submodules https://github.com/GlpiNg-fr/GlpiNg.git
```

## Licence

[GNU Affero General Public License v3.0](LICENSE).
