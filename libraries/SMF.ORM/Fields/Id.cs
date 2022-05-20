// <copyright file="Id.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.ORM.Fields;

/// <summary>
/// Id.
/// </summary>
public class Id : Field
{
    protected override FieldKind FieldKind => FieldKind.Id;
}