using System;
using System.Data;
using Models;

namespace CLI
{
    class CLIStream
    {
        CommandMode Mode { set; get; }
        bool IsProtected { set; get; } //Access type for table that prompts user verification after delete commands
        Table? CurrentTable { set; get; }
        CLICommand? Command { set; get; }

        public CLIStream()
        {
            //On start setting always the same
            Mode = CommandMode.DB;
            IsProtected = true;

            StartStream();
        }

        private void StartStream()
        {
            DateTime timei = DateTime.Now;
            string? input = " ";
            while (!String.Equals(input, "exit"))
            {
                Console.Write(GetPrefix());
                input = Console.ReadLine();
                if (String.IsNullOrEmpty(input))
                {
                    continue;
                }
                
                CLICommand command = new CLICommand(input, Mode);
                if (!command.IsValid)
                {
                    foreach (string error in command.Errors)
                    {
                        Console.WriteLine(error);
                        continue;
                    }
                }

                ExecuteCommand();
            }
        }

        string? ExecuteCommand()
        {
            if (Command == null) //is never the case
            {
                return "Invalid Command";
            }
            
            switch (Command.Function)
            {
                case CommandFunctions.Record:
                    if (!RecordValidate())
                    {
                        return "Invalid Arguments/Options for \"Record\"";
                    }
                    break;
                
                case CommandFunctions.GetAll:
                    if (!GetAllValidate())
                    {
                        return "Invalid Arguments/Options for \"Record\"";
                    }
                    break;
                default:
                    return "Invalid Function";
            }

            return null;
        }

        //Create Validation methods for each function or combine validation and evalution onto one method
        private bool RecordValidate()
        {
            return false;
        }

        private bool GetAllValidate()
        {
            return false;
        }
        private string GetPrefix()
        {
            if (Mode == CommandMode.DB || CurrentTable == null)
            {
                return "FileDB ~ ";
            }
            return $"{CurrentTable.name} ~ ";
        }
    }
}