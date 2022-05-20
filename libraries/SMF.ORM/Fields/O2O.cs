namespace SMF.ORM.Fields;

using SMF.ORM.Models;

public record O2O(RegisteredModel RelationshipWith) : RelationshipField
{
    /// <summary>
    /// Gets the field kind.
    /// </summary>
    protected override FieldKind FieldKind => FieldKind.One2one;
}
