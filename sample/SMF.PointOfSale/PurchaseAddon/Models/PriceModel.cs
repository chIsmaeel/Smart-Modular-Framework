global using SMFields = SMF.ORM.Fields;
namespace SMF.PointOfSale.PurchaseAddon.Models;
public partial class PriceModel
{
    public PriceModel()
    {
        //OrderBy = new((NameID, Order.Descending), (Description, Order.Ascending), (Age, Order.Descending));
    }
    public SMFields.String? NameID { get; set; } = new();
    public SMFields.String? Description { get; }
    public SMFields.Int? Age { get; }
}
