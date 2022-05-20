namespace SMF.ORM.Fields;

public partial record String : Field
{
    //private readonly string? _value;

    protected override FieldKind FieldKind => FieldKind.String;
    public int Length { get; init; } = -1;

    public bool IsTrim { get; init; }

    //public string Value
    //{
    //    get
    //    {
    //        return _value ?? DefaultValue;
    //    }
    //    private set => _value = value;
    //}

    public string GetDbType()
    {
        if (Length > 0)
            return $"VARCHAR({Length})";
        return $"VARCHAR(MAX)";
    }


    public string DefaultValue { get; init; } = string.Empty;

    public override string DbType => GetDbType();
}
