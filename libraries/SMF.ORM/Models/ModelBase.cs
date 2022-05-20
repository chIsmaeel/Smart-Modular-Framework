namespace SMF.ORM.Models;
/// <summary>
///     ModelBase.
/// </summary>
public class ModelBase
{
    /// <summary>
    /// Gets or sets the inherit model.
    /// </summary>
    public InheritModel? InheritModel { get; init; }

    public OrderBy? OrderBy { get; init; }

    //public ModelBase()
    //{
    //    OrderBy.Fields.OrderBy(x => (Order.Accesnding, x.Item2.Compute));
    //}

}

public enum Order
{
    Ascending,
    Descending
}
public record OrderBy(params (Fields.Field?, Order)[] Fields);
