namespace GlpiNg.Modules.Abstractions.Items;

/// <summary>
/// Natures de rattachement d'un objet à un autre, telles que les onglets des fiches les posent
/// (« Certificats », « Domaines », « Tickets », « Analyse d'impact »...). Une nature désigne un ou
/// plusieurs types de cibles — voir <see cref="IItemRelations.TargetTypesOf"/>.
/// </summary>
public static class ItemRelationKinds
{
    public const string Certificate = "certificate";
    public const string Domain = "domain";
    public const string Appliance = "appliance";
    public const string Project = "project";
    public const string Ticket = "ticket";
    public const string Problem = "problem";
    public const string Change = "change";
    public const string Knowledge = "knowledge";

    /// <summary>Lignes téléphoniques attribuées à l'objet.</summary>
    public const string Line = "line";

    /// <summary>Instances de base de données hébergées par l'objet.</summary>
    public const string Database = "database";

    /// <summary>Objets installés dans une baie, un châssis ou sur un PDU : la cible est le contenant.</summary>
    public const string Rack = "rack";
    public const string Enclosure = "enclosure";
    public const string Pdu = "pdu";

    /// <summary>Objets sur lesquels une licence est affectée.</summary>
    public const string License = "license";

    /// <summary>Imprimantes compatibles avec une cartouche.</summary>
    public const string Printer = "printer";

    /// <summary>« Analyse d'impact » : l'objet impacte la cible (ordinateur, imprimante, applicatif...).</summary>
    public const string Impact = "impact";
}

/// <summary>Objet visé par un rattachement, avec de quoi l'afficher et y aller.</summary>
public sealed record RelationTarget(string ItemType, int ItemId, string Name, string Url);

/// <summary>
/// Rattachement posé sur un objet. <see cref="Reverse"/> vaut vrai quand l'objet est lui-même la cible
/// (« impacté par ») et non la source ; seule l'analyse d'impact lit dans les deux sens.
/// </summary>
public sealed record ItemRelationLink(int LinkId, RelationTarget Target, bool Reverse = false);

/// <summary>
/// Fournit des cibles de rattachement : implémenté par le module qui porte les objets (Gestion pour
/// les certificats, Assistance pour les tickets...), l'hôte n'en connaissant pas les modèles.
/// <b>Un module qui restreint la visibilité de ses objets doit l'appliquer ici aussi</b> (c'est le cas
/// de la base de connaissances) : sans cela, un nom d'article caché serait lisible depuis une fiche.
/// </summary>
public interface IRelationTargetSource
{
    /// <summary>Types d'objets que cette source sait chercher et résoudre.</summary>
    IReadOnlyCollection<string> SupportedTypes { get; }

    Task<IReadOnlyList<RelationTarget>> SearchAsync(string itemType, string? term, int limit, CancellationToken cancellationToken = default);

    /// <summary>Résout des identifiants en cibles ; ceux qui n'existent plus, ou qu'on n'a pas le droit de voir, sont omis.</summary>
    Task<IReadOnlyList<RelationTarget>> ResolveAsync(string itemType, IReadOnlyCollection<int> ids, CancellationToken cancellationToken = default);
}

/// <summary>Rattachements d'un objet à d'autres — onglets partagés des fiches du parc. Rendu par l'hôte.</summary>
public interface IItemRelations
{
    /// <summary>Types de cibles admis pour une nature de rattachement.</summary>
    IReadOnlyCollection<string> TargetTypesOf(string relation);

    Task<IReadOnlyList<ItemRelationLink>> GetAsync(string itemType, int itemId, string relation, CancellationToken cancellationToken = default);

    /// <summary>Cibles possibles pour la nature donnée (hors celles déjà rattachées à l'objet).</summary>
    Task<IReadOnlyList<RelationTarget>> SearchTargetsAsync(string itemType, int itemId, string relation, string? term, CancellationToken cancellationToken = default);

    Task AddAsync(string itemType, int itemId, string relation, string targetType, int targetId, CancellationToken cancellationToken = default);

    Task RemoveAsync(int linkId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Objets du parc qui pointent vers celui-ci : l'onglet « Éléments » d'un ticket liste les actifs
    /// qui lui sont rattachés depuis leur propre fiche. <see cref="ItemRelationLink.Target"/> est ici l'actif.
    /// </summary>
    Task<IReadOnlyList<ItemRelationLink>> GetIncomingAsync(string targetType, int targetId, string relation, CancellationToken cancellationToken = default);

    /// <summary>Actifs que l'on peut rattacher à cet objet, hors ceux qui le sont déjà.</summary>
    Task<IReadOnlyList<RelationTarget>> SearchIncomingAsync(string targetType, int targetId, string relation, string? term, CancellationToken cancellationToken = default);
}

/// <summary>Informations de gestion d'un objet (onglet « Gestion ») : achat, garantie, fournisseur, budget.</summary>
public sealed record ItemManagementData(
    int? SupplierId,
    int? BudgetId,
    DateTime? PurchaseDate,
    DateTime? DeliveryDate,
    DateTime? UseDate,
    int? WarrantyMonths,
    string? WarrantyInfo,
    decimal? Value,
    string? OrderNumber,
    string? DeliveryNumber);

public sealed record ManagementOption(int Id, string Name);

/// <summary>
/// Informations financières et administratives d'un objet, rendues par l'hôte. Dates saisies sur la
/// fiche : valeurs murales, à afficher avec <c>Display.LocalDate</c>, jamais converties.
/// </summary>
public interface IItemManagement
{
    Task<ItemManagementData> GetAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    Task SaveAsync(string itemType, int itemId, ItemManagementData data, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ManagementOption>> GetSuppliersAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ManagementOption>> GetBudgetsAsync(CancellationToken cancellationToken = default);

    /// <summary>Objets achetés auprès d'un fournisseur (<c>supplier</c>) ou imputés à un budget (<c>budget</c>).</summary>
    Task<IReadOnlyList<RelationTarget>> GetItemsAsync(string kind, int id, CancellationToken cancellationToken = default);
}
