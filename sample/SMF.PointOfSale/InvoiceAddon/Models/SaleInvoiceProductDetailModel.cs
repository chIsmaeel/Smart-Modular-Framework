namespace SMF.PointOfSale.InvoiceAddon.Models;

using MNSUAPOS.Domain.InvoiceAddon.Entities;

public partial class SaleInvoiceProductDetailModel
{
    public SMFields.O2O ProductDetail => new(RegisteredModels.Product_ProductDetailModel);
    public SMFields.Int SaleQuantity => new();

    public SMFields.Float TotalPrice => new() { Compute = true };

    private partial float ComputeTotalPrice(MNSUAPOS.Application.Interfaces.ISMFDbContext _context, SaleInvoiceProductDetail currentObj)
    {
        var pd = _context.Product_ProductDetails.Find(currentObj.ProductDetail_SaleInvoiceProductDetail_FK);
        return pd.SalePrice * currentObj.SaleQuantity;
    }
}
