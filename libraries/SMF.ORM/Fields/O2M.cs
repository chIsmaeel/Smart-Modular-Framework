
namespace SMF.ORM.Fields;

using SMF.ORM.Models;

/// <summary>
/// The o2 m.
/// </summary>

public partial record O2M(RegisteredModel RelationshipWith) : RelationshipField
{
    /// <summary>
    /// Gets the field kind.
    /// </summary>
    protected override FieldKind FieldKind => FieldKind.One2many;
}