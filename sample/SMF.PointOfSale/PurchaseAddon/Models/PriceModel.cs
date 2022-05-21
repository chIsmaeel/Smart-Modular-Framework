namespace SMF.PointOfSale.PurchaseAddon.Models;
public partial class PriceModel
{
    public PriceModel()
    {
        //OrderBy = new((NameID, Order.Descending), (Description, Order.Ascending), (Age, Order.Descending));
    }
    public SMFields.String? NameID { get; set; } = new() { Index = true, Length = 5, DefaultValue = "Name" };
    public SMFields.String? Description { get; } = new() { Index = true, Length = 50, DefaultValue = "Description" };
    public SMFields.Int? Age { get; }
}
