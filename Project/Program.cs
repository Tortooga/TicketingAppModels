using System;
using System.Runtime.ExceptionServices;
using Models;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;


List<Person> people = Person.getAll(Person.GetTable());

foreach (Person person in people)
{
    Console.WriteLine($"{person.id}: {person.name}: {person.age}");
}

Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()