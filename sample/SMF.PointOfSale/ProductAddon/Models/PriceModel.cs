namespace SMF.PointOfSale.ProductAddon.Models;
public partial class PriceModel
{
    public SMFields.O2O Products => new(RegisteredModels.Product_ProductModel) { };
    public SMFields.Float SalePrice => new();

    public SMFields.Float PurchasePrice => new();

    public SMFields.Float Profit => new() { Compute = true };

    private partial float ComputeProfit(ISMFDbContext _context, Price currentObj)
    {
        return currentObj.SalePrice - currentObj.PurchasePrice;
    }
}
