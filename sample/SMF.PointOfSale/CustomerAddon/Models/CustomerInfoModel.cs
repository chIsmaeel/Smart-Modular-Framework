namespace SMF.PointOfSale.CustomerAddon.Models;

using MNSUAPOS.Application.Interfaces;
using MNSUAPOS.Domain.CustomerAddon.Entities;

public partial class CustomerInfoModel
{
    public SMFields.String FirstName => new();

    public SMFields.String LastName => new();

    public SMFields.String Email => new();

    public SMFields.DateTime DoB => new();

    public SMFields.Int Age => new() { Compute = true };

    public SMFields.String FullName => new()
    {
        Compute = true,
    };


    private partial int ComputeAge(ISMFDbContext _context, CustomerInfo currentObj)
    {
        if (currentObj.DoB == default)
        {
            return 0;
        }
        return DateTime.Now.Year - currentObj.DoB.Year;
    }

    private partial string ComputeFullName(ISMFDbContext _context, CustomerInfo currentObj)
    {
        return $"{currentObj.FirstName} {currentObj.LastName}";
    }
}


