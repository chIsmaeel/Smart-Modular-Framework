namespace SMF.PointOfSale.PurchaseAddon.Models;
public partial class PriceModel
{
    public PriceModel()
    {
        //OrderBy = new((NameID, Order.Descending), (Description, Order.Ascending), (Age, Order.Descending));
    }
    //public SMFields.O2M? NameID { get; set; } = new(RegisteredModels.Purchase_PurchaseLineModel) { Index = true };
    public SMFields.String? Description { get; } = new() { Index = true, Length = 50, DefaultValue = "Description" };
    public SMFields.Int? Age { get; }
}
