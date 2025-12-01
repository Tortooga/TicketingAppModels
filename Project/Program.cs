using System;
using System.Runtime.ExceptionServices;
using CLI;
using Models;
using Models.Projects;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;

CLICommand command = new CLICommand("sad re 12 -1", CommandMode.Table);

Console.WriteLine(command);

Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()