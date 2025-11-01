using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
namespace Models
{
    public class Writer{
        public Table table {get; init;}
        string tablePath;
        public Writer(Table _table){
            table = _table;
            tablePath = Table.rootPath + @$"\Base\{_table.name}.txt";
        }

        public int? write(string[] record, int? id = -1){
            //Assigning a unique id to the record
            bool forcedFlag = true;
            int? nextId = getIndex();

            if (id == -1)
            {
                forcedFlag = false;
                id = nextId;
            }
            else
            {
                Parser parser = new Parser(this.table);
                if (parser.find((int)id) != null)
                {
                    Table.log($"Error: Could not force write a record in {table.name} with id {id}, a record with that id already exists");
                    return null;
                }
            }
            
            string[] identifiedRecord = new string[record.Length + 1];
            identifiedRecord[0] = id.ToString();
            for (int i = 0; i < record.Length; i++){
                identifiedRecord[i + 1] = record[i];
            }

            //Validating the record
            if (!this.validateRecord(identifiedRecord)){
                string recordSummary = "";
                for (int i = 0; i < record.Length; i++)
                {
                    recordSummary += record[i];
                }
                Table.log($"Error: Could not commit record {recordSummary} to {this.table.name}");
                return null;
            }

            //writing record
            using (StreamWriter writer = new StreamWriter(this.tablePath, append: true))
            {
                for (int i = 0; i < identifiedRecord.Length - 1; i++){
                    writer.Write($"{identifiedRecord[i]}:"); //writing each cell with the cell spaeration delimiter
                }
                writer.Write($"{identifiedRecord[identifiedRecord.Length - 1]},\n"); //writing final cell with termination operator
                
            }
            if (forcedFlag && nextId > id)
            {
                idMileStone((int)nextId);
            }
            return id;
        }

        public bool deleteRecord(int id){
            Parser parser = new Parser(this.table);
            if (!parser.tableExists()){
                Table.log($"Warning: Could delete an item in {this.table.name}, table does not exist");
                return false;
            }

            long targetLineByteOffset = 0;

            using (StreamReader reader = new StreamReader(this.tablePath))
            {
                string curId = "";
                int curIdInt;
                string line = reader.ReadLine();
                long prevLineByteCount = 0;
                bool idFoundFlag = false;

                while (line != null){
                    targetLineByteOffset += prevLineByteCount;
                    prevLineByteCount = Encoding.UTF8.GetByteCount(line) + 1; //+1 for the end of line operator
                    curId = "";

                    if (line[0] == '-' || line[0] == '@'){
                        line = reader.ReadLine();
                        continue;
                    }
                    for (int i = 0; i < line.Length; i++){
                        if (line[i] == ':'){
                            break;
                        }

                        curId += line[i];
                    }
                    if (!int.TryParse(curId, out curIdInt)){
                        Table.log($"Warning: Invalid Token for id in {this.table.name}: {curId}");
                        line = reader.ReadLine();
                        continue;
                    }
                    if (curIdInt == id){
                        idFoundFlag = true;
                        break;
                    }

                    line = reader.ReadLine();
                }

                if (!idFoundFlag){
                    Table.log($"Warning: could not delete record of id {id} at {this.table.name}, record does not exist");
                    return false;
                }
            }

            using (FileStream stream = new FileStream(tablePath, FileMode.Open, FileAccess.Write))
            {
                stream.Seek(targetLineByteOffset, SeekOrigin.Begin);
                stream.WriteByte((byte)'-');
                return true;
            }
        }
        public void idMileStone(int mileStone)
        {
            using (StreamWriter writer = new StreamWriter(this.tablePath, append: true))
            {
                writer.WriteLine($"@{mileStone},");
            }
        }
        public int? getIndex(){
            using (StreamReader reader = new StreamReader(this.tablePath))
            {   
                string? finalLine = null;
                string line = reader.ReadLine();
                while (line != null){
                    if (!line.StartsWith('-'))
                    {
                        finalLine = line;
                    }
                    
                    line = reader.ReadLine();
                }
                if (finalLine == null){
                    return 0;
                }

                string index = "";
                for (int i = 0; i < finalLine.Length; i++){
                    if (finalLine[i] == '@')
                    {
                        continue;
                    }
                    if (finalLine[i] == ':' || finalLine[i] == ',' ){
                        return Int32.Parse(index) + 1;
                    }
                    index += finalLine[i];
                }
                
                Table.log($"Error: The data for {this.table.name} could not be read");
                return null;

            }
        }
        public bool validateRecord(string[] record){
            //Length Check
            if (this.table.fields.Length != record.Length){
                Table.log($"Error: Attempted to write a record with {record.Length} cells to {this.table.name}, it takes {this.table.fields.Length}");
                return false;
            }
            
            //Charecter Validation
            char[] AllowedCharSet = [' '];
            for (int cell = 0; cell < record.Length; cell++){
                bool boolflag = false;
                switch (this.table.fields[cell][1])
                {
                    case "Int32":
                        AllowedCharSet = PersistantData.IntAllowedChars;
                        break;
                    case "Single":
                        AllowedCharSet =  PersistantData.FloatAllowedChars;
                        break;
                    case "String":
                        AllowedCharSet = PersistantData.AllowedCharsArray;
                        break;
                    case "Boolean":
                        boolflag = true;
                        break;
                    default:
                        Table.log($"Table {this.table.name} has an invalid field {this.table.fields[cell][1]}");
                        return false;
                }
                if (boolflag){
                    if (record[cell] != "True" && record[cell] != "False"){
                        Table.log($"Error: attempted to record {record[cell]} to {this.table.fields[cell][0]} in {this.table.name}, this field only takese True or False");
                        return false;
                    }
                    continue;
                }

                for (int i = 0; i < record[cell].Length; i++){
                    bool invalidCharFlag = true;

                    for (int c = 0; c < AllowedCharSet.Length; c++){
                        if (AllowedCharSet[c] == record[cell][i]){
                            invalidCharFlag = false;
                        }
                    }

                    if (invalidCharFlag){
                        Table.log($"Error: attempted to record {record[cell]} to {this.table.fields[cell][0]} in {this.table.name}, {record[cell][i]} is an invalid charecter for that field");
                        return false;
                    }
                }
            }
            return true;
        }

    }
    
}