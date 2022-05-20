namespace SMF.ORM.Fields;

public partial class M2O<T>
{
    protected override FieldKind FieldKind => FieldKind.Many2one;
}