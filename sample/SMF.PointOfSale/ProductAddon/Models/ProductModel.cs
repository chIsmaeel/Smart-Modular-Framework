namespace SMF.PointOfSale.ProductAddon.Models;
public partial class ProductModel
{
    public ProductModel()
    {

    }

    public SMFields.String Title => new()
    {
        IsRequired = true,
        Index = true,
        Length = 60,
        DefaultValue = "Product 1",
    };

    public SMFields.String Description { get; } = new();

    public SMFields.Boolean? IsActive => new() { DefaultValue = true };

    public SMFields.O2M PriceO2M => new(RegisteredModels.Product_PriceModel);

    public SMFields.O2O StockO2O => new(RegisteredModels.Inventory_ProductStockModel);


    //public SMFields.M2O PriceM2M => new(RegisteredModels.Inventory_ProductStockModel) { };


    //public SMFields.M2M StockM2M => new(RegisteredModels.Product_PriceModel) { };

    public SMFields.Binary? ProductImage => new()
    {
        Compute = true,
    };

    /// <summary>
    /// Computes the product image.
    /// </summary>
    /// <param name="_context">The _context.</param>
    /// <param name="currentObj">The current obj.</param>
    /// <returns>A byte[]? .</returns>
    private partial byte[]? ComputeProductImage(ISMFDbContext _context, Product currentObj)
    {
        var path = Path.Combine($@"E:\Projects\Smart Modular Framework\sample\SMF.PointOfSale\Images", currentObj.Id.ToString(), ".png");
        if (!File.Exists(path))
            return null;
        return File.ReadAllBytes(path);
    }
}
