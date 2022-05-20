namespace SMF.ORM.Fields;

using SMF.ORM.Models;

public partial record M2M(RegisteredModel RelationshipWith) : RelationshipField
{
    /// <summary>
    /// Gets the field kind.
    /// </summary>
    protected override FieldKind FieldKind => FieldKind.Many2many;
}
