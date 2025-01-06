//we need to add a new namespace
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// i want to create a list that contains the person instances
List<Person> people = new List<Person>()
{
    new Person(1, "Martin", 33 , "oslo"),
    new Person(2, "Devrim", 48, "drammen"),
    new Person(3, "Cosmo", 5, "Oslo"),
    new Person(4, "Milo", 3, "Oslo"),
    new Person(5, "Mia", 32, "Oslo"),
    new Person(6, "Mina", 30, "Oslo"),
};


app.MapGet("/", () => "Hello World!");

app.Run();

record Person(int Id, string Name, int Age, string City);