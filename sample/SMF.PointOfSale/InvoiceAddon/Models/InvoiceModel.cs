namespace SMF.PointOfSale.InvoiceAddon.Models;

using MNS_PoS.Domain.InvoiceAddon.Entities;

public partial class InvoiceModel
{
    public SMFields.O2M CustomerInfo => new(RegisteredModels.Customer_CustomerInfoModel);

    public SMFields.M2M SaleInvoiceProductDetails => new(RegisteredModels.Invoice_SaleInvoiceProductDetailModel);

    public SMFields.Float TotalPrice => new() { Compute = true };

    private partial float ComputeTotalPrice(MNS_PoS.Application.Interfaces.ISMFDbContext _context, Invoice currentObj)
    {
        float totalPrice = 0.0f;
        foreach (var pd in currentObj.SaleInvoiceProductDetails)
        {
            var temp = _context.Inventory_ProductStocks.Where(_ => _.ProductDetail.Id == pd.ProductDetail.Id).FirstOrDefault();
            temp!.AvailableQuantity -= pd.SaleQuantity;
            (_context as Microsoft.EntityFrameworkCore.DbContext)!.SaveChanges();
            totalPrice += pd.TotalPrice;
        }
        return totalPrice;
    }
}
