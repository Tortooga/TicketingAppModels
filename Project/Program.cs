using System;
using System.Runtime.ExceptionServices;
using CLI;
using Models;
using Models.Projects;


//InitialiseTables.initialise();
Testable.table.InitialiseTable();
DateTime timei = DateTime.Now;

//CLIStream stream = new();

Person person = new("ahmed", 10); 
Person.getAll(Person.GetTable());
Console.WriteLine(Person.TryParse(["Omer", "19"], Person.GetTable(), ref person));
Console.WriteLine($"{person.Name}: {person.age}");
Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()