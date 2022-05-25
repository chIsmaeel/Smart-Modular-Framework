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
    };

    public SMFields.String Description { get; } = new();

    public SMFields.Boolean IsActive => new() { DefaultValue = true };


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
