namespace SMF.ORM.Fields;
public partial record Int : Field
{
    public int DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Integer;
}
