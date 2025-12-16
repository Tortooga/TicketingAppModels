using System;
using Models;

class Testable : ORMModel<Testable>
{
    public static readonly Table table = new Table("Testables", [
        ["ID", "Int32"],
        ["StrField", "String"],
        ["BoolField", "Boolean"],
        ["FloatField", "Single"]
    ]);

    public override int? Id { get; set; }
    public override string Name { get; set; }
    protected override Table TableI() => table;
    protected override Dictionary<string, object> GetFields() => new Dictionary<string, object>
    {
        ["Id"] = Id,
        ["StrField"] = StrField,
        ["boolField"] = BoolField,
        ["SingleField"] = SingleField
    };

    //Model Properties
    public string StrField { get; set; }
    public bool BoolField { get; set; }
    public float SingleField { get; set; }

    public Testable(string StrField, bool BoolField, float SingleField)
    {
        this.StrField = StrField;
        this.BoolField = BoolField;
        this.SingleField = SingleField;

        Name = $"TestItem {Id}";
    }
}