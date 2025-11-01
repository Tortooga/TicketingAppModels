using System;
using System.IO;
namespace Models
{
    public class Table{
        public string name;
        public string[][] fields; 
        public static readonly string rootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..")) + @"\Data";
        public static string schemaPath = rootPath + @"\schema.txt";
        
        public Table(string name_field, string[][] fields_field){
            name = name_field;
            fields = fields_field;
        }

        public bool InitialiseTable(){
            FileStream schFile;
            Parser schemaParser = new Parser(this);
            string filePath = rootPath + @$"\base\{this.name}.txt";
            
            if (!File.Exists(schemaPath)){
                try
                {
                    schFile = File.Create(schemaPath);
                    schFile.Close(); //So as to open using a streamrwiter later
                }
                catch (System.Exception)
                {
                    log("Error: schema directory does not exist and could not be created.");
                    return false;
                }
                log("schema file created.");
            }
            if (schemaParser.tableExists()){
                if (!File.Exists(filePath)){
                    log($"Error: {this.name} exists in the Schema but its data could not be located");
                    return false;
                }
                log($"Warning: Could not Initiate {this.name}, it already exists.");
                return true; //if it already exists then then we can write into it regardless of its past contents
            }

            if (File.Exists(filePath)){
                log($"Error: Could not create the data file {this.name}, a file with the same name already exists");
                return false;
            }
            if (!this.validateTable()){
                log("Error: Could not initialise table: " + this.name);
                return false;
            }
                                 //Creating schema entry
            using (FileStream fileStream = new FileStream(schemaPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine("{" + $"{this.name}:{this.fields.Length},");
                    for (int i = 0; i < this.fields.Length; i++){
                        writer.WriteLine($"{this.fields[i][0]}:{this.fields[i][1]},");
                    }
                    writer.WriteLine("}");
                }
            }

                                //Creating dataBase file
            File.Create(filePath);
            
            log("DataBase table created: " + this.name);
            return true;
        }
        
        public bool validateTable(){

            bool isAllowed = false;
            for (int i = 0; i < this.name.Length; i++){
                isAllowed = false;
                for (int j = 0; j < PersistantData.AllowedCharsArray.Length; j++)
                {
                    if (this.name[i] == PersistantData.AllowedCharsArray[j]){
                        isAllowed = true;
                        break;
                    }
                }

                if(!isAllowed){
                    log("Error: Invalid Table Name: " + this.name);
                    return false;
                }
            }
            
            for (int i = 0; i < this.fields[1].Length; i++){
                isAllowed = false;
                for (int j = 0; j < PersistantData.AllowedTypesArray.Length; j++){
                    if (this.fields[i][1] == PersistantData.AllowedTypesArray[j]){
                        isAllowed = true;
                    }
                }

                if (!isAllowed){
                    log("Error: Invalid field type for " + this.fields[i][0] + ": " + this.fields[i][1]);
                    return false; 
                }
            }
            if (this.fields[0][0] != "ID" || this.fields[0][1] != "Int32"){
                log("Error: ID field not set properly for " + this.name);
                return false;
            }

            log(this.name + " was validated successfully");
            return true;
        }

        public static void log(string entry){
            string logPath = rootPath + @"\log.txt";
            if (!File.Exists(logPath)){
                File.Create(logPath);
            }
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(DateTime.Now + " | " + entry);
            }
        }
    }
}