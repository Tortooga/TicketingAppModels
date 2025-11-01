using System;
using System.IO;
namespace Models
{
    public class Parser{
        public Table table {get; init;}
        string tablePath;
        public Parser(Table _table){
            table = _table;
            tablePath = Table.rootPath + @$"\Base\{_table.name}.txt";
        }

        public static readonly long RamUseLim = 10485760; //10MB  
        public bool tableExists(){
            using (StreamReader reader = new StreamReader(Table.schemaPath)){
                string line = reader.ReadLine();
                int charindex = 1;
                string schemaTableName = "";
                while (line != null){
                    schemaTableName = ""; //reset it after each iteration
                    if (line[0] == '{'){
                        charindex = 1;
                        while (line[charindex] != ':'){
                            schemaTableName += line[charindex];
                            charindex++;
                            if (charindex >= line.Length){
                                Table.log("Warning: There is an issue at the schema of" + this.table.name);
                                break;
                            }
                        }
                        
                        if (schemaTableName == this.table.name){
                            return true;
                        }
                    }
                    line = reader.ReadLine();
                }

                return false;
            }
        }
        public string[][]? validateTableFromSchema(){

            using (StreamReader reader = new StreamReader(Table.schemaPath)){
                string line = reader.ReadLine();
                bool noExitSolutionFlag = true; //if not changed to false in the loops this will indicate the loop was not exited before the condition was met
                
                while (line != null){ //Getting the start position of the table in the file
                    if (line == ("{"+ this.table.name + ":" + this.table.fields.Length + ",")){
                        noExitSolutionFlag = false;
                        break;
                   }
                   line = reader.ReadLine();
                   
                }

                if (noExitSolutionFlag){
                    Table.log($"Error: Table {this.table.name} was not found in the schema.");
                    return null;
                }

                line = reader.ReadLine();
                string[][] fields = new string[this.table.fields.Length][];
                int lineIndex = 0;
                int currentCharIndex;
                while (line != "}"){
                    if (lineIndex >= this.table.fields.Length){
                        Table.log($"Error: Schema for {this.table.name} did not terminate when expected");
                    }
                    if (line == null){
                        Table.log($"Error: Schema file ended before {this.table.name} terminated");
                        return null;
                    }

                    fields[lineIndex] = new string[2];
                    currentCharIndex = 0;
                    fields[lineIndex][0] = ""; //initialise the entry for field name
                    while (line[currentCharIndex] != ':'){
                        if (currentCharIndex >= line.Length){
                            Table.log($"Error: Schema for {this.table.name} is invalid");
                        }
                        fields[lineIndex][0] += line[currentCharIndex];
                        currentCharIndex++;
                    }                    
                    
                    currentCharIndex++;
                    fields[lineIndex][1] = ""; //initialise the entry for value data type
                    while (line[currentCharIndex] != ','){
                        if (currentCharIndex >= line.Length){
                            Table.log($"Error: Schema for {this.table.name} is invalid");
                        }
                        fields[lineIndex][1] += line[currentCharIndex];
                        currentCharIndex++;
                    }  

                    lineIndex++;
                    line = reader.ReadLine();
                }
                return fields;
            }
        }

        public string[]? find(int id){
            string[] record = new string[this.table.fields.Length];
            string[][] tableFields = this.validateTableFromSchema();

            for (int i = 0; i < this.table.fields.Length; i++){
                if (this.table.fields[i][0] != tableFields[i][0] || this.table.fields[i][1] != tableFields[i][1]){
                    Table.log($"Error: Table {this.table.name} does not exist in this form");
                    return null;
                }
            }

            using (StreamReader reader = new StreamReader(tablePath)){
                string curId = "-1";
                string line = "";
                while (int.Parse(curId) != id){
                    curId = "";
                    line = reader.ReadLine();
                    if (line == null){
                        return null;
                    }
                    
                    if (line.StartsWith('-') || line.StartsWith('@')){
                        curId = "-1";
                        continue;
                    }

                    for (int i = 0; i < line.Length; i++){
                        if (line[i] == ':' || line[i] == ','){
                            break;
                        }
                        curId += line[i];
                    }
                }
                
                int curField = 0;
                int curChar = 0;
                while (line[curChar] != ','){
                    if (line[curChar] == ':'){
                        curField++;
                        curChar++;
                        continue;
                    }
                    record[curField] += line[curChar];
                    curChar++;
                    if (curChar >= line.Length){
                        Table.log($"Error: record {id} in {this.table.name} terminated early");
                    }
                }

                return record;

            }
        }
        public string[][]? ParseTable(){ //Parse entire table
            //Validation
            string[][] tableFields = this.validateTableFromSchema();

            for (int i = 0; i < this.table.fields.Length; i++){
                if (this.table.fields[i][0] != tableFields[i][0] || this.table.fields[i][1] != tableFields[i][1]){
                    Table.log($"Error: Table {this.table.name} does not exist in this form");
                    return null;
                }
            }

            if (!File.Exists(tablePath)){
                Table.log($"Error: The data for {this.table.name} could not be located");
                return null;
            }
            FileInfo fileInfo = new FileInfo(tablePath);
            if (fileInfo.Length > RamUseLim){
                Table.log($"Error: Size of the data file for {this.table.name} exceeds the limit of {RamUseLim} for full parsing. Try streaming it instead");
                return null;
            }
            
            //Counting the amount of records
            string tableData = File.ReadAllText(tablePath);
            int recordAmount = 0;
            for (int c = 0; c < tableData.Length; c++){
                if (tableData[c] == '\n'){
                    recordAmount++;
                }
                if (tableData[c] == '-' || tableData[c] == '@'){
                    recordAmount--;
                }
            }            
            //parsing data
            string[][] records = new string[recordAmount][];
            int charInd = 0; //current charecter index
            int fieldInd; //current field index
            bool recordIsDeleted = false;
            for (int recInd = 0; recInd < recordAmount; recInd++){
                fieldInd = 0;
                records[recInd] = new string[this.table.fields.Length]; //initialising the array for the record
                records[recInd][0] = "";
                while (tableData[charInd] != ','){
                    //deleted data handling
                    if (tableData[charInd] == '-' || tableData[charInd] == '@'){
                        recordIsDeleted = true;
                    }
                    if (recordIsDeleted){
                        charInd++;
                        if (tableData[charInd] == ','){
                            charInd++;
                            recordIsDeleted = false;
                        }
                        continue;
                    }

                    if (tableData[charInd] == ':'){
                        fieldInd++;
                        charInd++;
                        records[recInd][fieldInd] = ""; //initialising next field in record
                        continue;
                    }
                    records[recInd][fieldInd] += tableData[charInd];
                    charInd++;
                    if(charInd >= tableData.Length){
                        Table.log($"Error: data file for {this.table.name} ended before records terminated");
                        return null;
                    }
                }
                charInd += 2;
                if(charInd >= tableData.Length){
                    break;
                }
            }

            return records;
        }

        public string[][]? search(int fieldIndex, string val){//generalised version of find()
            List<string[]> records = new List<string[]>();
            using (StreamReader reader = new StreamReader(tablePath))
            {
                string line = reader.ReadLine();
                int curFieldIndex;
                int curCharIndex;
                string targetField;
                bool invalidRecordFlag;
                while (line != null){
                    if (line.StartsWith('-') || line.StartsWith("@")){
                        line = reader.ReadLine();
                        continue;
                    }
                    curCharIndex = 0;
                    curFieldIndex = 0;
                    targetField = "";
                    invalidRecordFlag = false;
                    
                    
                    while (curFieldIndex < fieldIndex)
                    {
                        if (line[curCharIndex] == ':'){
                            curFieldIndex++;
                        }
                        curCharIndex++;
                        if (curCharIndex >= line.Length){
                            invalidRecordFlag = true;
                            break;
                        }
                    }
                    if (invalidRecordFlag){
                        Table.log($"Warning: Table {this.table.name} has an  invalid record");
                        line = reader.ReadLine();
                        continue;
                    }

                    while (line[curCharIndex] != ':' && line[curCharIndex] != ','){
                        targetField += line[curCharIndex];
                        curCharIndex++;
                    }

                    if (targetField == val){
                        records.Add(ParseLine(line));
                    }

                    line = reader.ReadLine();
                }

                return records.ToArray();
            }
        }
        public string[] ParseLine(string line){ //validate before using
            string[] record = new string[this.table.fields.Length]; 
            int curField = 0;
            int curChar = 0;
            while (line[curChar] != ','){
                if (line[curChar] == ':'){
                    curField++;
                    curChar++;
                    continue;
                }
                record[curField] += line[curChar];
                curChar++;
                if (curChar >= line.Length){
                    Table.log($"Error: record in {this.table.name} terminated early");
                    return null;
                }
            }

            return record;
        }
    }
}