namespace SMF.ORM.Fields;
internal record Double : Field
{
    public double DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Double;
}