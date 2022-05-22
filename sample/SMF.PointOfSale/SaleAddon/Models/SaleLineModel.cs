// <copyright file="SaleLineModel.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.PointOfSale.SaleAddon.Models;
/// <summary>
/// asdfjasdklfjkasdlfjlad
/// </summary>
public partial class SaleLineModel
{
    //partial byte[]? ComputeDescription(UnitOfWork uow)
    //{
    //    throw new NotImplementedException();
    //}
    //public SMFields.Binary? Description { get; } = new() { Compute = true };
    public SMFields.Binary? ProductName { get; } = new() { Compute = true };
    //public SMFields.O2M O2M => new(RegisteredModels.Purchase_PriceModel);

    //public SMFields.O2O Price => new(RegisteredModels.Purchase_PriceModel);

    public SMFields.M2O Price => new(RegisteredModels.Purchase_PriceModel);
    //private partial byte[]? ComputeProductName(UniPointOfSale.Domain.UnitOfWork uow, UniPointOfSale.Domain.SaleAddon.Models.SaleLine currentObj)
    //{
    //    return currentObj.ProductName;
    //}






    //private partial byte[]? ComputeDescription(UnitOfWork uow, SaleLine currentObj)
    //{
    //    return currentObj.ProductName;
    //}
    //public SMFields.M2M M2M => new(RegisteredModels.Purchase_PriceModel);


    //public SMFields.M2O M2O => new(RegisteredModels.Purchase_PriceModel);

    //private readonly ModelKind parentModel = ModelKind.Sale_Sale;
    //private readonly ModelImplementationKind[] modelImplementations = new[] {
    //ModelImplementationKind.IPurchaseLineModel,                   
    //ModelImplementationKind.IPurchaseModel,
    //ModelImplementationKind.ISaleModel,
    ////ModelImplementationKind.ISaleLineModel
    //                                                                             };

    //private readonly ModelKind inheritedModel = ModelKind.Sale_SaleModel;
    /// <summary>
    /// Gets name of the Field.
    /// </summary>
    //[SMF.Addons.Interfaces.IncludeInNewInterface("IPurch")]
    //public IField Name { get; } = new Varchar();
    //public IField? MyProperty { get; set; } = new SM.String();

    /// <summary>
    /// Gets description for Model.
    /// </summary>
    //public IField Description => new Field.Id();

    //public void MyMethod()
    //{
    //}
}
