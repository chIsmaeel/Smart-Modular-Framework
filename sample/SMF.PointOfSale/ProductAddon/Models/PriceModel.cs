namespace SMF.PointOfSale.ProductAddon.Models;
public partial class PriceModel
{
    public SMFields.O2O Products => new(RegisteredModels.Product_ProductModel) { };
    public SMFields.Decimal SalePrice => new();

    public SMFields.Decimal PurchasePrice => new();

    public SMFields.Decimal Profit => new() { Compute = true };

    private partial decimal ComputeProfit(SampleApp.Application.Interfaces.ISMFDbContext _context, SampleApp.Domain.ProductAddon.Entities.Price currentObj)
    {
        return currentObj.SalePrice - currentObj.PurchasePrice;
    }
}
