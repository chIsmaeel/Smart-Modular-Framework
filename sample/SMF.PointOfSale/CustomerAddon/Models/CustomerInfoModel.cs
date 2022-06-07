namespace SMF.PointOfSale.CustomerAddon.Models;

using MNS_PoS.Application.Interfaces;
/// <summary>
/// Add Customer Info.
/// </summary>

public partial class CustomerInfoModel
{

    /// <summary>
    /// First Name.
    /// </summary>
    public SMFields.String FirstName => new();

    /// <summary>
    /// Last Name.
    /// </summary>

    public SMFields.String LastName => new();

    public SMFields.String Email => new();

    public SMFields.String Address => new();

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

