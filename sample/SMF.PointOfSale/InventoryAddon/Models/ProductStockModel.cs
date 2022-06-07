namespace SMF.PointOfSale.InventoryAddon.Models;
/// <summary>
/// Product Stock Model
/// </summary>
public partial class ProductStockModel
{
    public SMFields.O2O ProductDetail => new(RegisteredModels.Product_ProductDetailModel);
    public SMFields.Float AvailableQuantity => new();
    public SMFields.Int LowStockAlert => new();
    public SMFields.Boolean HasLowStock => new() { Compute = true };

    private partial bool ComputeHasLowStock(ISMFDbContext _context, MNS_PoS.Domain.InventoryAddon.Entities.ProductStock currentObj)
    {
        if (currentObj.AvailableQuantity < currentObj.LowStockAlert)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}