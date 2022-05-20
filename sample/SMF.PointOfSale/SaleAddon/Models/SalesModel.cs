// <copyright file="SaleModel.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.PointOfSale.SaleAddon.Models;
/// <summary>
/// Define Sale Model.
/// </summary>
public partial class SaleModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaleModel"/> class.
    /// </summary>
    public SaleModel()
    {
    }

    /// <summary>
    /// Used for calculate Age of Personadfasdfasdfa
    /// </summary>
    public SMFields.String Name => new() { Name = "Field Name", DefaultValueSql = "getTime()", DefaultValue = "", Index = true, IsReadOnly = false };

    /// <summary>
    /// Used for calculate Age of Person
    /// </summary>
    public SMFields.Id Age => new();

    /// <summary>
    /// Gets description for Model.
    /// </summary>
    public SMFields.Id DescriptionMethod { get; } = new() { IsReadOnly = true, Index = true };
}
