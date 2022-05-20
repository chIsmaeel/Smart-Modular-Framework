namespace SMF.ORM.Fields;
public partial record Decimal : Field
{
    public decimal DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Decimal;
}
