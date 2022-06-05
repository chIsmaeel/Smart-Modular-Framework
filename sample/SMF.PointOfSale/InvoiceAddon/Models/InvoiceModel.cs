namespace SMF.PointOfSale.InvoiceAddon.Models;

using MNSUAPOS.Domain.InvoiceAddon.Entities;

public partial class InvoiceModel
{
    public SMFields.O2M CustomerInfo => new(RegisteredModels.Customer_CustomerInfoModel);

    public SMFields.M2M ProductDetails => new(RegisteredModels.Invoice_SaleInvoiceProductDetailModel);

    public SMFields.Float TotalPrice => new() { Compute = true };

    private partial float ComputeTotalPrice(MNSUAPOS.Application.Interfaces.ISMFDbContext _context, Invoice currentObj)
    {
        float totalPrice = 0.0f;
        foreach (var pd in currentObj.ProductDetails)
        {
            var temp = _context.Inventory_ProductStocks.Where(_ => _.ProductDetail.Id == pd.ProductDetail.Id).FirstOrDefault();
            temp!.AvailableQuantity -= pd.SaleQuantity;
            (_context as Microsoft.EntityFrameworkCore.DbContext)!.SaveChanges();
            totalPrice += pd.TotalPrice;
        }
        return totalPrice;
    }
}
