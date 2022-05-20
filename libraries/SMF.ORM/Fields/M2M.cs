namespace SMF.ORM.Fields;

public partial class M2M<T>
{
    protected override FieldKind FieldKind => FieldKind.Many2many;
}
