using System;
using System.Net;
using System.Reflection;

namespace Models
{
    public class CLICommand
    {
        public readonly string Command;
        public readonly bool IsValid;
        public readonly CommandMode Mode;

        public CommandFunctions? Function { get; private set; }
        
        public List<String> Arguments { get; private set; }
        public List<String> Options { get; private set; } 
    
        public List<String> Errors { get; private set; }
        public CLICommand(string Command, CommandMode Mode)
        {
            this.Command = Command;
            this.IsValid = Parse(); //TODO: Implement Validation
            this.Mode = Mode;

            this.Arguments = new List<String>();
            this.Options = new List<String>();
            this.Errors = new List<String>();
        }

        private bool Parse() 
        {
            List<string> tokens = new List<string>();
            bool validity = true; //Validity is not directly return so as to report on all the errors in the command rather than just the first error

            string token = "";
            for (int i = 0; i < Command.Length; i++)
            {
                if (Command[i] == ' ') //Space delimited
                {
                    if (!string.IsNullOrEmpty(token)) tokens.Add(token);
                    token = "";
                    continue;
                }
                token += Command[i];
            }

            if (tokens.Count < 1) 
            {
                Errors.Add("Error: Command is empty or invalid. Run \"help\" for command guide");
                validity = false;
            }

            if (!IdentifyFunction(tokens[0])) //The first token is always the command function
            {
                Errors.Add("Invalid Function. Run \"help function\" for the list of possible functions");
                validity = false;
            }

            IdentifyArgumentsAndOptions(tokens.GetRange(1, tokens.Count - 1)); //Passing a copy of tokens where the first element(the function) is skipped.

            return validity;
        }

        private void IdentifyArgumentsAndOptions(List<string> tokens)
        {
            foreach(string token in tokens)
            {
                if (token[0] == '-') //Options are identified with - as the first charecter
                {
                    Options.Add(token);
                }
                else
                {
                    Arguments.Add(token);
                }
            }
        }
        private bool IdentifyFunction(string functionToken)
        {
            string[] commandFunctions = Enum.GetNames(typeof(CommandFunctions));
            for (int i = 0; i < commandFunctions.Length; i++)
            {
                if (functionToken == ((CommandFunctions)i).ToString())
                {
                    Function = (CommandFunctions)i;
                }
            }

             if (Function == null)
            {
                return false;
            }

            return true;
        }
    }

    public enum CommandMode
    {
        //Enter command Modes
    }
    public enum CommandFunctions //All the functions that could be invoked in CLI
    {
        Record,
        getAll,
        Delete,
        Clone, 
        Exit,
        Open,
        Create,
        help
    }
}