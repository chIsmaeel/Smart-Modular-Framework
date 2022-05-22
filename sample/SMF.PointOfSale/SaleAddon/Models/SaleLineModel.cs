// <copyright file="SaleLineModel.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.PointOfSale.SaleAddon.Models;

using MyPointOfSale.Application.Interfaces;
using MyPointOfSale.Domain.SaleAddon.Entities;

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

    //private partial byte[]? ComputeProductName(UnitOfWork uow, SaleLine currentObj)
    //{
    //    throw new NotImplementedException();
    //}
    //public SMFields.M2O Price => new(RegisteredModels.Purchase_PriceModel);
    //private partial byte[]? ComputeProductName(UniPointOfSale.Domain.UnitOfWork uow, UniPointOfSale.Domain.SaleAddon.Models.SaleLine currentObj)
    //{
    //    return currentObj.ProductName;
    //}
    /// <summary>
    /// Computes the product name.
    /// </summary>
    /// <param name="uow">The uow.</param>
    /// <param name="currentObj">The current obj.</param>
    /// <returns>A byte[]? .</returns>
    //private partial byte[]? ComputeProductName(UnitOfWork uow, SaleLine currentObj)
    //{
    //    throw new NotImplementedException();
    //}

    //private partial byte[]? ComputeProductName(MyPointOfSale.Infrastructure.UnitOfWork uow, MyPointOfSale.Domain.SaleAddon.Entities.SaleLine currentObj)
    //{
    //    MyPointOfSale.Domain.SaleAddon.Entities.SaleLine saleLine = uow.Sale_SaleLineRepository.GetByIdAsync(1).Result;
    //    throw new NotImplementedException();
    //}



    private partial byte[]? ComputeProductName(IUnitOfWork uow, SaleLine currentObj)
    {

        throw new NotImplementedException();
    }

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
