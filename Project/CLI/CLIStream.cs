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
        CLICommand Command { set; get; }

        public CLIStream()
        {
            //On start setting always the same
            Command = CLICommand.EmptyCommand();
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
                    if (RecordCommand())
                    {
                        return "Invalid Arguments/Options for \"Record\"";
                    }
                    break;
                
                default:
                    return "Invalid Function";
            }

            return null;
        }

        //Record [Cells] -amount
        bool RecordCommand()
        {
            int amount = 1;

            //amount option validation
            if (Mode == CommandMode.DB)
            {
                Console.WriteLine("Cannot record, please select a table first");
                return false;
            }
            if (Command.Options.Count > 1)
            {
                Console.WriteLine("Record only takes one option: it takes -[int amount]");
                return false;
            }
            if (Command.Options.Count == 1)
            {
                if (int.TryParse(Command.Options[0], out amount))
                {
                    Console.WriteLine("Invalid value for -[int amount]");
                    return false;
                }
                if (amount < 1)
                {
                    Console.WriteLine("-[int amount] cannot be less that 1");
                }
            }

            //arguments validation
            
            return true;
        }

        void Terminate()
        {
            Console.WriteLine("Add termination");
            //Add termination
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