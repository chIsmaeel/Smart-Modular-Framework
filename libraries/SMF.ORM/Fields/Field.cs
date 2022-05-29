// <copyright file="Field.cs" company="ERP">
// Copyright (c) ERP. All rights reserved.
// </copyright>

namespace SMF.ORM.Fields;
/// <summary>
/// Fields.
/// </summary>
public abstract record Field
{
    public bool IsReadOnly { get; init; }
    public bool IsRequired { get; init; }
    public string Name { get; init; } = string.Empty;

    public bool Store { get; init; } = true;

    public bool Compute { get; init; }

    public bool Translate { get; init; }

    public bool Index { get; init; }

    public string? DefaultValueSql { get; set; }
    protected abstract FieldKind FieldKind { get; }
    //protected abstract string[] ColumnType { get; }

    public virtual string? DbType => FieldKind switch
    {
        FieldKind.Boolean => "BIT",
        FieldKind.String => $"varchar(max)",
        FieldKind.Integer => "integer",
        FieldKind.Id => "integer",
        FieldKind.Float => "float",
        FieldKind.Money => "money",
        FieldKind.Selection => "selection",
        FieldKind.DateTime => "dateTime",
        FieldKind.Monetary => "Monetary",
        //FieldKind.Char => "char",
        FieldKind.Html => "html",
        FieldKind.Date => "date",
        FieldKind.Binary => "VARBINARY",
        FieldKind.Image => "image",
        FieldKind.Many2one => "many2one",
        FieldKind.One2many => "one2many",
        FieldKind.Many2many => "many2many",
        _ => null,
    };
}