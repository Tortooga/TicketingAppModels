using System;
using System.Runtime.ExceptionServices;
using CLI;
using Models;
using Models.Projects;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;

CLIStream stream = new();

Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()