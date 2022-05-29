namespace SMF.PointOfSale.ProductAddon.Models;
public partial class PriceModel
{
    public SMFields.Float SalePrice => new();

    public SMFields.Float PurchasePrice => new();

    public SMFields.Float Profit => new() { Compute = true };

    public PriceModel()
    {
    }

    private partial float ComputeProfit(ISMFDbContext _context, Price currentObj)
    {
        return currentObj.SalePrice - currentObj.PurchasePrice;
    }
}
