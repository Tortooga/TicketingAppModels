using System;
using System.Net;
using System.Reflection;
using Models;

namespace CLI
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
            this.Mode = Mode;

            this.Arguments = new List<String>();
            this.Options = new List<String>();
            this.Errors = new List<String>();

            this.IsValid = Parse(); //Parse() returns a bool determined by some validation steps within it
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

            if (!String.IsNullOrEmpty(token)) //Adding the last token, the previous loop only added the tokens preceding the space delimiter
            {
                tokens.Add(token);
            }

            if (tokens.Count < 1) 
            {
                Errors.Add("Error: Command is empty or invalid. Run \"help\" for command guide");
                validity = false;
                return false;
            }

            if (!IdentifyFunction(tokens[0])) //The first token is always the command function
            {
                Errors.Add("Invalid Function. Run \"help function\" for the list of possible functions");
                validity = false;
            }

            tokens.RemoveAt(0); //The Function is removed
            IdentifyArgumentsAndOptions(tokens); //Passing tokens after the Function is removed so we can iterate over arguments and options

            return validity;
        }

        private void IdentifyArgumentsAndOptions(List<string> tokens)
        {
            foreach(string token in tokens)
            {
                if (token[0] == '-') //Options are identified with - as the first charecter
                {
                    Options.Add(token.Remove(0,1));
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

        public override string ToString() //Method will be called implicitly each time an object is passed into Console.WriteLine() 
        {
            //Formating Lists as string to be Written
            string argumentsListStr = "(";
            for (int arg = 0; arg < Arguments.Count(); arg++)
            {
                if (arg == Arguments.Count() - 1) //We dont add a comma after the last term
                {
                    argumentsListStr += $"{Arguments[arg]}";
                    break;
                }
                argumentsListStr += $"{Arguments[arg]}, ";
            }

            string optionsListStr = "(";
            for (int option = 0; option < Options.Count(); option++)
            {
                if (option == Options.Count() - 1)
                {
                    optionsListStr += $"{Options[option]}";
                    break;
                }
                optionsListStr += $"{Options[option]}, ";
            }

            string errorListStr = "";
            foreach (String error in Errors)
            {
                errorListStr += $"-{error} \n";
            }

            return 
            @$"
Command: '{Command}'
Mode: {Mode}
IsValid: {IsValid}
Function: {Function}
Arguments: {argumentsListStr})
Options: {optionsListStr})
Errors: 
{errorListStr}";
        }
    }

    public enum CommandMode
    {
        Table,
        DB
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