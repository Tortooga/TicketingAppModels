using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Utilities;

namespace Models
{
    public abstract class ORMModel<T> where T : ORMModel<T>
    {
        public abstract int? Id { set; get; }
        public abstract string Name { set; get; }
        protected abstract Table TableI();
        protected abstract Dictionary<string, object> GetFields();

        public bool Record(bool force = false)
        {
            if ((this.Id != null && !force) || (this.Id == null && force))
            {
                Table.log($"Error: Could not write record {this.Name} to {TableI().name} (Check the force settings)");
                return false;
            }

            Writer writer = new Writer(TableI());
            bool successFlag = true;
            var fields = GetFields();
            string[] record = new string[fields.Count - 1]; // We dont need to allocate space for Id

            for (int i = 1; i < record.Length + 1; i++)
            {
                record[i - 1] = fields.ElementAt(i).Value.ToString();
            }

            if (force)
            {
                if (writer.write(record, Id) == null)
                {
                    successFlag = false;
                }
            }
            else
            {
                this.Id = writer.write(record);
            }

            if (this.Id == null || !successFlag)
            {
                Table.log($"Error: Could not write record {this.Name} to {TableI().name}");
                return false;
            }
            return true;
        }

        public static List<T>? getAll(Table table) // TODO: make the table argument derived
        {
            List<T> objects = new List<T>();
            Parser parser = new Parser(table);
            string[][]? records = parser.ParseTable();

            if (records == null || records.Length < 2)
            {
                return null;
            }
            T? curObject;
            object?[] typedFields = new object[records[0].Length - 1]; //Using the length of the first element as reference
            foreach (string[] record in records)
            {
                for (int fieldInd = 1; fieldInd < record.Length; fieldInd++)
                {
                    if ((typedFields[fieldInd - 1] = Utilities.TypeConversion.ConvertTo(record[fieldInd], table.fields[fieldInd][1])) == null) //Getting the field Name and converting the field into it
                    {
                        throw new Exception($"Warning: Null value at Table: {table.name}, Object ID: {record[0]}");
                    }
                }
                foreach (object field in typedFields)
                {
                    Console.WriteLine(field.ToString());
                }
                curObject = (T)Activator.CreateInstance(typeof(T), typedFields); //TODO: There isnt an issue but fix this warning
                curObject.Id = Int32.Parse(record[0]); //Assigning the ID
                objects.Add(curObject);
            }

            return objects;
        }
        
        public static bool TryParse(string[] record, Table table, ref T obj) //TODO Implement TryParse on getAll
        {
            if (record.Count() < 2)
            {
                Table.log($"Error(non-fatal): Record in {table.name}, of ID {record[0]}, has less than 2 fields");
                return false;
            }
            object?[] typedFields = new object[record.Length - 1];
            for (int fieldInd = 1; fieldInd < record.Length; fieldInd++)
            {
                typedFields[fieldInd - 1] = Utilities.TypeConversion.ConvertTo(record[fieldInd], table.fields[fieldInd][1]); //Getting the field Name and converting the field into it
                if (typedFields[fieldInd - 1] == null) 
                {
                    return false;
                }
            }

            T tryObject; //Made to ensure obj does not get partialy initialised before a throw.
            try
            {
                tryObject = (T)Activator.CreateInstance(typeof(T), typedFields); //TODO: There isnt an issue but fix this warning
                tryObject.Id = Int32.Parse(record[0]); //Assigning the ID

                //if tryObject didnt throw
                obj = tryObject;
                return true;
            }

            catch (Exception ex)
            {
                Table.log(ex.Message);
                return false;
            }

        }
        //TODO: Test
        public string? DeleteRecord()
        {
            Writer writer = new Writer(TableI());
            if (this.Id == null)
            {
                Table.log($"Warning: could not delete record at {TableI().name}, it does not have an id and does not exist in the table");
                return "Record has no ID";
            }
            if (!writer.deleteRecord((int)this.Id))
            {
                return "An Error Has Occured";
            }

            this.Id = null;
            return null;
        }


        public T Clone()
        {
            return (T)this.MemberwiseClone();
        }
    }
}