// <copyright file="Id.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.ORM.Fields;

/// <summary>
/// Id.
/// </summary>
public record Id : Field
{
    protected override FieldKind FieldKind => FieldKind.Id;
}