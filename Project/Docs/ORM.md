# ORM Layer Documentation

## ORM Functions
Inheriting from ORMModel gives access to object.record(force = false) which records the fields of the object in a table in text file based storage(table is created when Type.table.initialiseTable() is ran).

ORMModel also exposed Type.getAll(Type.Table) which loads all the content of the table into memory and returns it as an array of type object.

## ORM Compatability
The ORMModel abstract class requires derived classes to override the following properties and methods:
-int? id
-string name
-Table TableI()
-Dictionary(string, object) GetFields (returns the type names, fields of the object)