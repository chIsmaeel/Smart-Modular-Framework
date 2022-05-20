// <copyright file="SaleLineModel.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.PointOfSale.SaleAddon.Models;
/// <summary>
/// asdfjasdklfjkasdlfjlad
/// </summary>
public partial class SaleLineModel
{
    public SMFields.Binary? Description { get; } = new() { };
    public SMFields.Binary? ProductName { get; } = new() { };

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
    //public IField Description => new SMFField.Id();

    //public void MyMethod()
    //{
    //}
}
