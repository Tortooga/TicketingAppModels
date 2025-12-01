using System;
using System.Runtime.ExceptionServices;
using CLI;
using Models;
using Models.Projects;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;

CLICommand command = new CLICommand("Record Omer 10 15 -f10", CommandMode.Table);

Console.WriteLine(command);

Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()