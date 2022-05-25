namespace SMF.PointOfSale.CustomerAddon.Models;
public partial class CustomerModel
{
    public SMFields.String FirstName => new();
    public SMFields.String LastName => new();

    public SMFields.String FullName => new() { Compute = true };

    private partial string ComputeFullName(ISMFDbContext _context, Customer currentObj)
    {
        return $"{currentObj.FirstName} {currentObj.LastName}";
    }
}
