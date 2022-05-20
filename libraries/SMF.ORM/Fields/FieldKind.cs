// <copyright file="FieldKind.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.ORM.Fields;

/// <summary>
/// FieldKind.
/// </summary>
public enum FieldKind
{
    /// <summary>
    /// Text.
    /// </summary>
    String,

    /// <summary>
    /// Integer.
    /// </summary>
    Integer,

    /// <summary>
    /// Decimal.
    /// </summary>
    Decimal,

    /// <summary>
    /// Boolean.
    /// </summary>
    Boolean,

    /// <summary>
    /// DateTime.
    /// </summary>
    DateTime,

    /// <summary>
    /// Binary.
    /// </summary>
    Binary,

    /// <summary>
    /// Id.
    /// </summary>
    Id,

    /// <summary>
    ///   One2one
    /// </summary>
    One2one,

    /// <summary>
    /// Many2one.
    /// </summary>
    Many2one,

    /// <summary>
    /// One2many.
    /// </summary>
    One2many,

    /// <summary>
    /// Many2many.
    /// </summary>
    Many2many,

    /// <summary>
    /// Money.
    /// </summary>
    Money,

    /// <summary>
    /// Selection.
    /// </summary>
    Selection,

    /// <summary>
    /// Monetary.
    /// </summary>
    Monetary,

    /// <summary>
    /// Html.
    /// </summary>
    Html,

    /// <summary>
    /// Date.
    /// </summary>
    Date,

    /// <summary>
    /// Image.
    /// </summary>
    Image,
}