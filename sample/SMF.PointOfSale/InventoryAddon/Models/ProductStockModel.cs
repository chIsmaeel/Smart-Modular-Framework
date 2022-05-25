namespace SMF.PointOfSale.InventoryAddon.Models;
public partial class ProductStockModel
{
    public SMFields.O2O Product => new(RegisteredModels.Product_ProductModel);

    public SMFields.String SKU => new() { Length = 8 };

    public SMFields.Int AvailableQuantity => new();


}
