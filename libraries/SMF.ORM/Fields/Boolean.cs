namespace SMF.ORM.Fields;
public partial record Boolean : Field
{
    public bool DefaultValue { get; init; } = false;

    protected override FieldKind FieldKind => FieldKind.Boolean;
}
