namespace GlpiNg.Modules.Abstractions.Items;

/// <summary>Un objet rattaché à un utilisateur, tel qu'une fiche utilisateur l'affiche.</summary>
public sealed record UserItem(string ItemType, int ItemId, string Name, string Url);

/// <summary>Rubriques de la fiche utilisateur alimentées par les modules.</summary>
public static class UserItemKinds
{
    /// <summary>« Éléments utilisés » : objets dont l'utilisateur est l'utilisateur assigné.</summary>
    public const string Used = "used";

    /// <summary>« Éléments gérés » : objets dont l'utilisateur est le technicien en charge.</summary>
    public const string Managed = "managed";

    public const string Tickets = "tickets";
    public const string Problems = "problems";
    public const string Changes = "changes";
}

/// <summary>
/// Objets d'un module rattachés à un utilisateur. Le modèle utilisateur vit dans l'hôte, que les
/// modules ne peuvent pas référencer : chacun déclare donc les rubriques (<see cref="UserItemKinds"/>)
/// qu'il sait remplir, et l'hôte les assemble. Le parc rattache par le <b>nom</b> (il stocke du texte),
/// l'assistance par l'identifiant.
/// </summary>
public interface IUserItemSource
{
    IReadOnlyCollection<string> Kinds { get; }

    Task<IReadOnlyList<UserItem>> GetAsync(string kind, int userId, string userName, CancellationToken cancellationToken = default);
}
