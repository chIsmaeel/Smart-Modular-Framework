namespace SMF.PointOfSale.ProductAddon.Models;

using MNSUAPOS.Application.Interfaces;
using MNSUAPOS.Domain.ProductAddon.Entities;

public partial class ProductDetailModel
{
    public SMFields.String ProductName => new();

    public SMFields.String Description => new();

    public SMFields.Float SalePrice => new();

    public SMFields.Float PurchasePrice => new();

    public SMFields.Float Profit => new() { Compute = true };

    public SMFields.Float ProfitPercentage => new() { Compute = true };

    private partial float ComputeProfit(ISMFDbContext _context, ProductDetail currentObj)
    {
        return currentObj.SalePrice - currentObj.PurchasePrice;
    }

    private partial float ComputeProfitPercentage(ISMFDbContext _context, ProductDetail currentObj)
    {
        return (currentObj.PurchasePrice / currentObj.SalePrice) * 100;
    }

}
