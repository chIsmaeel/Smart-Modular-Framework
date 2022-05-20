namespace SMF.ORM.Fields;

public partial class DateTime
{
    public System.DateTime DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.DateTime;
}
