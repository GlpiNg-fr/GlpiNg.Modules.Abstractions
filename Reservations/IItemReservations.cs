using GlpiNg.Modules.Abstractions.Items;

namespace GlpiNg.Modules.Abstractions.Reservations;

/// <summary>Rythme de répétition d'une série de réservations — voir <see cref="IItemReservations.ReserveAsync"/>.</summary>
public enum ReservationRepeat
{
    /// <summary>Réservation unique.</summary>
    None,

    Daily,
    Weekly,
    Monthly,
}

/// <summary>
/// Projection d'une réservation telle qu'une fiche en a besoin pour l'afficher.
///
/// <see cref="Begin"/> et <see cref="End"/> sont des heures <b>locales murales</b> : une salle
/// réservée de 14 h à 16 h l'est à l'heure de celui qui la réserve, et non à un instant UTC qu'un
/// changement de fuseau déplacerait. À afficher avec <c>Display.LocalDateTime</c>, jamais avec
/// <c>Display.DateTime</c> — voir « Affichage des dates » dans CLAUDE.md.
/// </summary>
public sealed record ItemReservation(
    int Id,
    string ItemType,
    int ItemId,
    string ItemName,
    string? ItemUrl,
    int? UserId,
    string UserName,
    DateTime Begin,
    DateTime End,
    string? Comment,
    bool IsSeries);

/// <summary>
/// Résultat d'une tentative de réservation : l'identifiant de la première réservation créée, ou
/// le motif du refus. Un chevauchement n'est pas une anomalie du programme — c'est le cas courant
/// (deux personnes veulent le même vidéoprojecteur) —, d'où un message plutôt qu'une exception.
/// </summary>
public sealed record ReservationOutcome(int? Id, string? Error)
{
    public bool Ok => Error is null;
}

/// <summary>
/// Réservation d'un objet du parc, rendue par le module Réservations aux autres modules.
///
/// Même montage qu'<see cref="Notes.IItemNotes"/> ou <see cref="Documents.IDocumentAttachments"/>,
/// à ceci près que l'implémentation vit dans un module et non dans l'hôte : un module (Inventory,
/// Gestion) ne peut pas référencer un autre module, donc l'onglet « Réservations » d'une fiche
/// d'ordinateur passe par ce contrat. Le service est <b>facultatif</b> : résoudre
/// <c>IItemReservations?</c> plutôt que <c>IItemReservations</c> permet à une fiche de masquer
/// l'onglet quand le module est absent ou désactivé.
///
/// Le <c>itemType</c> que portent ces méthodes vient de <see cref="ItemTypes"/>. Aucune de ces
/// méthodes ne vérifie que l'appelant a le droit de voir l'objet porteur : c'est la page qui l'a
/// déjà établi avant d'afficher l'onglet, même frontière que pour les notes.
/// </summary>
public interface IItemReservations
{
    /// <summary>Vrai si l'objet a été déclaré réservable et que la réservation y est active.</summary>
    Task<bool> IsReservableAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    /// <summary>Nombre de réservations à venir ou en cours, pour le compteur d'onglet.</summary>
    Task<int> CountForItemAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    /// <summary>Réservations de l'objet, de la plus proche à la plus lointaine, passé compris.</summary>
    Task<IReadOnlyList<ItemReservation>> GetForItemAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    /// <summary>Réservations d'un utilisateur — l'onglet « Réservations » de sa fiche.</summary>
    Task<IReadOnlyList<ItemReservation>> GetForUserAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Déclare l'objet réservable, ou réactive une déclaration existante.
    ///
    /// <paramref name="itemName"/> et <paramref name="itemUrl"/> sont fournis par la fiche
    /// appelante et conservés tels quels : le module Réservations ne peut pas lire les modèles des
    /// autres modules, et une jointure polymorphe vers un type qu'il ne connaît pas n'existe pas.
    /// Conséquence assumée : un objet renommé garde son ancien nom dans la liste jusqu'à la
    /// prochaine visite de sa fiche.
    /// </summary>
    Task EnableAsync(
        string itemType,
        int itemId,
        string itemName,
        string? itemUrl,
        string? comment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retire l'objet des éléments réservables. Ses réservations partent avec lui : elles n'ont
    /// pas de sens sans l'autorisation qui les portait.
    /// </summary>
    Task DisableAsync(string itemType, int itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Réserve l'objet. <paramref name="occurrences"/> &gt; 1 avec un
    /// <paramref name="repeat"/> autre que <see cref="ReservationRepeat.None"/> crée une série
    /// (annulable d'un coup) ; un chevauchement, même sur une seule occurrence, refuse
    /// l'ensemble — une série à trous serait pire qu'un refus net.
    /// </summary>
    Task<ReservationOutcome> ReserveAsync(
        string itemType,
        int itemId,
        int? userId,
        string userName,
        DateTime begin,
        DateTime end,
        string? comment,
        ReservationRepeat repeat = ReservationRepeat.None,
        int occurrences = 1,
        CancellationToken cancellationToken = default);

    /// <summary>Annule une réservation, ou toute sa série avec <paramref name="wholeSeries"/>.</summary>
    Task CancelAsync(int reservationId, bool wholeSeries, CancellationToken cancellationToken = default);
}
