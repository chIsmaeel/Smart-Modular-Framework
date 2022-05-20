
namespace SMF.ORM.Fields;
/// <summary>
/// The o2 m.
/// </summary>

public partial class O2M<T>
{
    /// <summary>
    /// Gets the relational model.
    /// </summary>
    public T? RelationalModel { get; init; }
    /// <summary>
    /// Gets the field kind.
    /// </summary>
    protected override FieldKind FieldKind => FieldKind.One2many;
}