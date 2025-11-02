namespace Models
{
    class Person : ORMModel<Person>
    {
        public override int? Id { set; get; }
        public override string Name { set; get; }

        public int age { set; get; }

        public static Table GetTable() => new Table("Person", [ // TODO: Derive from GetFields()
            ["ID", "Int32"],
            ["name", "String"],
            ["age", "Int32"]
        ]);

        protected override Table TableI() => GetTable();

        protected override Dictionary<string, object> GetFields() => new()
        {
            ["ID"] = Id,
            ["Name"] = Name,
            ["age"] = age
        };

        public Person(string Name, int age)
        {
            this.Name = Name;
            this.age = age;
        }
    }
}