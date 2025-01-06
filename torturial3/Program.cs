//we need to add a new namespace
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

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


// get all people
app.MapGet("/people", () =>
{
    return Results.Ok(people);
});

//add a new person with post
app.MapPost("/addperson", ([FromForm] string name, [FromForm] int age, [FromForm] string city) =>
{
    if (string.IsNullOrEmpty(name) || age == 0)
    {
        return Results.BadRequest("Name and age are both required");
    }
    // tenery operator = short if else statement    this is the same as if(people.Any()) {people.Max(p=>p.Id)+1} else {1} or
    int lastIndex=people.Any() ? people.Max(p=>p.Id)+1 : 1;

    Person newPerson = new Person(lastIndex, name, age, city);
    people.Add(newPerson);
    return Results.Ok($"new person with the name {newPerson.Name} has been added");
}).DisableAntiforgery();

app.Run();

record Person(int Id, string Name, int Age, string City);