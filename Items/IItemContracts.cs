namespace GlpiNg.Modules.Abstractions.Items;

/// <summary>Un contrat rattaché à un objet, tel que l'onglet « Contrats » d'une fiche l'affiche.</summary>
public sealed record ItemContract(int LinkId, int ContractId, string Name, string? Number, DateTime? StartDate, DateTime? EndDate);

/// <summary>Un contrat qu'on peut rattacher (pas encore lié à l'objet).</summary>
public sealed record ContractOption(int Id, string Name);

/// <summary>
/// Contrats rattachés à un objet — onglet « Contrats » des fiches du parc. Rendu par le module
/// Gestion, qui porte les contrats ; le parc ne connaît que ce contrat. Rattachement polymorphe
/// (<c>ItemType</c> + <c>ItemId</c>, voir <see cref="ItemTypes"/>) comme les documents et les notes.
/// </summary>
public interface IItemContracts
{
    Task<IReadOnlyList<ItemContract>> GetForItemAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    /// <summary>Contrats pas encore rattachés à cet objet, pour le sélecteur d'ajout.</summary>
    Task<IReadOnlyList<ContractOption>> GetAvailableAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    Task AttachAsync(string itemType, int itemId, int contractId, CancellationToken cancellationToken = default);

    Task DetachAsync(int linkId, CancellationToken cancellationToken = default);

    /// <summary>Objets rattachés à un contrat — onglet « Éléments affectés » de sa fiche.</summary>
    Task<IReadOnlyList<RelationTarget>> GetItemsAsync(int contractId, CancellationToken cancellationToken = default);
}
