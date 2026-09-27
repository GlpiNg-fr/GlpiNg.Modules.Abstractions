# GlpiNg.Modules.Abstractions

*[Version française](README.md)*

Contracts shared by the GlpiNg host, its modules and plugins. A module depends only on these contracts, never on the host project.

> **Disclaimer** — GlpiNg is an independent project. It is not affiliated with, endorsed,
> supported or sponsored by Teclib' or the GLPI project. "GLPI" and "GLPI-Agent" are trademarks
> of their respective owners; they are mentioned here only to describe GlpiNg's compatibility
> with the GLPI-Agent protocol and import from a GLPI database.

## Contents

- `Menu`: `IMenuProvider`, side-menu contributions
- `Reports`: `IReportProvider`, report contributions (`/tools/reports`)
- `Cron`: `ICronTask`, automatic actions run by the Cron module
- `Directory`: directory of principals (entities, groups, profiles, users) and the current user's authorizations
- `Documents`, `Notes`: an item's attached documents and notes
- `Entities`: entity scoping
- `Localization`: `Tr`, UI translation (the French text is the key)
- `Preferences`: the user's display preferences (dates, time zone...)
- `Notifications`, `Storage`, `Import`, `FieldUnicity`, `ExternalLinks`: host services exposed to modules

## Usage

This repository is a submodule of [GlpiNg](https://github.com/GlpiNg-fr/GlpiNg), under
`src/GlpiNg.Modules.Abstractions`. It has no project dependency.

```bash
git clone --recurse-submodules https://github.com/GlpiNg-fr/GlpiNg.git
```

## License

[GNU Affero General Public License v3.0](LICENSE).
