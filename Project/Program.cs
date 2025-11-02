using System;
using System.Runtime.ExceptionServices;
using Models;
using Models.Projects;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;

Project project = new Project(
    "Odoo ERP",
    new ProjectType("ERP", true),
    "Odoo ERP",
    "ERP Solutions using Odoo",
    ProjectStatus.Active,
    DateTime.Now
);

Project.table.InitialiseTable();
project.Record();
Console.WriteLine(project.Id);
Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()