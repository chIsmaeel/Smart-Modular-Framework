namespace SMF.PointOfSale.CustomerAddon.Models;

using POSTest.Application.Interfaces;
using POSTest.Domain.CustomerAddon.Entities;

public partial class CustomerModel
{
    public SMFields.String FirstName => new();
    public SMFields.String LastName => new();

    public SMFields.String FullName => new() { Compute = true };

    public SMFields.String Address => new();

    public SMFields.String City => new();

    private partial string ComputeFullName(ISMFDbContext _context, Customer currentObj)
    {
        return currentObj.FirstName + " " + currentObj.LastName;
    }
}
