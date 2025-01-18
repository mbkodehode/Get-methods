//we need to add a new namespace
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//we neew permission to use Addcors
builder.Services.AddCors(options =>
{
    options.AddPolicy("allowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

//we need to use the cors policy we created in the services for it to work
app.UseCors("allowAll");
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
    int lastIndex = people.Any() ? people.Max(p => p.Id) + 1 : 1;
    Person newPerson = new Person(lastIndex, name, age, city);
    people.Add(newPerson);
    return Results.Ok(newPerson);
});

//find person
app.MapPost("/findperson", ([FromForm] int Id) =>
{
    var doesPersonExist = people.FirstOrDefault(p => p.Id == Id);
    if (doesPersonExist == null)
    {
        return Results.NotFound(new { message = $"person with the id {Id} does not exist" });
    }
    return Results.Json(new { Id = doesPersonExist.Id, Name = doesPersonExist.Name, Age = doesPersonExist.Age, City = doesPersonExist.City });
});

// uppdate person
app.MapPut("/updateperson", ([FromForm] int Id, [FromForm] string updateName, [FromForm] int updateAge, [FromForm] string updateCity) =>
{
    var doesPersonExist = people.FirstOrDefault(p => p.Id == Id);
    if (doesPersonExist == null)
    {
        return Results.NotFound(new { message = $"person with the id {Id} does not exist" });
    }
    var updateperson = doesPersonExist with { Name = updateName, Age = updateAge, City = updateCity };
    var index = people.FindIndex(p => p.Id == Id);
    people[index] = updateperson;
    return Results.Ok($"person with the id {Id} has been updated");
});

// delete person
app.MapDelete("/deleteperson", ([FromForm] int Id) =>
{
    var doesPersonExist = people.FirstOrDefault(p => p.Id == Id);
    //check if the person exists
    if (doesPersonExist == null)
    {
        return Results.NotFound(new { message = $"person with the id {Id} does not exist" });
    }
    //remove the person from the list
    people.Remove(doesPersonExist);
    return Results.Ok(new { message = $"person with the name {doesPersonExist.Name} has been removed" });
});

app.Run();

record Person(int Id, string Name, int Age, string City);