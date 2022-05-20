namespace SMF.ORM.Fields;

public partial record Binary : Field
{
    public Func<byte[]>? DefaultValue { get; init; }
    protected override FieldKind FieldKind => FieldKind.Binary;
}