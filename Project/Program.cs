using System;
using System.Runtime.ExceptionServices;
using Models;
using Models.Projects;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;

Project project = new Project(
    "project1",
    new ProjectType("ERP", true),
    "Odoo ERP",
    "ERP Solutions using Odoo",
    ProjectStatus.Active,
    DateTime.Now
);

Project project2 = project.Clone();
project2.Name = "project2";

Console.WriteLine(project2.ToString());
Console.WriteLine(project.ToString());
Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()