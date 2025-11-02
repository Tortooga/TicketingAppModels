using System;
using System.Runtime.ExceptionServices;
using Models;
using Models.Projects;


//InitialiseTables.initialise();

DateTime timei = DateTime.Now;
/*
Project project = new Project(
    "Odoo ERP",
    new ProjectType("ERP", true),
    "Odoo ERP",
    "ERP Solutions using Odoo",
    ProjectStatus.Active,
    DateTime.Now
); */

Project.table.InitialiseTable();
List<Project> projects = Project.getAll(Project.table);

foreach (Project project in projects)
{
    Console.WriteLine($"{project.Id}: {project.Name}");
}

Console.WriteLine((DateTime.Now - timei).ToString() + " Second/s");


//Todo TEST ORM GetAll() and Record()