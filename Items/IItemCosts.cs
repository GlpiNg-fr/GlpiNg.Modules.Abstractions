namespace GlpiNg.Modules.Abstractions.Items;

/// <summary>Coût imputé à un objet (ticket, problème, changement, projet), tel que l'onglet « Coûts » l'affiche.</summary>
public sealed record ItemCostLine(int Id, string Name, DateTime? Date, decimal Amount, string? Comment);

/// <summary>
/// Coûts d'un objet — onglet « Coûts » des fiches d'assistance et de projet. Rendu par l'hôte.
/// <c>Date</c> est une valeur murale saisie à la main : <c>Display.LocalDate</c>, jamais de conversion de fuseau.
/// Les montants sont des <c>decimal</c> : de la monnaie, pas une mesure.
/// </summary>
public interface IItemCosts
{
    Task<IReadOnlyList<ItemCostLine>> GetAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    Task AddAsync(string itemType, int itemId, string name, DateTime? date, decimal amount, string? comment, CancellationToken cancellationToken = default);

    Task RemoveAsync(int costId, CancellationToken cancellationToken = default);
}
