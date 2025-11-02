# ORM Layer Documentation

## ORM Functions
Inheriting from ORMModel gives access to object.record(force = false) which records the fields of the object in a table in text file based storage(table is created when Type.table.initialiseTable() is ran).

ORMModel also exposed Type.getAll(Type.Table) which loads all the content of the table into memory and returns it as an array of type object.

## ORM Compatability
The ORMModel abstract class requires derived classes to override the following properties and methods:

-int? Id
-string Name
-Table TableI()
-Dictionary(string, object) GetFields()

# The Constructor
The class must have a constructer that excepts data as acceptable by FileDB, So only built-in types.

The constructer may then construct the user-defined typed properties using the arguments of built-in type passed to it.

Note that a class may have multiple constructors.

# GetFields()
GetFields() returns a key value pair of the name and refrence to the properties to be stored. Inside GetFields() the properties must be converted to built-in types as acceptable by FileDB. GetFields() must provide all the required arguements of the constructor

If required objects of user defined types could be split into multiple objects of built-in type and then reconstructed in the constructor.

Objects in GetFields() must be listed in the same order as in the constructor.

# TableI()
TableI() must return the static Table of the class. Fields in the table must be ordered in the same way as in the constructor and GetFields()

For more info on Tables check Table.md

