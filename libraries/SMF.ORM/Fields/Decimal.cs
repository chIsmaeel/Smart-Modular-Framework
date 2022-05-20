namespace SMF.ORM.Fields;
public partial class Decimal
{
    public decimal DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Decimal;
}
