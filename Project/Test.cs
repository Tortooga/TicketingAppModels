namespace Models
{
    class Person : ORMModel<Person>
    {
        public override int? id { set; get; }
        public override string name { set; get; }

        public int age { set; get; }

        public static Table GetTable() => new Table("Person", [ // TODO: Derive from GetFields()
            ["ID", "Int32"],
            ["name", "String"],
            ["age", "Int32"]
        ]);

        protected override Table TableI() => GetTable();

        protected override Dictionary<string, object> GetFields() => new()
        {
            ["ID"] = id,
            ["Name"] = name,
            ["age"] = age
        };

        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
}