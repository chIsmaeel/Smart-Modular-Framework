namespace SMF.ORM.Fields;

public partial class Binary
{
    public Func<byte[]>? DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Binary;
}