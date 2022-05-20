// <copyright file="PurchaseModel.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.PointOfSale.PurchaseAddon.Models;
/// <summary>
/// 
/// PurchaseLineModel
/// </summary>
public partial class PurchaseModel
{
    //private InheritType? _inheritType = InheritedModels.Purchase_PurchaseLineModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="PurchaseModel"/> class.
    /// </summary>
    public PurchaseModel()
    {
        //OrderBy = new((Description, Order.Ascending), (Description, Order.Descending));
        //InheritModel = InheritModels.Purchase_PurchaseLineModel;
    }
    /// <summary>
    /// Dos the.
    /// </summary>
    //private readonly RegModels _inheritModel = RegModels.Purchase_PurchaseLine;
    ////private readonly ModelKind parentModel = ModelKind.Purchase_PurchaseLine;
    ////private readonly ModelKind inheritedModel = ModelKind.Purchase_PurchaseLineModel;

    ///// <summary>
    ///// Gets name of the Field.
    ///// </summary>
    ////[IncludeInExistingInterface(SMF.Shared.Models.IPurchaseModel)]
    //public PurchaseModel()
    //{
    //}


    ////[IncludeInExistingInterface(ModelInterfaces.Ok)]
    //public SM.String Name { get; set; } = new() { IsRequired = false };

    //public SM.String Names { get; set; } = new() { IsRequired = false };

    ////public IString Namess { get; set; } = new SM.String() { IsRequired = true, DefaultValue = "Okkk" };
    ///// <summary>
    ///// Gets description for Model.
    ///// </summary>
    ////[ExcludeFromDefaultInterface]
    ////[IncludeInNewInterface("Ali")]

    public SMFields.Binary? Description { get; } = new() { DefaultValue = GetDefaultValue };

    /// <summary>
    /// Gets the default value.
    /// </summary>
    /// <returns>An array of byte.</returns>
    private static byte[] GetDefaultValue()
    {
        return new byte[] { 1, 2, 3 };
    }
}
public partial class PurchaseModel
{
}
