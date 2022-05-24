// <copyright file="PurchaseLineModel.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.PointOfSale.PurchaseAddon.Models;
/// <summary>
///          PurchaseLineModel
/// 
/// </summary>
public partial class PurchaseLineModel
{
    public PurchaseLineModel()
    {
        
        InheritModel = RegisteredModels.Sale_SaleLineModel;
    }

    //private readonly ModelKind parentModel = ModelKind.Sale_SaleLine;
    //private readonly ModelImplementationKind[] modelImplementations = new[] {
    //                                                                               //ModelImplementationKind.IPurchaseLineModel,
    ////ModelImplementationKind.IPurchaseModel,
    ////ModelImplementationKind.ISaleModel,
    //ModelImplementationKind.ISaleLineModel
    //};

    //public IField Namess { get; } = new SM.String();

    /// <summary>
    /// Gets name of the Field.
    /// </summary>
    //[IncludeInNewInterface("Sbcds")]
    public SMFields.String? Name { get; } = new() { IsRequired = true, Index = true, DefaultValue = "Name" };

    public SMFields.String? Age { get; } = new() { IsRequired = true, Index = true, DefaultValue = "Name" };

    /// <summary>
    /// Gets description for Model.
    /// </summary>
    //[IncludeInExistingInterface(typeof(Sbcds))]
    //public IField? Descriptions { get; } = new SM.Id() { Name = "ali", IsReadOnly = true };
}
