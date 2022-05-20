namespace SMF.ORM.Fields;
public partial class Int
{
    public int DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Integer;
}
