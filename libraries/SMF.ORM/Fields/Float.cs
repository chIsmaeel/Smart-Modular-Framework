namespace SMF.ORM.Fields;
public partial record Float : Field
{
    public float DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Float;
}
