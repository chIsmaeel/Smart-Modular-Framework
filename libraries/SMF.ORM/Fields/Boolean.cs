namespace SMF.ORM.Fields;
public partial class Boolean
{
    public bool DefaultValue { get; init; } = false;

    protected override FieldKind FieldKind => FieldKind.Boolean;
}
