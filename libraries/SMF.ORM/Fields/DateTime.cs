namespace SMF.ORM.Fields;

public partial record DateTime : Field
{
    public System.DateTime DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.DateTime;
}
